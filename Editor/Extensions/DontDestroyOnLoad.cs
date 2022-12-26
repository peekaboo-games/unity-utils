

using System.Collections.Generic;
using UnityEngine;

namespace MyUtils
{
    /// <summary>
    /// 将物体转换为跨场景长存物体，相当于单例物体
    /// </summary>
    public class DontDestroyOnLoad<T> : MonoBehaviour
    {

        /// <summary>
        /// 游戏物体的唯一名字
        /// </summary>
        public string GameObjectName = typeof(T).Name;

        // 存储不销毁的游戏对象
        private static readonly Dictionary<string, GameObject> GameObjects = new();


        /// <summary>
        /// 获取不销毁的对象
        /// </summary>
        /// <param name="_objectName">对象名称</param>
        /// <returns>对象</returns>
        public static Optional<GameObject> Get(string _objectName) {
            return Optional<GameObject>.Of(GameObjects[_objectName]);
        }

        /// <summary>
        /// 根据默认
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        public static Optional<GameObject> Get<K>()
        {
            return Optional<GameObject>.Of(GameObjects[typeof(K).Name]);
        }

        /// <summary>
        /// 强制获取一个 GameObject， 如果不存在则抛出异常
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static GameObject RequiredGet<K>() {
            Optional<GameObject> optional = Get<K>();
            if (!optional.IsPresent())
            {
                throw new System.Exception($"[{typeof(K).Name}]DontDestroyOnLoad is not found.");
            }
            return optional.Get();
        }


        /// <summary>
        /// 获取当前的实例
        /// </summary>
        /// <typeparam name="K">实例类型</typeparam>
        /// <param name="_objectName">对象名称</param>
        /// <returns></returns>
        public static Optional<K> GetInstance<K>(string _objectName) {
            Optional<GameObject> optional = Get(_objectName);
            if (optional.IsPresent()) {
                return Optional<K>.Of(optional.Get().GetComponent<K>());
            }
            return Optional<K>.OfNullable();
        }

        /// <summary>
        /// 获取实例，实例名称是默认名称
        /// </summary>
        /// <typeparam name="K">类型</typeparam>
        /// <returns>实例</returns>
        public static Optional<K> GetInstance<K>() {
            Optional<GameObject> optional = Get<K>();
            if (optional.IsPresent())
            {
                return Optional<K>.Of(optional.Get().GetComponent<K>());
            }
            return Optional<K>.OfNullable();
        }

        /// <summary>
        /// 强制获取实例
        /// </summary>
        /// <typeparam name="K">实例类型</typeparam>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static K RequiredGetInstance<K>() {
            Optional<K> optional = GetInstance<K>();
            if (!optional.IsPresent()) {
                throw new System.Exception($"[{typeof(K).Name}]DontDestroyOnLoad is not found.");
            }
            return optional.Get();
        }

        /// <summary>
        /// 强制销毁一个跨场景物体
        /// </summary>
        /// <param name="_objectName">物体名称</param>
        /// <returns>是否销毁成功</returns>
        public static bool ForcedDestroy(string _objectName) {

            Optional<GameObject> gameObject = Get(_objectName);

            if (!gameObject.IsPresent()) {
                return false;
            }
            // 销毁对象
            GameObject.Destroy(gameObject.Get());
            //从字典中移除
           return GameObjects.Remove(_objectName);
        }

        private void Awake()
        {
            // 先查询是否存在，如果存在则直接删除
            if (GameObject.Find(GameObjectName) != null) {
                Destroy(gameObject);
                return;
            }
            // 做成永久存在
            DontDestroyOnLoad(gameObject);
            GameObjects.Add(GameObjectName, gameObject);
        }


    }
}
