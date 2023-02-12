using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;

namespace PG.Event
{
    public delegate void OnEvent();

    public delegate void OnCalcDataEntity(DataEntity entity);
    public delegate void OnCalcData_Form1(Data_Character target, DataEntity dataEntity);
    public delegate void OnCalcData_Form_2(Data_Character dataStarter, Data_Character dataTarget, DataEntity dataEntity);


    public delegate void OnBattleEvent();

    public delegate void OnEventWithbool(bool val);
    public delegate void OnEventWithInt(int val);
    public delegate void OnEventWithFloat(float val);
    public delegate void OnEventWithFloatArray(float[] val);
    public delegate void OnEventWithString(string val);
    public delegate void OnEventWithIntWithTarget(Data_Character 대상자, int val);
    public delegate void OnEventWithFloatWithTarget(Data_Character 대상자, float val);
    public delegate void OnEventWithPattern(DrawPatternPresetID val);


    public static class Global_BattleEventSystem
    {
        //맵
        public static event OnEvent _onEnterStage;
        public static void CallOnEnterStage() { _onEnterStage?.Invoke(); }


        //특정 노드를 다가갈 때 
        public static event OnEventWithInt _onNodeReached;
        public static void CallOnNodeReached(int id) { _onNodeReached?.Invoke(id); }

        public static event OnEventWithFloatArray _onNodeSetWeight;
        public static void CallOnNodeSetWeight(float[] 수치값) { _onNodeSetWeight?.Invoke(수치값); }


        public static event OnEvent _onNodeSetFar;
        public static void CallOnNodeSetFar() { _onNodeSetFar?.Invoke(); }
        public static event OnEvent _onNodeSetClose;
        public static void CallOnNodeSetClose() { _onNodeSetClose?.Invoke(); }



        //배틀 중 이벤트 
        public static event OnEvent _onTouchMain;
        public static void CallOnTouchMain() { _onTouchMain?.Invoke(); }

        // 진짜 시작 되었을때 발동 waitforStart 때
        public static event OnEvent _onBattleBegin;
        public static void CallOnBattleBegin() { _onBattleBegin?.Invoke(); }

        public static OnCalcDataEntity _onAddCharge;
        public static void CallAddCharge(DataEntity entity) { _onAddCharge?.Invoke(entity); }


        public static event OnEventWithFloat _onAddAttackMag;
        public static void CallOnAddAttackMag(float _배수) { _onAddAttackMag?.Invoke(_배수); }


        //레벨업 할 경우 콜함 
        static bool _callLevelUP = false;
        public static event OnEvent _onLevelUpShow;
        public static event OnEvent _onLevelUpHide;
        public static void CallOnLevelUp()
        {
            _callLevelUP = true;
            CallTotalPauseNoMatterWhat();
            _onLevelUpShow?.Invoke();
        }
        public static void CallOffLevelUp()
        {
            _callLevelUP = false;
            CallOffTotalPauseNoMatterWhat();
            _onLevelUpHide?.Invoke();
        }

        public static event OnEventWithInt _onWaveChange;
        public static void CallOnWaveChange(int nowWaveOrder) { _onWaveChange?.Invoke(nowWaveOrder); }
        
        
        #region//Combat
        public static event OnEvent _onPlayerSizeChanged;
        public static void CallOnSizeChanged() 
        {
            _onPlayerSizeChanged?.Invoke();
        }

        #endregion



        #region//일시정지

        public static bool _isCutScenePaused = false;
        public static void CallCutScenePause()
        {
            if (_isCutScenePaused)
            {
                _isCutScenePaused = false;
                CallOffCutScenePause();
            }
            else
            {
                _isCutScenePaused = true;
                CallOnCutScenePause();
            }


        }
        public static event OnEvent _onCutScenePause;
        private static void CallOnCutScenePause() { _onCutScenePause?.Invoke(); }
        public static event OnEvent _offCutScenePause;
        private static void CallOffCutScenePause() { _offCutScenePause?.Invoke(); }
        public static bool _isTotalPaused = false;
        public static void CallTotalPauseSwitch()
        {
            //레벨업,아이템 선택 중에는 작동 안하도록 만들기. 임시로 해둠
            if (_callLevelUP)
                return;

            if (_isTotalPaused)
            {
                _isTotalPaused = false;
                CallOffTotalPause();
            }
            else
            {
                _isTotalPaused = true;
                CallOnTotalPause();
            }
        }
        public static void CallTotalPauseNoMatterWhat() 
        {
            _isTotalPaused = true;
            CallOnTotalPause();
        }
        public static void CallOffTotalPauseNoMatterWhat()
        {
            _isTotalPaused = false;
            CallOffTotalPause();
        }


        public static event OnEvent _onTotalPause;
        private static void CallOnTotalPause() { _onTotalPause?.Invoke(); }
        public static event OnEvent _offTotalPause;
        private static void CallOffTotalPause() { _offTotalPause?.Invoke(); }
        #endregion

        #region//차지 및 패턴 관련
        

        public static event OnEvent _onChargeStart;
        public static void CallOnChargeStart() { _onChargeStart?.Invoke(); }

        public static event OnEvent _onChargeEnd;
        public static void CallOnChargeEnd() { _onChargeEnd?.Invoke(); }

        //패턴 성공시
        public static event OnEventWithPattern _onPatternSuccessed;
        public static void CallOnPatternSuccessed(DrawPatternPresetID patternPreset) { _onPatternSuccessed?.Invoke(patternPreset); }


        #endregion

        public static event OnEventWithFloat _onGainEXP;
        public static void CallOnGainEXP(float exp) { _onGainEXP?.Invoke(exp); }


        public static event OnEvent _onGameOver;
        public static void CallOnGameOver() { _onGameOver?.Invoke(); }




        //애니메이션 관련
        #region
        #endregion

        //데미지 계산

        public static event OnEventWithFloat _onCalcPlayerAttack;
        public static void CallOnCalcPlayerAttack(float val)
        { _onCalcPlayerAttack?.Invoke(val); }

        public static event OnCalcDataEntity _onCalcDamageByEntity;
        



     




    }


}
