

using System.Collections.Generic;
/// <summary>
/// editor 工具类
/// </summary>
public class EditorUtils {

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
}
