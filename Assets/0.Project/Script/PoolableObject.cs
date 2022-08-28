using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle 
{
    public class PoolableObject : MonoBehaviour
    {
        protected bool _isActive = false;

        protected virtual void OnObjectEnabled()
        {
            _isActive = true;
        }


        protected virtual void OnObjectDisabled() 
        {
            _isActive = false;
        }

    }

}
