using System;

namespace MyUtils
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class SubscribeAttribute : System.Attribute
    {
        /**
         * 事件名称
         */
        public readonly string EventName;

        public SubscribeAttribute(string eventName)
        {
            this.EventName = eventName;
        }
    }
}