using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyUtils
{
    /**
     * <summary>随机工具类</summary>
     * 
     */
    public class RandomUtils
    {
        /**
         * <summary>是否在一定比率内，比率是0-100</summary>
         * <param name="precent">比率范围</param>
         */
        public static bool isWithinRatioOfPrecent(int precent)
        {
            if (precent >= 100)
            {
                return true;
            }
            return UnityEngine.Random.Range(0, 100) <= precent;
        }

        /**
         * <summary>从数组中随机获取一个对象</summary>
         * <param name="array">数组</param>
         * <returns>随机元素</returns>
         */
        public static Optional<T> GetOneUnityObject<T>(T[] array) where T : UnityEngine.Object
        {
            int index = GetIndex(array);
            return index == -1 ? Optional<T>.OfNullable() : Optional<T>.Of(array[index]);
        }

        /// <summary>
        /// 随机获取一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="array">数组</param>
        /// <returns>对象</returns>
        public static Optional<T> GetOne<T>(T[] array)
        {
            int index = GetIndex(array);
            return index == -1 ? Optional<T>.OfNullable() : Optional<T>.Of(array[index]);
        }

        /// <summary>
        /// 根据随机数和元素的随机概率获取一个元素，可能获取不到
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="precents">元素和概率，概率最大100，总和也不能大于100</param>
        /// <returns>元素</returns>
        public static Optional<T> GetOneOfPrecentList<T>(Dictionary<T, int> precents)
        {
            return GetOneOfPrecentList(precents, Random.Range(0, 100));
        }

        /// <summary>
        /// 根据随机数和元素的随机概率获取一个元素，可能获取不到
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="precents">元素和概率，概率最大100，总和也不能大于100</param>
        /// <param name="randomNum">随机数</param>
        /// <returns>元素</returns>
        public static Optional<T> GetOneOfPrecentList<T>(Dictionary<T, int> precents, int randomNum)
        {
            var list = new List<PrecentObject<T>>();
            int totalPrecent = 0;
            foreach ((T o, int precent) in precents)
            {
                if (precent > 0)
                {
                    list.Add(new PrecentObject<T>(o, totalPrecent));
                    totalPrecent += precent;
                }
            }
            if (totalPrecent > 100)
            {
                Debug.LogError($"{typeof(T).Name} total precent is {totalPrecent}.");
                return Optional<T>.OfNullable();
            }
            // 开始随机获取
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].Precent >= randomNum)
                {
                    return Optional<T>.Of(list[i].Target);
                }
            }
            return Optional<T>.OfNullable();
        }

        /**
         * <summary>获取一个随机的index -1 表示不存在</summary>
         * <param name="array">数组</param>
         * <returns>索引，-1 表示不存在</returns>
         */
        public static int GetIndex<T>(T[] array)
        {
            if (ArrayUtils.IsEmpty<T>(array))
            {
                return -1;
            }
            return UnityEngine.Random.Range(0, array.Length);
        }

        /// <summary>
        /// 随机从列表中取出特定数量的元素
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="size">数量</param>
        /// <returns>结果</returns>
        public static T[] GetList<T>(T[] array, int size)
        {
            // 如果数量不够，就不随机了
            if (array.Count() <= size)
            {
                return array;
            }
            if (size < 1)
            {
                return new T[0];
            }
            //随机起始位置
            int start = UnityEngine.Random.Range(0, array.Count() - size);
            T[] list = new T[size];
            int count = 0;
            for (var i = start; i < start + size; i++)
            {
                list[count++] = array[i];
            }
            return list;
        }
    }

}

public class PrecentObject<T>
{
    public PrecentObject(T target, int precent)
    {
        Target = target;
        Precent = precent;
    }

    /// <summary>
    /// 对象
    /// </summary>
    public T Target { get; private set; }

    //百分比
    public int Precent { get; private set; }
}