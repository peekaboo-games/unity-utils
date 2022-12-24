using Codice.CM.Common;
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

        /// <summary>
        /// 如果存在则执行消费函数
        /// </summary>
        /// <param name="consumer">消费函数</param>
        public void IfPresent(Action<T> consumer) {
            if (IsPresent()) {
                consumer(data);
            }
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

        /// <summary>
        /// 如果值存在，并且这个值匹配给定的 predicate，返回一个Optional用以描述这个值，否则返回一个空的Optional。
        /// </summary>
        /// <param name="predicate">过滤函数</param>
        /// <returns>结果</returns>
        public Optional<T> Filter(Func<T, Optional<T>> predicate) {
            if (IsPresent()) {
                return predicate(data);
            }
            return this;
        }
        /// <summary>
        /// 如果值存在，返回基于Optional包含的映射方法的值，否则返回一个空的Optional
        /// </summary>
        /// <typeparam name="U">新的类型</typeparam>
        /// <param name="mapper">转换函数</param>
        /// <returns>结果</returns>
        public Optional<U> FlatMap<U>(Func<T, Optional<U>> mapper) {
            if (IsPresent())
            {
                return mapper(data);
            }
            return Optional<U>.OfNullable();
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

