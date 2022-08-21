using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;

namespace PG 
{
    public class ShowDebugtextScript : MonoBehaviour
    {
        // Start is called before the first frame update
        public static ShowDebugtextScript _instance;
        [SerializeField]
        Text _debugshower;
        [SerializeField]
        Text _debugshower2;

        void Start()
        {
            _instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Global_BattleEventSystem.CallNonTotalPause();
                Debug.Log("non total Paused button");
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                Global_BattleEventSystem.CallTotalPause();
                Debug.Log("Paused button");
            }
            if (Input.GetKeyDown(KeyCode.Y))
                Global_BattleEventSystem.CallOnGameOver();
        }

        public static void SetDebug(string _str)
        {
            _instance._debugshower.text = "_";
            _instance._debugshower.text = _str;
        }
        public static void SetDebug2(string _str)
        {
            _instance._debugshower2.text = "_";
            _instance._debugshower2.text = _str;
        }


    }

}

