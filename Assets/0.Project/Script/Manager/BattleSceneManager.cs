using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;

namespace PG.Battle 
{
    //게임 시작, 적에 의한 이벤트 세팅
    public class BattleSceneManager : MonoSingleton<BattleSceneManager>
    {
        [SerializeField]
        Player_Script _ingamePlayer;
        [SerializeField]
        Enemy_Script _ingameEnemy;
        [SerializeField]
        float _delayedTime =2.5f;
        // Start is called before the first frame update
        protected override void CallOnAwake()
        {
            if (GlobalUIEventSystem._isTotalFade) 
                GlobalUIEventSystem.CallTotalFade();

            StartCoroutine(DelayedStart(_delayedTime));
            SwitchEventPause();
            SwitchEventCombat();
        }

        protected override void CallOnDestroy()
        {
            SwitchEventPause();
            SwitchEventCombat();
        }


        IEnumerator DelayedStart(float delayedTime) 
        {
            yield return new WaitForSeconds(delayedTime);
            Global_BattleEventSystem.CallOnBattleBegin();
        }

        // Update is called once per frame
        void Update()
        {
    



        }

        #region//Combat EventSetter

        bool _isCombatSetted = false;
        void SwitchEventCombat() 
        {
            if (!_isCombatSetted)
            {

                _isCombatSetted = true;
            }
            else 
            {

                _isCombatSetted = false;
            }


        }


        #endregion



        #region//pauseset
        bool _isPauseSet = false;

        void SwitchEventPause() 
        {
            if (!_isPauseSet)
            {
                Global_BattleEventSystem._onTotalPause += TotalPause;
                Global_BattleEventSystem._offTotalPause += TotalUnpause;
                _isPauseSet = true;
            }
            else 
            {
                Global_BattleEventSystem._onTotalPause -= TotalPause;
                Global_BattleEventSystem._offTotalPause -= TotalUnpause;
                _isPauseSet = false;
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
        #endregion
    }

}
