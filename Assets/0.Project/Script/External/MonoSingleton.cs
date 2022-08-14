using UnityEngine;

namespace PG
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T _instance { get; private set; }

        private void Awake()
        {
            if (_instance != null)
                Debug.LogError("one more current Status");
            _instance = this as T;
            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }
    }
} 