using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PG.Data;
using PG.Event;
using UnityEngine;
using UnityEngine.UI;

namespace PG.Battle
{
    public class GameClearManager : MonoSingleton<GameClearManager>
    {

        private Image thisImage;

        protected override void CallOnDestroy()
        {
            base.CallOnDestroy();
            Global_BattleEventSystem._onWaveChange -= WaveLastCheck; 
        }

        private int _lastWave = 0;
        protected override void CallOnAwake()
        {
            base.CallOnAwake();
            thisImage = GetComponent<Image>();
            thisImage.enabled = false;
            thisImage.color = Color.clear;
            _lastWave = Global_CampaignData._waveTimeList.Count -1;
            Global_BattleEventSystem._onWaveChange += WaveLastCheck; 
        }

        void WaveLastCheck(int i)
        {
            print(_lastWave + "+" + i);
            if (_lastWave <= i)
            {
                CallGameCleared();
            }
        }

        void CallGameCleared()
        {
            thisImage.enabled = true;
            thisImage.DOColor(Color.white, 0.5f);
            Global_CampaignData._gameCleared = true;
            Global_BattleEventSystem.CallOnGameClear();
        }



    }
}