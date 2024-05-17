using UnityEngine;
using UnityEngine.Events;

namespace Plataformer
{
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] EventChannel<T> eventcChannel;

        [SerializeField] UnityEvent<T> unityEvent;

        protected void Awake()
        {
            eventcChannel.Register(this);
        }


        protected void OnDestroy() 
        {
            eventcChannel.Deregister(this);
        }

        public void Raise(T value)
        {
            unityEvent?.Invoke(value);
        }

        
    }

    public class EventListener : EventListener<Empty> { }
    }
