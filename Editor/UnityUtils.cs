using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityUtils
{

    /**
     * <summary>递归获取特定名称的子物体，递归获取，不包含当前物体</summary>
     * <param name="parent">查询的父物体</param>
     * <param name="name">物体名称</param>
     * <returns>结果, null 不存在</returns>
     */
    public static GameObject FindByName(Transform parent, string name)
    {
        if (parent == null)
        {
            return null;
        }
        // 获取子物体
        foreach (Transform tf in parent)
        {
            // 查到第一个为止
            if (tf.name == name)
            {
                return tf.gameObject;
            }
            else
            {
                GameObject go = FindByName(tf, name);
                if (go != null)
                {
                    return go;
                }
            }
        }
        return null;
    }

    /**
     * <summary>查询特定标签的子物体，递归获取，不包含当前物体，使用此方法需要注意内存问题，子物体不要包含过多</summary>
     * <param name="parent">父物体</param>
     * <param name="tagName">标签名称</param>
     * <returns>符合条件的物体列表</returns>
     */
    public static List<GameObject> FindByTagName(Transform parent, string tagName)
    {
        List<GameObject> list = new List<GameObject>();
        if (parent == null || tagName == null || tagName.Length == 0)
        {
            return list;
        }

        // 获取子物体
        foreach (Transform tf in parent)
        {
            // 检查当前物体
            if (tf.tag == tagName)
            {
                list.Add(tf.gameObject);
            }
            List<GameObject> gos = FindByTagName(tf, tagName);
            list.AddRange(gos);
        }
        return list;
    }


    /**
     * <summary>获取组件</summary>
     * <param name="name">子物体名称</param>
     * <param name="parent">查询的物体</param>
     * <returns>结果，如果未查到的</returns>
     */
    public static Optional<T> GetComponent<T>(Transform parent, string name) where T : Component
    {
        GameObject g = FindByName(parent, name);
        if (g == null)
        {
            return Optional<T>.OfNullable(null);
        }
        return Optional<T>.Of(g.GetComponent<T>());
    }

    /**
     * <summary>获取组件，如果不存在，则抛出异常</summary>
     * <param name="parent">父元素</param>
     * <param name="name">组件名称</param>
     * <returns>组件</returns>
     */
    public static T RequiredGetComponent<T>(Transform parent, string name) where T : Component
    {
        return GetComponent<T>(parent, name).OrElseThrow(() => new Exception($"{name} not found."));
    }
}

public class Optional<T>
{

    public readonly T data;


    private Optional(T data)
    {
        this.data = data;
    }

    public static Optional<T> Of(T data)
    {
        return new Optional<T>(data);
    }

    public static Optional<T> OfNullable(T value)
    {
        return new Optional<T>(value);
    }

    public bool IsPresent()
    {
        return data != null;
    }

    public T Get()
    {
        if (data == null)
        {
            throw new Exception("Data is null.");
        }
        return data;
    }

    public T OrElse(T other)
    {
        return data == null ? other : data;
    }

    public T OrElseGet(Func<T> other)
    {
        return data == null ? other() : data;
    }

    public T OrElseThrow(Func<Exception> action)
    {
        if (data == null)
        {
            throw action();
        }
        return data;
    }
}