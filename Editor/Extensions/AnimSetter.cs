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
        public virtual AnimSetter<T> Set(T value)
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
        public virtual T Get()
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

        /**
         * <summary>初始化一个trigger animator setter</summary>
         */
        public class Trigger : AnimSetter<bool>
        {
            public Trigger(Animator animator, string paramName) : base(animator, paramName)
            { }

            public override AnimSetter<bool> Set(bool value)
            {
                throw new Exception("Animator trigger type not supported");
            }

            public override bool Get()
            {
                throw new Exception("Animator trigger type not supported");
            }

            /**
             * 触发一下事件
             */
            public void DoTrigger()
            {
                this.animator.SetTrigger(paramName);
            }
        }
    }

}