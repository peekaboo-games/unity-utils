using System;
namespace MyUtils
{
    /**
     * <summary>空对象判断包装器类，用于空对象的判断和操作</summary>
     * <typeparam name="T">数据类型，必须是对象类型</typeparam>
     */
    public class Optional<T>
    {

        public T data { private set; get; }

        public T Nullable { private set; get; }

        private Optional(T data)
        {
            this.data = data;
        }

        private Optional()
        {
        }

        /// <summary>
        /// 是否为空的判断
        /// </summary>
        /// <returns>结果 true 空，false 非空</returns>
        private bool IsNullable() {
            if (typeof(T).IsValueType && !typeof(T).IsEnum)
            {
                return data.Equals(Nullable);
            }
            return data == null;
        }

        /**
         * <summary>基于特定值初始化</summary>
         * <param name="data">初始化的源数据</param>
         */
        public static Optional<T> Of(T data)
        {
            return new Optional<T>(data);
        }

        /// <summary>
        /// 对于struct/int等非空类型的初始化操作
        /// </summary>
        /// <param name="data">当前值</param>
        /// <param name="nullable">默认值/null值</param>
        /// <returns>新的对象</returns>
        public static Optional<T> Of(T data, T nullable)
        {
            var opt = new Optional<T>(data);
            opt.Nullable = nullable;
            return opt;
        }

        /**
         * <summary>初始化一个空对象</summary>
         * 
         */
        public static Optional<T> OfNullable()
        {
            if (typeof(T).IsValueType && !typeof(T).IsEnum) {
                throw new Exception($"Optional T type is '{typeof(T)}' and it is unsupport 'OfNullable' method.");
            }
            return new Optional<T>();
        }

        /// <summary>
        /// 处理不能为空的数据类型，空状态
        /// </summary>
        /// <param name="nullable">该类型的空对象</param>
        /// <returns>对象</returns>
        public static Optional<T> OfNullable(T nullable)
        {
            var opt = new Optional<T>();
            opt.Nullable = nullable;
            return opt;
        }

        /**
         * <summary>数据是否存在，是否为空</summary>
         * <returns>true 存在，false 不存在</returns>
         */
        public bool IsPresent()
        {
            return !IsNullable();
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

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="dd"></param>
        /// <returns></returns>
        public Optional<T> Set(T dd) {
            this.data = dd;
            return this;
        }

        /**
         * <summary>获取当前值，如果值为空</summary>
         * <returns>值</returns>
         */
        public T Get()
        {
            return OrElseThrow("Data is null.");
        }

        public T OrElse(T other)
        {
            return IsNullable() ? other : data;
        }

        public T OrElseGet(Func<T> other)
        {
            return IsNullable() ? other() : data;
        }

        public T OrElseThrow(string message)
        {
            if (IsNullable())
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
        public Optional<U> FlatMap<U>(Func<T, Optional<U>> mapper){
            if (IsPresent())
            {
                return mapper(data);
            }
            return Optional<U>.OfNullable();
        }

        public T OrElseThrow(Exception exception)
        {
            if (IsNullable())
            {
                throw exception;
            }
            return data;
        }

        public T OrElseThrow(Func<Exception> action)
        {
            if (IsNullable())
            {
                throw action();
            }
            return data;
        }
    }
}

