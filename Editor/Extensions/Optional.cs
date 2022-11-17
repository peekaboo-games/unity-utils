using System;
namespace MyUtils
{
    /**
     * <summary>空对象判断包装器类，用于空对象的判断和操作</summary>
     * <typeparam name="T">数据类型，必须是对象类型</typeparam>
     */
    public class Optional<T>
    {

        public readonly T data;


        private Optional(T data)
        {
            this.data = data;
        }

        private Optional()
        {
        }

        /**
         * <summary>基于特定值初始化</summary>
         * <param name="data">初始化的源数据</param>
         */
        public static Optional<T> Of(T data)
        {
            return new Optional<T>(data);
        }

        /**
         * <summary>初始化一个空对象</summary>
         * 
         */
        public static Optional<T> OfNullable()
        {
            return new Optional<T>();
        }

        /**
         * <summary>数据是否存在，是否为空</summary>
         * <returns>true 存在，false 不存在</returns>
         */
        public bool IsPresent()
        {
            return data != null;
        }

        /**
         * <summary>获取当前值，如果值为空</summary>
         * <returns>值</returns>
         * 
         */
        public T Get()
        {
            return OrElseThrow("Data is null.");
        }

        public T OrElse(T other)
        {
            return data == null ? other : data;
        }

        public T OrElseGet(Func<T> other)
        {
            return data == null ? other() : data;
        }

        public T OrElseThrow(string message)
        {
            if (data == null)
            {
                throw new Exception(message);
            }
            return data;
        }

        public T OrElseThrow(Exception exception)
        {
            if (data == null)
            {
                throw exception;
            }
            return data;
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
}

