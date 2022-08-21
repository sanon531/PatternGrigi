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
                _isPannelshow = false;
            }
            else 
            {
                _isPannelshow = true;
            }
            _pausePannel.SetActive(_isPannelshow);

        }

        //���� �������� ����� Ȯ���ϴ� �ڵ�
        void ShowCurrentArtifactImage() 
        {
        
        
        }


    }

}