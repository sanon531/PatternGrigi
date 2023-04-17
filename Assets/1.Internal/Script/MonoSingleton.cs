using UnityEngine;

namespace PG
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool _isPlacedOnce = false;
        public static T _instance { get; private set; }

        private void Awake()
        {
            if (_isPlacedOnce && _instance!=null && _instance != this)
            {
                //Debug.Log("one more current Status");
                Destroy(this.gameObject);
                return;
            }
            else if (!_isPlacedOnce || _instance == null)
            {
                //Debug.Log("Set this");
                _instance = this as T;
                _isPlacedOnce = true;
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
            if (_instance == this)
            {
                _instance = null;
                _isPlacedOnce = false;
            }

            //print("Boom" + name);
            CallOnDestroy();
        }

        protected virtual void CallOnDestroy() 
        {
        
        
        }


    }
} 