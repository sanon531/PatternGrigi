using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;

namespace PG.Event
{
    public delegate void On이벤트();

    public delegate void OnCalc데이터_0형식(DataEntity 정보체);
    public delegate void OnCalc데이터_1형식(Data_Character 대상, DataEntity 정보체);
    public delegate void OnCalc데이터_2형식(Data_Character 정보계산주체, Data_Character 부체, DataEntity 정보체);
    public delegate void On배틀이벤트();

    public delegate void On휴식회복(DataEntity 정보체);
    public delegate void On이벤트Withbool수치값(bool 수치값);
    public delegate void On이벤트With정수수치값(int 수치값);
    public delegate void On이벤트WithFloat수치값(float 수치값);
    public delegate void On이벤트WithString수치값(string 수치값);
    public delegate void On이벤트With대상수치값(Data_Character 대상자, int 수치값);
    public delegate void On이벤트With대상Float수치값(Data_Character 대상자, float 수치값);


    public static class Global_BattleEventSystem 
    {
        //맵
        public static event On이벤트 _on맵입장;
        public static void CallOn맵입장() { _on맵입장?.Invoke(); }
        public static event On이벤트 _on노드선택;
        public static void CallOn노드선택() { _on노드선택?.Invoke(); }
        public static event On이벤트 _on노드로드완료;
        public static void CallOn노드로드완료() { _on노드로드완료?.Invoke(); }


        //배틀 중 이벤트 
        public static event On이벤트 _onTouchMain;
        public static void CallOnTouchMain() { _onTouchMain?.Invoke(); }

        // 진짜 시작 되었을때 발동 waitforStart 때
        public static event On이벤트 _on배틀시작;
        public static void CallOn배틀시작() { _on배틀시작?.Invoke(); }


        public static event On이벤트WithFloat수치값 _on공격배수추가;
        public static void CallOn공격배수추가(float _배수) { _on공격배수추가?.Invoke(_배수);  }
        public static event On이벤트WithFloat수치값 _on방어배수추가;
        public static void CallOn방어배수추가(float _배수) { _on방어배수추가?.Invoke(_배수); }


        public static event On이벤트Withbool수치값 _on모듈항상들기;
        public static void CallOn모듈항상들기(bool 수치값)
        { _on모듈항상들기?.Invoke(수치값); }



        public static event On이벤트 _on발언비활성화;
        public static void CallOn발언비활성화() { _on발언비활성화?.Invoke(); }
        public static event On이벤트 _on발언활성화;
        public static void CallOn발언활성화() { _on발언활성화?.Invoke(); }




        public static bool _isLevelupPaused = false;
        public static void Call레벨업일시정지() 
        {
            if (_isLevelupPaused) 
            {
                _isLevelupPaused = false;
                CallOn레벨업일시정지해제();
            }
            else
            {
                _isLevelupPaused = true;
                CallOn레벨업일시정지();
            }


        }
        public static event On이벤트 _on레벨업일시정지;
        private static void CallOn레벨업일시정지() { _on레벨업일시정지?.Invoke(); }
        public static event On이벤트 _on레벨업일시정지해제;
        private static void CallOn레벨업일시정지해제() { _on레벨업일시정지해제?.Invoke(); }

        public static event On이벤트WithFloat수치값 _on경험치획득;
        private static void CallOn경험치획득(float exp) { _on경험치획득?.Invoke(exp); }


        private static bool _isTotalPaused = false;
        public static void Call완전일시정지()
        {
            if (_isTotalPaused)
            {
                _isTotalPaused = false;
                CallOn완전일시정지해제();
            }
            else
            {
                _isTotalPaused = true;
                CallOn완전일시정지();
            }
        }
        public static event On이벤트 _on완전일시정지;
        private static void CallOn완전일시정지() { _on완전일시정지?.Invoke(); }
        public static event On이벤트 _on완전일시정지해제;
        private static void CallOn완전일시정지해제() { _on완전일시정지해제?.Invoke(); }


        //애니메이션 관련
        #region
        #endregion


        //블록 생성및 배치 관련
        public static event On이벤트WithFloat수치값 _on계산쿨타임변동;
        public static void CallOn계산쿨타임변동(float _changeVal) { _on계산쿨타임변동?.Invoke(_changeVal); }


        //데미지 계산
        public static event OnCalc데이터_1형식 _onCalc데미지;
        public static void CallOnCalc데미지(Data_Character 피해대상, DataEntity 계산정보체)
        { _onCalc데미지?.Invoke(피해대상, 계산정보체); }
        public static event OnCalc데이터_1형식 _onCalc방어도;
        public static void CallOnCalc방어도(Data_Character 피해대상, DataEntity 계산정보체)
        { _onCalc방어도?.Invoke(피해대상, 계산정보체); }



        //생명력 계산
        public static event OnCalc데이터_1형식 _onCalc최대생명력;
        public static void CallOnCalc최대생명력(Data_Character 대상, DataEntity 최대생명력계산정보체)
        { _onCalc최대생명력?.Invoke(대상, 최대생명력계산정보체); }


        //계산하는데 걸리는 쿨타임과 관련됨
        public static event OnCalc데이터_0형식 _onCalc토론시작쿨타임;
        public static void CallOnCalc토론시작쿨타임(DataEntity 계산정보체)
        { _onCalc토론시작쿨타임?.Invoke(계산정보체); }



        public static event OnCalc데이터_0형식 _onCalc쿨타임;
        public static void CallOnCalc쿨타임(DataEntity 계산정보체)
        { _onCalc쿨타임?.Invoke(계산정보체); }


        //계산(발언)을 할때 활용 할것. 계산 직전은 계산 하기 전에 발동
        public static event On이벤트 _on판계산선언;
        public static void CallOn판계산선언() { _on판계산선언?.Invoke(); }

        public static event OnCalc데이터_1형식 _on판계산;
        public static void CallOn판계산(Data_Character 정보계산타겟, DataEntity 정보체)
        { _on판계산?.Invoke(정보계산타겟, 정보체); }




        public static event On이벤트 _on적턴시작;
        public static void CallOn적턴시작() { _on적턴시작?.Invoke(); }

        public static event On이벤트 _on시퀀스넘기기;
        public static void CallOn시퀀스넘기기() { _on시퀀스넘기기?.Invoke(); }

        public static event On이벤트 _on게임종료;
        public static void CallOn게임종료() { _on게임종료?.Invoke(); }


    }


}
