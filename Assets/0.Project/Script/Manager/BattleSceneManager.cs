using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using PG.Event;
using PG.Data;

namespace PG.Battle 
{
    //게임 시작, 적에 의한 이벤트 세팅
    public class BattleSceneManager : MonoSingleton<BattleSceneManager>
    {


        bool _isgameStarted = false;
        [SerializeField]
        float _playTime = 0f;
        [SerializeField]
        TextMeshProUGUI _timeShower;
        [SerializeField]
        float _delayedTime =2.5f;
        [SerializeField]
        private CampaignData _currentCampaignData;

        [SerializeField] private TextMeshProUGUI _DebugCurrentText;

        private int _playEndTime = 0;

        // Start is called before the first frame update
        protected override void CallOnAwake()
        {
            MultiSceneUIScript.PublicFadeOut();
            //Debug.Log("Call Awake");
            _isgameStarted = false;
            _playTime = 0f;
            StartCoroutine(DelayedStart(_delayedTime));
            SwitchEventPause();
            SwitchEventCombat();
            Global_CampaignData.SetCampaginInitialize(_currentCampaignData);
            //Debug.Log(Global_CampaignData._charactorAttackDic[CharacterID.Player].FinalValue);
            _DebugCurrentText.text = _currentCampaignData.name;
            Global_BattleEventSystem._onGameOver += TrySetBestTime;
            Global_BattleEventSystem._onGameClear += TrySetBestTime;
        }

        protected override void CallOnDestroy()
        {
            //print("ssdd");
            SwitchEventPause();
            SwitchEventCombat();
            Global_BattleEventSystem._onGameOver -= TrySetBestTime;
            Global_BattleEventSystem._onGameClear -= TrySetBestTime;
        }


        IEnumerator DelayedStart(float delayedTime) 
        {
            yield return new WaitForSeconds(delayedTime);
            Global_BattleEventSystem.CallOnBattleBegin();
            AudioManager.ChangeBackgroundMusicOnSceneChange(1);
            _playEndTime = Mathf.RoundToInt(Global_CampaignData._waveTimeList.Max());

            _isgameStarted = true;
        }

        // 적의 스폰과 보스 스폰 은 그냥. 현재의 형태로 만들자.
        void Update()
        {
            if (_isgameStarted ) 
            {
                _playTime += Time.deltaTime;
                _timeShower.SetText(Mathf.Round(_playTime).ToString()+ "/"+_playEndTime.ToString());
            }

        }

        public float GetPlayTime()
        {
            return _playTime;
        }

        private void TrySetBestTime()
        {
            SaveDataManager._instance.saveData.BestTime = _playTime;
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
