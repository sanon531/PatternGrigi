using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;

namespace PG.Event
{
    public delegate void OnEvent();

    public delegate void OnCalcDataEntity(DataEntity 정보체);
    public delegate void OnCalcData_Form1(Data_Character 대상, DataEntity 정보체);
    public delegate void OnCalcData_Form_2(Data_Character 정보계산주체, Data_Character 부체, DataEntity 정보체);
    public delegate void On배틀이벤트();

    public delegate void OnEventWithbool(bool 수치값);
    public delegate void OnEventWithInt(int 수치값);
    public delegate void OnEventWithFloat(float 수치값);
    public delegate void OnEventWithString(string 수치값);
    public delegate void OnEventWithIntWithTarget(Data_Character 대상자, int 수치값);
    public delegate void OnEventWithFloatWithTarget(Data_Character 대상자, float 수치값);


    public static class Global_BattleEventSystem 
    {
        //맵
        public static event OnEvent _onEnterStage;
        public static void CallOnEnterStage() { _onEnterStage?.Invoke(); }


        public static event OnEventWithInt _onNodeReached;
        public static void CallOnNodeReached(int id) { _onNodeReached?.Invoke(id); }

        //배틀 중 이벤트 
        public static event OnEvent _onTouchMain;
        public static void CallOnTouchMain() { _onTouchMain?.Invoke(); }

        // 진짜 시작 되었을때 발동 waitforStart 때
        public static event OnEvent _onBattleBegin;
        public static void CallOnBattleBegih() { _onBattleBegin?.Invoke(); }


        public static event OnEventWithFloat _onAddAttackMag;
        public static void CallOnAddAttackMag(float _배수) { _onAddAttackMag?.Invoke(_배수);  }



        #region//일시정지
        public static bool _isLevelupPaused = false;
        public static void CallLevelUpPause() 
        {
            if (_isLevelupPaused) 
            {
                _isLevelupPaused = false;
                CallOffLevelUpPause();
            }
            else
            {
                _isLevelupPaused = true;
                CallOnLevelUpPause();
            }


        }
        public static event OnEvent _onLevelUpPause;
        private static void CallOnLevelUpPause() { _onLevelUpPause?.Invoke(); }
        public static event OnEvent _offLevelUpPause;
        private static void CallOffLevelUpPause() { _offLevelUpPause?.Invoke(); }
        
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
                _isLevelupPaused = true;
                CallOnCutScenePause();
            }


        }
        public static event OnEvent _onCutScenePause;
        private static void CallOnCutScenePause() { _onCutScenePause?.Invoke(); }
        public static event OnEvent _offCutScenePause;
        private static void CallOffCutScenePause() { _offCutScenePause?.Invoke(); }
        private static bool _isTotalPaused = false;
        public static void CallTotalPause()
        {
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
        public static event OnEvent _onTotalPause;
        private static void CallOnTotalPause() { _onTotalPause?.Invoke(); }
        public static event OnEvent _offTotalPause;
        private static void CallOffTotalPause() { _offTotalPause?.Invoke(); }
        #endregion

        #region//차지 관련
        public static event OnEvent _on차지시작;
        public static void CallOn차지시작() { _on차지시작?.Invoke(); }
        public static event OnEvent _on차지종료;
        public static void CallOn차지종료() { _on차지종료?.Invoke(); }

        #endregion




        public static event OnEventWithFloat _onGainEXP;
        public static void CallOnGainEXP(float exp) { _onGainEXP?.Invoke(exp); }


      
        

        public static event OnEvent _onGameOver;
        public static void CallOnGameOver() { _onGameOver?.Invoke(); }



        //애니메이션 관련
        #region
        #endregion

        //데미지 계산
        public static event OnCalcData_Form1 _onCalc데미지;
        public static void CallOnCalc데미지(Data_Character 피해대상, DataEntity 계산정보체)
        { _onCalc데미지?.Invoke(피해대상, 계산정보체); }
        public static event OnCalcData_Form1 _onCalc방어도;
        public static void CallOnCalc방어도(Data_Character 피해대상, DataEntity 계산정보체)
        { _onCalc방어도?.Invoke(피해대상, 계산정보체); }



        //생명력 계산
        public static event OnCalcData_Form1 _onCalc최대생명력;
        public static void CallOnCalc최대생명력(Data_Character 대상, DataEntity 최대생명력계산정보체)
        { _onCalc최대생명력?.Invoke(대상, 최대생명력계산정보체); }






    }


}
