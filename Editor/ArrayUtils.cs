using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class ArrayUtils
{

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
    public static void iterator<T, K>(Dictionary<T, List<K>> dic, T key, Action<K> action)
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
    public static bool isEmpty<T>(T[] list)
    {
        return list == null || list.Count() == 0;
    }

}
