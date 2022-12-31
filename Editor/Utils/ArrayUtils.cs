using System;
using System.Collections.Generic;
using System.Linq;
namespace MyUtils
{

    /**
     * <summary>数组工具类</summary>
     * 
     */
    public class ArrayUtils
    {

        /// <summary>
        /// 合并数组
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="originArray">原始数组</param>
        /// <param name="newLength">新长度</param>
        /// <param name="def">默认值</param>
        /// <returns>新的数组</returns>
        public static T[] MergeArray<T>(T[] originArray, int newLength, T def)
        {
            T[] newArray = new T[newLength];
            // 设置原来的
            for (var i = 0; i < originArray.Length && i < newLength; i++)
            {
                newArray[i] = originArray[i];
            }
            // 填充默认值
            for (var i = originArray.Length; i < newLength; i++)
            {
                newArray[i] = def;
            }
            return newArray;
        }

        /**
         * <summary>获取集合数据，如果集合为空，则初始化</summary>
         * 
         * <param name="dic">原始字段</param>
         * <param name="key">字段key</param>
         * <returns>新的列表</returns>
         */
        public static List<K> GetAndInit<T, K>(Dictionary<T, List<K>> dic, T key)
        {
            return dic.ContainsKey(key) ? dic[key] : new List<K>();
        }


        /**
         * <summary>添加一个元素，并获取最终的列表</summary>
         * <param name="action">初始化元素</param>
         * <param name="key">key</param>
         * <param name="list">原石列表</param>
         * <returns>新的列表</returns>
         */
        public static List<K> AddAndGet<T, K>(Dictionary<T, List<K>> dic, T key, Func<K> action)
        {
            List<K> list = GetAndInit<T, K>(dic, key);
            list.Add(action());
            dic[key] = list;
            return list;
        }

        /**
         * <summary>删除一个元素并</summary>
         * <param name="dic">原始列表</param>
         * <param name="key">字段key</param>
         * <param name="match">需要删除的元素</param>
         */
        public static List<K> RemoveAndSet<T, K>(Dictionary<T, List<K>> dic, T key, Predicate<K> match)
        {
            if (!dic.ContainsKey(key))
            {
                return null;
            }
            List<K> list = GetAndInit<T, K>(dic, key);
            list.RemoveAll(match);
            if (list == null || list.Count == 0)
            {

                dic.Remove(key);
                return null;
            }
            else
            {
                dic[key] = list;
                return list;
            }
        }

        /// <summary>
        /// 检查数量是否满足要求，如果不满足则通过def生成
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="sources">原始元素</param>
        /// <param name="needSize">需要的数量</param>
        /// <param name="def">生成函数</param>
        /// <returns>处理后的列表</returns>
        public static T[] CheckIfNotEnough<T>(T[] sources, int needSize, Func<int, T> def)
        {
            return CheckIfNotEnough(sources == null ? null : sources.ToList(), needSize, def).ToArray();
        }

        /// <summary>
        /// 检查数量是否满足要求，如果不满足则通过def生成
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="sources">原始元素</param>
        /// <param name="needSize">需要的数量</param>
        /// <param name="def">生成函数</param>
        /// <returns>处理后的列表</returns>
        public static List<T> CheckIfNotEnough<T>(List<T> sources, int needSize, Func<int, T> def)
        {
            // 检查是否存在
            sources = sources == null ? new List<T>() : sources;
            // 数量如果已经满足，则直接返回
            if (sources.Count >= needSize)
            {
                return sources;
            }
            for (var i = 0; i < needSize; i++)
            {
                if (sources[i] == null)
                {
                    sources[i] = def(i);
                }
            }
            return sources;
        }
        /**
         * <summary>移除或设置字段值，如果字段的list为空，则移除，否则设置</summary>
         * 
         * <param name="key">字典key</param>
         * <param name="dic">字段</param>
         * <param name="setIt">需要设置的列表</param>
         */
        public static List<K> RemoveOrSet<T, K>(Dictionary<T, List<K>> dic, T key, List<K> setIt)
        {
            if (setIt == null || setIt.Count == 0)
            {
                dic.Remove(key);
                return null;
            }
            else
            {
                dic[key] = setIt;
                return setIt;
            }
        }

        /**
         * 
         * <summary>便利</summary>
         * <param name="dic">字典</param>
         * <param name="key">便利的key</param>
         * <param name="action">比例执行</param>
         * 
         */
        public static void Iterator<T, K>(Dictionary<T, List<K>> dic, T key, Action<K> action)
        {
            if (!dic.ContainsKey(key))
            {
                return;
            }

            List<K> list = dic[key];

            foreach (K o in list)
            {
                action(o);
            }
        }

        /**
         * <summary>列表是否为空</summary>
         * <param name="list">检查的列表</param>
         * <returns>结果,true 为空，false 不为空</returns>
         */
        public static bool IsEmpty<T>(T[] list)
        {
            return list == null || list.Count() == 0;
        }


        /**
         * <summary>初始化一个空数组</summary>
         * <returns>空数组</returns>
         */
        public static T[] NewNoop<T>()
        {
            return new T[] { };
        }



        /**
         * <summary>将元素转换成数组</summary>
         * <param name="arg1">参数1</param>
         * <param name="args">可变参数</param>
         */
        public static T[] As<T>(T arg1, params T[] args)
        {
            //如果后面的参数不存在，则直接返回
            if (args == null || args.Length == 0)
            {
                return new T[] { arg1 };
            }
            // 链接其他参数
            T[] arrays = new T[args.Length + 1];
            arrays[0] = arg1;
            for (var i = 0; i < args.Length; i++)
            {
                arrays[i + 1] = args[i];
            }
            return arrays;
        }

        /// <summary>
        /// 集合转换成数组
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="collection">集合元数据</param>
        /// <returns>数组</returns>
        public static T[] CollectionToArray<T>(List<T> collection)
        {
            if (collection == null)
            {
                return null;
            }

            T[] array = new T[collection.Count];
            for (var i = 0; i < collection.Count; i++)
            {
                array[i] = collection[i];
            }
            return array;
        }

        /// <summary>
        /// 数组转换成集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="array">数组</param>
        /// <returns>结合</returns>
        public static List<T> ArrayToCollection<T>(T[] array)
        {
            if (array == null)
            {
                return null;
            }
            List<T> collection = new List<T>();
            foreach (var o in array)
            {
                collection.Add(o);
            }
            return collection;
        }

        public static object[] AsObject(object arg1, params object[] args)
        {
            //如果后面的参数不存在，则直接返回
            if (args == null || args.Length == 0)
            {
                return new object[] { arg1 };
            }
            // 链接其他参数
            object[] arrays = new object[args.Length + 1];
            arrays[0] = arg1;
            for (var i = 0; i < args.Length; i++)
            {
                arrays[i + 1] = args[i];
            }
            return arrays;
        }

        /// <summary>
        /// 计算数组中
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="search">查找的对象</param>
        /// <returns>出现的次数</returns>
        public static int Count<T>(T[] array, T search)
        {
            if (array == null || search == null)
            {
                return 0;
            }
            int count = 0;
            foreach (T o in array)
            {
                if (o.Equals(search))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 计算数组中
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="array">集合</param>
        /// <param name="search">查找的对象</param>
        /// <returns>出现的次数</returns>
        public static int Count<T>(List<T> array, T search)
        {
            if (array == null || search == null)
            {
                return 0;
            }
            int count = 0;
            foreach (T o in array)
            {
                if (o.Equals(search))
                {
                    count++;
                }
            }
            return count;
        }
    }
}