

using System.Collections.Generic;
using UnityEngine;

namespace MyUtils
{
    /// <summary>
    /// 将物体转换为跨场景长存物体，相当于单例物体
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {

        /// <summary>
        /// 游戏物体的唯一名字
        /// </summary>
        public string GameObjectName;

        // 存储不销毁的游戏对象
        private static Dictionary<string, GameObject> GameObjects = new Dictionary<string, GameObject>();


        /// <summary>
        /// 获取不销毁的对象
        /// </summary>
        /// <param name="_objectName">对象名称</param>
        /// <returns>对象</returns>
        public static Optional<GameObject> Get(string _objectName) {
            return Optional<GameObject>.Of(GameObjects[_objectName]);
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
