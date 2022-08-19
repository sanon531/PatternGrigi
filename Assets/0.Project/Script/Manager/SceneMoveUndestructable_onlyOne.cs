using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG 
{
    public class SceneMoveUndestructable_onlyOne : MonoSingleton<SceneMoveUndestructable_onlyOne>
    {
        public bool _aaa = false;
        // Start is called before the first frame update
        protected override void OnAwake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
