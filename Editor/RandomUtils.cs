using System;
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
    public static T GetOneUnityObject<T>(T[] array) where T : UnityEngine.Object
    {
        int index = GetIndex<T>(array);
        return index == -1 ? null : array[index];
    }


    /**
     * <summary>获取一个随机的index -1 表示不存在</summary>
     * <param name="array">数组</param>
     * <returns>索引，-1 表示不存在</returns>
     */
    public static int GetIndex<T>(T[] array)
    {
        if (ArrayUtils.isEmpty<T>(array))
        {
            return -1;
        }
        return UnityEngine.Random.Range(0, array.Length);
    }
}

