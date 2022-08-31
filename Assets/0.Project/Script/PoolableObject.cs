using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle 
{
    public class PoolableObject : MonoBehaviour
    {
        [SerializeField]
        protected bool _isPlaced = false;

        protected virtual void OnObjectEnabled()
        {
            _isPlaced = true;
        }


        protected virtual void OnObjectDisabled() 
        {
            _isPlaced = false;
        }

    }

}
