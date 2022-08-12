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
            if (GlobalUIEventSystem._isTotalFade) 
                GlobalUIEventSystem.CallTotalFade();

            Global_BattleEventSystem._onTotalPause += TotalPause;
            Global_BattleEventSystem._offTotalPause += TotalUnpause;
            StartCoroutine(DelayedStart(_delayedTime));
        }
        IEnumerator DelayedStart(float delayedTime) 
        {
            yield return new WaitForSeconds(delayedTime);
            Global_BattleEventSystem.CallOnBattleBegin();
        }
        bool _isPaused = false;

        // Update is called once per frame
        void Update()
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

        void TotalPause() 
        {
            Time.timeScale = 0;
        }
        void TotalUnpause()
        {
            Time.timeScale = 1;
        }

        private void OnDestroy()
        {
            Global_BattleEventSystem._onTotalPause -= TotalPause;
            Global_BattleEventSystem._offTotalPause -= TotalUnpause;

        }
    }

}
