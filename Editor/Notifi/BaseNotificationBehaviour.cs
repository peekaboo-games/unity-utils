using UnityEngine;
using System.Collections;
namespace MyUtils
{
    public abstract class BaseNotificationBehaviour : MonoBehaviour
    {


        public virtual void Awake()
        {
            Debug.Log($"{this.GetType().Name} Awake");
            PostNotification.Register(this);
        }

        public virtual void OnDestroy()
        {
            Debug.Log($"{this.GetType().Name} OnDestroy");
            PostNotification.UnRegister(this);
        }
    }

}