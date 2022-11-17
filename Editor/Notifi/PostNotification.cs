using System.Collections.Generic;
using System;
using UnityEngine;
using System.Reflection;
namespace MyUtils
{
    // 消息走起来
    public class PostNotification
    {

        // 事件名称和时间回调
        private static readonly Dictionary<string, List<MethodObject>> _EventNames = new Dictionary<string, List<MethodObject>>();

        // 目标和时间回调
        private static readonly Dictionary<object, List<string>> _TargetEventNames = new Dictionary<object, List<string>>();


        /**
         * 
         * <summary>注册事件</summary>
         * 
         * <param name="target">事件对象</param>
         * 
         */
        public static void Register(object target)
        {

            MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            // 检查是否有 attribute 注解
            foreach (MethodInfo info in methods)
            {
                IEnumerable<System.Attribute> attrs = info.GetCustomAttributes(typeof(SubscribeAttribute));
                foreach (System.Attribute attr in attrs)
                {
                    // 注册一个事件
                    _Register(((SubscribeAttribute)attr).EventName, target, info);
                }
            }
        }

        /**
         * 
         * 注册
         * 
         */
        private static void _Register(string name, object target, MethodInfo method)
        {
            ArrayUtils.AddAndGet<string, MethodObject>(_EventNames, name, () => new MethodObject(target, method));
            ArrayUtils.AddAndGet<object, string>(_TargetEventNames, target, () => name);
        }


        /**
         * 
         * 取消注册
         * 
         */
        private static void _UnRegister(string name, object target, MethodInfo method)
        {
            ArrayUtils.RemoveAndSet<string, MethodObject>(_EventNames, name, o => o.Method == method && o.Target == target);
            ArrayUtils.RemoveAndSet<object, string>(_TargetEventNames, target, o => o == name);
        }

        /**
         * <summary>注册一个事件</summary>
         * <param name="name">事件名称</param>
         * <param name="action">注册的事件函数</param>
         */
        public static void Register<T>(string name, Action<MessagePayload<T>> action)
        {
            if (!_EventNames.ContainsKey(name))
            {
                return;
            }
            _Register(name, action.Target, action.Method);
        }


        /**
        * <summary>移除一个事件</summary>
        * <param name="name">事件名称</param>
        * <param name="action">注册的事件函数</param>
        */
        public static void UnRegister<T>(string name, Action<MessagePayload<T>> action)
        {
            if (!_EventNames.ContainsKey(name))
            {
                return;
            }
            _UnRegister(name, action.Target, action.Method);
        }

        /**
         * <summary>取消注册特定对象的所有数据</summary>
         * <param name="target">取消的对象</param>
         */
        public static void UnRegister(object target)
        {
            if (!_TargetEventNames.ContainsKey(target))
            {
                return;
            }
            List<string> names = _TargetEventNames[target];
            _TargetEventNames.Remove(target);
            //设置
            foreach (string name in names)
            {
                ArrayUtils.RemoveAndSet<string, MethodObject>(_EventNames, name, o => o.Target == target);
            }

        }

        /**
        * <summary>触发一个事件</summary>
        * <param name="name"> 事件名称 </param>
        * <param name="sender">触发者</param>
        * <param name="data">传递的数据</param>
        */
        public static void Post<T>(string name, object sender, T data)
        {
            if (!_EventNames.ContainsKey(name))
            {
                return;
            }
            ArrayUtils.Iterator<string, MethodObject>(_EventNames, name, mo =>
            {
                if (mo.Required)
                {
                    mo.Method.Invoke(mo.Target, new object[] { MessagePayload<T>.ValueOf(name, sender, data) });
                }
                else
                {
                    mo.Method.Invoke(mo.Target, new object[] { });
                }

            });
        }

        /**
        * <summary>触发一个事件</summary>
        * <param name="name"> 事件名称 </param>
        * <param name="sender">触发者</param>
        */
        public static void Post(string name, object sender)
        {
            if (!_EventNames.ContainsKey(name))
            {
                return;
            }
            ArrayUtils.Iterator<string, MethodObject>(_EventNames, name, mo =>
            {

                if (mo.Required)
                {

                    mo.Method.Invoke(mo.Target, new object[] { MessagePayload<object>.ValueOf(name, sender) });
                }
                else
                {
                    mo.Method.Invoke(mo.Target, new object[] { });
                }
            });
        }

    }


    public class MethodObject
    {

        public readonly object Target;

        public readonly MethodInfo Method;

        public readonly bool Required;

        public MethodObject(object target, MethodInfo method)
        {
            this.Target = target;
            this.Method = method;
            this.Required = !ArrayUtils.IsEmpty(method.GetParameters());
        }
    }
}