using UnityEngine;

namespace PG
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T _instance { get; private set; }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("one more current Status");
                Destroy(this.gameObject);
                return;
            }
            else if (_instance == null)
            {
                //Debug.Log("Set this");
                _instance = this as T;
                CallOnAwake();
            }
            else 
            {
                //Debug.Log("ok" + _instance);
            }

        }

        protected virtual void CallOnAwake()
        {
        }

        private void OnDestroy() 
        {
            if(_instance ==this )
                _instance = null;
            CallOnDestroy();
        }

        protected virtual void CallOnDestroy() 
        {
        
        
        }


    }
} 