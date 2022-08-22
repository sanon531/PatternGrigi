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
            CallOnAwake();
        }

        protected virtual void CallOnAwake()
        {
        }

        private void OnDestroy() 
        {
            _instance = null;
            CallOnDestroy();
        }

        protected virtual void CallOnDestroy() 
        {
        
        
        }


    }
} 