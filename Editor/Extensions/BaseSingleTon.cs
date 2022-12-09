

using UnityEngine;

namespace MyUtils
{
    /// <summary>
    /// 单例对象基础类，使用此对象以后，可以通过 SingletonUtils.Get(T.name)来获取
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public abstract class BaseSingleton<T> : MonoBehaviour where T : Component
    {

        private static T instance;

        public static T Instance {
            // 获取
            get { 
                // 如果没有则需要初始化
                if (instance == null)
                {
                    // 从对象池中获取，如果不存在，则忽略
                    instance = FindObjectOfType<T>();
                    //如果实例中也获取不到，则开始初始化
                    if (instance == null) {
                        GameObject game = new(typeof(T).Name);
                        instance = game.AddComponent<T>();
                        SingletonUtils.Add(typeof(T).Name,  instance);
                    }
                }
                return instance;
           }
        
        }

        private string InfoGameObject()
        {
            return GetGameObjectInfo(gameObject);
        }

        private static string GetGameObjectInfo(GameObject obj)
        {
            return "(" + obj.GetInstanceID() + ":" + obj.name + ")";
        }

        public virtual void Awake()
        {
            // 如果不是自己，则销毁
            if (Instance != this)
            {
                Debug.LogWarning($"Singleton[{ typeof(T).Name }]: will destroy the extra gameObject {InfoGameObject()}");
                Destroy(gameObject);
                return;
            }
        }


        public virtual void OnDestroy()
        {
            SingletonUtils.Delete(typeof(T).Name);
        }
    }
 }
