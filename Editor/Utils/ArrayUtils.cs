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

    }
}