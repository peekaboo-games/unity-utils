
namespace MyUtils
{
    public class MessagePayload<T>
    {

        // 事件名称
        public readonly string name;
        // 事件发送者
        public readonly object sender;
        // 数据
        public readonly T data;

        public MessagePayload(string name, object sender, T data)
        {
            this.name = name;
            this.sender = sender;
            this.data = data;
        }


        public MessagePayload(string name, object sender)
        {
            this.name = name;
            this.sender = sender;
        }


        public static MessagePayload<T> ValueOf(string name, object sender, T data)
        {
            return new MessagePayload<T>(name, sender, data);
        }


        public static MessagePayload<object> ValueOf(string name, object sender)
        {
            return new MessagePayload<object>(name, sender);
        }
    }

}
