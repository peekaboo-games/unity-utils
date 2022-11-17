using System;
using UnityEngine;
namespace MyUtils
{

    /**
     * <summary>Animator 扩展类，简化 animate set 和 get 操作</summary>
     * 
     */
    public class AnimSetter<T>
    {

        public readonly Animator animator;

        public readonly string paramName;


        public AnimSetter(Animator animator, string paramName)
        {
            this.animator = animator;
            this.paramName = paramName;
        }

        /**
         * <summary>设置值</summary>
         * <param name="value">要设置的值</param>
         */
        public AnimSetter<T> Set(T value)
        {
            Type dataType = typeof(T);

            if (dataType == typeof(bool))
            {
                animator.SetBool(paramName, Convert.ToBoolean(value));
            }
            else if (dataType == typeof(int))
            {
                animator.SetInteger(paramName, Convert.ToInt16(value));
            }
            else if (dataType == typeof(float))
            {
                animator.SetFloat(paramName, (float)Convert.ToDouble(value));
            }
            else
            {
                throw new Exception($"Set value {value} is error.");
            }
            return this;
        }

        /**
         * <summary>获取特定参数的值</summary>
         * <returns>数据值</returns>
         */
        public T Get()
        {
            Type dataType = typeof(T);
            object obj;

            if (dataType == typeof(bool))
            {
                obj = animator.GetBool(paramName);
            }
            else if (dataType == typeof(int))
            {
                obj = animator.GetInteger(paramName);
            }
            else if (dataType == typeof(float))
            {
                obj = animator.GetFloat(paramName);
            }
            else
            {
                throw new Exception($"Animator data type {dataType.Name} is not supported.");
            }

            return (T)obj;
        }
    }

}