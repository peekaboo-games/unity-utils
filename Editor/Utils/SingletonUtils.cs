using System.Collections.Generic;

namespace MyUtils
{
    /**
     * <summary>单例工具类</summary>
     * <typeparam name="T">单例类型，需要实现 enum</typeparam>
     */
    public class SingletonUtils
    {

        private static Dictionary<string, object> __instances = new();


        /**
         * <summary>添加一个匿名对象</summary>
         * <param name="name">对象名称</param>
         */
        public static void Add(string name, object obj)
        {
            if (obj == null)
            {
                throw new System.Exception($"{name} value is null.");
            }
            __instances[name] = obj;
        }

        /**
         * <summary>获取一个单例对象，返回的是可选对象</summary>
         * <param name="name">单例对象名称</param>
         * <returns>可选对象</returns>
         * <typeparam name="K">对象类型</typeparam>
         */
        public static Optional<K> Get<K>(string name)
        {
            return Optional<K>.Of((K)__instances[name]);
        }

        /**
         * <summary>获取特定对象，如果不存在则抛出异常</summary>
         * <param name="name">名称</param>
         * <returns>值</returns>
         * <exception cref="System.Exception">如果单例对象不存在</exception>
         * <typeparam name="K">返回的对象类型</typeparam>
         */
        public static K RequiredGet<K>(string name)
        {
            Optional<K> optional = Get<K>(name);
            return optional.OrElseThrow(() => new System.Exception($"{name} value is null."));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <returns>删除结果</returns>
        public static bool Delete(string name) {
            return __instances.Remove(name);
        }
    }
}