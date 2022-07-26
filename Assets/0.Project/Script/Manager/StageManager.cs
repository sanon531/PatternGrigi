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

            Global_BattleEventSystem._on완전일시정지 += TotalPause;
            Global_BattleEventSystem._on완전일시정지해제 += TotalUnpause;
        }

        IEnumerator DelayedStart(float delayedTime) 
        {
            yield return new WaitForSeconds(delayedTime);
            Global_BattleEventSystem.CallOn배틀시작();
        }
        bool _isPaused = false;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Global_BattleEventSystem.Call레벨업일시정지();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                Global_BattleEventSystem.Call완전일시정지();
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
