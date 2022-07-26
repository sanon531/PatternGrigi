using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;

namespace PG.Battle 
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField]
        Player_Script _ingamePlayer;
        [SerializeField]
        Enemy_Script _ingameEnemy;
        [SerializeField]
        float _delayedTime =2.5f;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(DelayedStart(_delayedTime));

            Global_BattleEventSystem._on�����Ͻ����� += TotalPause;
            Global_BattleEventSystem._on�����Ͻ��������� += TotalUnpause;
        }

        IEnumerator DelayedStart(float delayedTime) 
        {
            yield return new WaitForSeconds(delayedTime);
            Global_BattleEventSystem.CallOn��Ʋ����();
        }
        bool _isPaused = false;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Global_BattleEventSystem.Call�������Ͻ�����();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                Global_BattleEventSystem.Call�����Ͻ�����();
            }

        }

        void TotalPause() 
        {
            Time.timeScale = 0;
        }
        void TotalUnpause()
        {
            Time.timeScale = 1;
        }


    }

}
