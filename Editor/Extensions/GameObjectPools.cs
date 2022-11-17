using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Assertions;

namespace MyUtils
{

    /**
     * <summary>游戏对象池，用于管理需要大量生成的对象池，不支持阻塞</summary>
     * 
     */
    public class GameObjectPools
    {

        // 可用的游戏对象列表
        private readonly Stack<GameObject> usableGameObjects;
        // 不可用的
        private readonly List<GameObject> unavailableGameObjects;
        // 创建游戏对象的函数，如果游戏对象不够使用
        private readonly Func<GameObject> createAction;
        // 从池子里面拿到对象以后处理函数
        private readonly Action<GameObject> getAction;
        // 对象返回池子的函数
        private readonly Action<GameObject> releaseAction;
        // 后期销毁的函数
        private readonly Action<GameObject> destroyAction;
        // 每次获取时检查资源是否有效
        private readonly Func<GameObject, bool> validateAction;
        // 最大的对象个数
        private readonly int maxCount;
        // 数量
        private int currentCount = 0;

        public GameObjectPools(Func<GameObject> createAction, Action<GameObject> getAction, Action<GameObject> releaseAction, Action<GameObject> destroyAction, Func<GameObject, bool> validateAction, int maxCount)
        {
            this.usableGameObjects = new Stack<GameObject>(maxCount);
            this.unavailableGameObjects = new List<GameObject>(maxCount);
            this.createAction = createAction;
            this.getAction = getAction;
            this.releaseAction = releaseAction;
            this.destroyAction = destroyAction;
            this.validateAction = validateAction;
            this.maxCount = maxCount;
        }

        /**
         * <summary>初始化一个对象池</summary>
         * <param name="createAction">每次创建对象的函数</param>
         * <param name="getAction">每次从池子中获取对象后的处理函数，默认不做任何处理</param>
         * <param name="releaseAction">每次放入池子后需要处理的时间函数，比如释放资源啥的，默认是将Active设置成false</param>
         * <param name="destroyAction">销毁对象</param>
         * <param name="validateAction">每次获取对象之前的验证函数，默认是返回true，如果返回false，则任务是无效，需要重新处理</param>
         * <param name="maxCount">最大数量，如果达到最大数量以后，就不会创建新的</param>
         */
        public static GameObjectPools Of(Func<GameObject> createAction, Action<GameObject> getAction, Action<GameObject> releaseAction, Action<GameObject> destroyAction, Func<GameObject, bool> validateAction, int maxCount)
        {
            Assert.AreEqual(maxCount > 0, true, $"MaxCount[{maxCount}] must be ge zero.");
            Assert.IsNotNull(createAction, "CreateAction is not null.");
            Assert.IsNotNull(destroyAction, "DestroyAction is not null.");
            return new GameObjectPools(
                createAction,
                getAction == null ? (t => { }) : getAction,
                releaseAction == null ? t => t.SetActive(false) : releaseAction,
                destroyAction,
                validateAction == null ? t => true : validateAction,
                maxCount
                );
        }

        /**
         * <summary>通过注解初始化对象池</summary>
         * <param name="target">对象池依赖的对象，注解都定义在此对象上</param>
         * <param name="maxCount">最大数量</param>
         */
        public static GameObjectPools Of(MonoBehaviour target, int maxCount)
        {
            MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            Func<GameObject> createAction = null;
            Action<GameObject> getAction = null;
            Action<GameObject> releaseAction = null;
            Action<GameObject> destroyAction = t => GameObject.Destroy(t);
            Func<GameObject, bool> validateAction = null;
            // 检查是否有 attribute 注解
            foreach (MethodInfo info in methods)
            {
                System.Attribute attr = info.GetCustomAttribute(typeof(GOPoolActionAttribute));
                GOPoolActionType type = ((GOPoolActionAttribute)attr).ActionType;
                switch (type)
                {
                    case GOPoolActionType.CreateAction:
                        createAction = () => (GameObject)info.Invoke(target, ArrayUtils.NewNoop<object>());
                        break;

                    case GOPoolActionType.GetAction:
                        getAction = t => info.Invoke(target, ArrayUtils.AsObject(t));
                        break;

                    case GOPoolActionType.ReleaseAction:
                        releaseAction = t => info.Invoke(target, ArrayUtils.AsObject(t));
                        break;

                    case GOPoolActionType.DestroyAction:
                        destroyAction = t => info.Invoke(target, ArrayUtils.AsObject(t));
                        break;

                    default:
                        validateAction = t => (bool)info.Invoke(target, ArrayUtils.AsObject(t));
                        break;
                }
            }
            return Of(createAction, getAction, releaseAction, destroyAction, validateAction, maxCount);

        }



        /**
         * <summary>获取一个池子对象</summary>
         * <returns>结果</returns>
         */
        public Optional<GameObject> Get()
        {
            // 先看看有没有可用的
            GameObject go = null;
            while (true)
            {
                go = usableGameObjects.Pop();
                if (go == null)
                {
                    break;
                }
                //检查是否可用
                if (validateAction(go))
                {
                    // 调用一下初始化
                    getAction(go);
                    //将可用对象设置成不可用
                    unavailableGameObjects.Add(go);
                    // 如果可用，则直接退出
                    return Optional<GameObject>.Of(go);
                }
                else
                {
                    //销毁失效的
                    //计数--
                    currentCount--;
                    //销毁对象
                    destroyAction(go);
                }
                // 如果不可用则循环获取，直到把池子里面的元素都获取玩
            }
            // 如果没有可用的，则创建
            return createAndUsed();
        }

        /**
         * <summary>释放资源</summary>
         * <param name="source">要移除的对象</param>
         * <returns>处理结果，true成功，false失败</returns>
         */
        public bool Release(GameObject source)
        {
            Assert.IsNotNull(source, "GameObjectPool remove element is not null.");
            // 如果移除成功
            if (unavailableGameObjects.Remove(source))
            {
                releaseAction(source);
                //放入可用函数中
                usableGameObjects.Push(source);
                return true;
            }
            return false;
        }

        private Optional<GameObject> createAndUsed()
        {
            // 如果已经达到最大，则直接返回空
            if (currentCount >= maxCount)
            {
                return Optional<GameObject>.OfNullable();
            }
            GameObject gameObject = createAction();
            //放入可用池子
            unavailableGameObjects.Add(gameObject);
            //计数+1
            currentCount++;
            // 调用一下初始化方法
            getAction(gameObject);
            return Optional<GameObject>.Of(gameObject);
        }

        /**
         * <summary>销毁池对象</summary>
         */
        public void Destroy()
        {
            //销毁全部对象
            foreach (GameObject go in usableGameObjects)
            {
                destroyAction(go);
            }
            unavailableGameObjects.ForEach(destroyAction);
            unavailableGameObjects.Clear();
            usableGameObjects.Clear();
            currentCount = 0;
        }


    }

    /**
     * <summary>绑定对象池相关回调函数，主要是简化池子初始化</summary>
     */
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
    public class GOPoolActionAttribute : System.Attribute
    {
        /**
         * 事件名称
         */
        public readonly GOPoolActionType ActionType;

        public GOPoolActionAttribute(GOPoolActionType actionType)
        {
            this.ActionType = actionType;
        }
    }

    public enum GOPoolActionType
    {
        CreateAction,
        GetAction,
        ReleaseAction,
        DestroyAction,
        ValidateAction
    }
}