using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
        private List<GameObject> gameObjects;

        // 创建游戏对象的函数，如果游戏对象不够使用
        private Func<GameObject> createAction;

        // 后期销毁的函数
        private Action<GameObject> destroyAction;
        // 最大的对象个数
        private int maxCount;

        public GameObjectPools(Func<GameObject> createAction, Action<GameObject> destroyAction, int maxCount)
        {
            this.createAction = createAction;
            this.destroyAction = destroyAction;
            this.maxCount = maxCount;
            gameObjects = new List<GameObject>(maxCount);
        }



        /**
         * <summary>初始化一个对象池</summary>
         * <param name="createAction">每次创建对象的函数</param>
         * <param name="destroyAction">销毁对象</param>
         * <param name="maxCount">最大数量，如果达到最大数量以后，就不会创建新的</param>
         */
        public static GameObjectPools Of(Func<GameObject> createAction, Action<GameObject> destroyAction, int maxCount)
        {
            Assert.AreEqual(maxCount > 0, true, $"MaxCount[{maxCount}] must be ge zero.");
            Assert.IsNotNull(createAction, "CreateAction is not null.");
            Assert.IsNotNull(destroyAction, "DestroyAction is not null.");
            return new GameObjectPools(
                createAction,
                destroyAction,
                maxCount
                );
        }


        /**
        * <summary>初始化一个对象池</summary>
        * <param name="createAction">每次创建对象的函数</param>
        * <param name="destroyAction">销毁对象</param>
        * <param name="maxCount">最大数量，如果达到最大数量以后，就不会创建新的</param>
        */
        public static GameObjectPools Of(Func<GameObject> createAction, int maxCount)
        {
            Assert.AreEqual(maxCount > 0, true, $"MaxCount[{maxCount}] must be ge zero.");
            Assert.IsNotNull(createAction, "CreateAction is not null.");
            return new GameObjectPools(
                createAction,
                GameObject.Destroy,
                maxCount
                );
        }



        /**
         * <summary>获取一个池子对象</summary>
         * <returns>结果</returns>
         */
        public GameObject Get()
        {
            return Get(t => { });
        }

        /**
         * <summary>取一个元素，并初始化</summary>
         * <param name="doInit">初始化</param>
         * <returns>对象</returns>
         */
        public GameObject Get(Action<GameObject> doInit)
        {
            GameObject gameObject;
            // 如果池子存在，则取一个出来
            if (gameObjects.Count > 0)
            {
                gameObject = gameObjects[0];
                gameObjects.RemoveAt(0);
                doInit(gameObject);
                return gameObject;
            }
            gameObject = createAction();
            doInit(gameObject);
            return gameObject;
        }

        /**
         * <summary>释放资源</summary>
         * <param name="source">要移除的对象</param>
         * <returns>处理结果，true成功，false失败</returns>
         */
        public bool Released(GameObject source)
        {
            return Released(source, t => { });
        }

        /**
         * <summary>释放一个资源</summary>
         * <param name="source">释放的资源</param>
         * <param name="doRelease">释放的方式</param>
         */
        public bool Released(GameObject source, Action<GameObject> doRelease)
        {
            Assert.IsNotNull(source, "GameObjectPool remove element is not null.");
            Assert.IsNotNull(doRelease, "GameObjectPool doRelease is not null.");
            doRelease(source);
            if (gameObjects.Count >= maxCount)
            {
                destroyAction(source);
                return true;
            }
            else
            {
                gameObjects.Add(source);
                return false;
            }
        }

        /**
         * <summary>销毁池对象</summary>
         */
        public void Destroy()
        {
            Destroy(t => { });
        }

        /**
         * <summary>销毁对象池</summary>
         * <param name="preDestory">前置迭代函数</param>
         * 
         */
        public void Destroy(Action<GameObject> preDestory)
        {
            //销毁全部对象
            gameObjects.ForEach(t =>
            {
                preDestory(t);
                destroyAction(t);
            });
            gameObjects.Clear();
            destroyAction = null;
            createAction = null;
            maxCount = 0;
        }
    }
}