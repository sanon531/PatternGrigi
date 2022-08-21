using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;

namespace PG.Battle 
{

    public class PauseUIButton : MonoBehaviour
    {
        [SerializeField]
        GameObject _pausePannel;

        bool _isPannelshow = false;
        public void CallPausePanel()
        {
            if (Global_BattleEventSystem._isTotalPaused != _isPannelshow) 
            {
                Debug.LogError("UnmatchedCall");
                return;
            }
            Global_BattleEventSystem.CallTotalPause();
            if (_isPannelshow)
            {
                _pausePannel.transform.position = new Vector3(10000,0,0);
                _isPannelshow = false;
            }
            else 
            {
                int i_width = Screen.width;
                int i_height = Screen.height;
                _pausePannel.transform.position = new Vector3(i_width/2, i_height/2, 0);
                _isPannelshow = true;
            }

        }



    }

}