using System;
using System.Collections.Generic;

public class SingleTonUtils
{

    private static Dictionary<SingletonType, object> __instances = new Dictionary<SingletonType, object>();


    public static void add(SingletonType name, object obj)
    {

        __instances[name] = obj;
    }


    public static T get<T>(SingletonType name)
    {
        return (T)__instances[name];
    }

}


public enum SingletonType
{
    AudioMananger,

    UserStorage

}