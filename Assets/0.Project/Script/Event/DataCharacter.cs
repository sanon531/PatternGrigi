using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToronPuzzle.Data;
using ToronPuzzle.Event;

namespace ToronPuzzle
{
    public enum CharacterSide
    {
        Ally = 0,       // 아군 (전투 시, 왼쪽에 서있음)
        Enemy = 1,      // 적군 (전투 시, 오른쪽에 서있음)
        Nuetral = 2,    // 중립 (전투 시, 전장 바깥에 서있음)
    }

    [System.Serializable]
    public partial class Data_Character
    {
        protected struct Data_CombatProgress
        {


        }


        public bool IsDead { get; private set; }
        public void SetDead() { IsDead = true; }
        public void SetAlive() { IsDead = false; }

        public CharacterID characterID;
        public CharacterSide 소속진영 ;
        public CharacterSide 상대진영
        {
            get
            {
                if (소속진영 == CharacterSide.Ally) { return CharacterSide.Enemy; }
                if (소속진영 == CharacterSide.Enemy) { return CharacterSide.Ally; }
                return CharacterSide.Nuetral;
            }
        }

        public int 최대생명력_Default = 100;
        public int 최대생명력
        {
            get
            {
                DataEntity 최대생명력data = DataEntity.고유데이터(최대생명력_Default);
                Global_InWorldEventSystem.CallOnCalc최대생명력(this, 최대생명력data);
                return (int)최대생명력data.FinalValue;
            }
        }
        public int 현재생명력;
        public int 현재방어도;


    }
}

namespace ToronPuzzle.Data
{
    public enum CharSide
    {
        Ally,
        Enemy
    }
    // 턴수가 지남에따라 감소하는 상태이상과 아닌 상태이상으로 나뉨 
    // 해당하는 상태에 따라 이펙트가 달라짐 
    public enum CharBuff
    {
        //합리 포인트 절반일때
        Tired = 0,
        //방어력감소
        Confused = 1,
        //공격력 감소
        Depressed = 2,
        //방어력 증가 감소랑 중첩 불가
        Concentrate = 3,
        //공격력 증가 감소랑 중첩 불가
        Boasted = 4,
        //일정시간 블록 배치 불가.
        Surprised = 5,
        //발언 쿨타임이 돌면 바로 발언, 발언 쿨타임 절반 감소
        Rage,
        //보너스 블록 능력치 2배
        Compassion,
        Horror,
        Brave,
        //독뎀
        Painful,
        //회복
        Relaxed,

        //여기서 부터는 블록과 행동에 의한 것
        //공격시 반사가 들어옴
        Spiked,

        //1회 패배 직전의 상황에서 해당 공격 무시함.
        Famed,


    }
    [System.Serializable]
    public class CharBuffData
    {

        public CharBuff _effect;
        public int _amount;

        public CharBuffData(CharBuff effect, int amount)
        {
            _effect = effect;
            _amount = amount;
        }
     
    }


    //해당 행동이 생각인지, 행동인지 나눔
    public enum CharAnimType
    {
        Think, 
        Speech,
        Extra,

    }




    public static class CharacterLibrary
    {

        public static Dictionary<CharBuff, string> BuffNameDic =
            new Dictionary<CharBuff, string>()
            {
                { CharBuff.Tired , "지침."},
                { CharBuff.Confused , "혼란."},
                { CharBuff.Depressed , "우울."},
                { CharBuff.Concentrate  , "집중. "},
                { CharBuff.Boasted , "자신감."},
                { CharBuff.Surprised , "놀람."},
                { CharBuff.Rage , "분노"},
                { CharBuff.Horror , "무서움"},
                { CharBuff.Painful , "고통스러움"},
                { CharBuff.Relaxed , "편안함."},
                { CharBuff.Spiked , "가시돋힘"},
                { CharBuff.Brave , "위풍당당"},
                { CharBuff.Compassion , "이해심"},
                { CharBuff.Famed , "명망있는"},

            };

        public static Dictionary<CharBuff, string> BuffExplainDic =
            new Dictionary<CharBuff, string>()
            {
                { CharBuff.Tired , "공격력과 방어력이 10% 감소합니다."},
                { CharBuff.Confused , "방어력이 25% 감소합니다. "},
                { CharBuff.Depressed , "공격력이 25% 감소합니다. "},
                { CharBuff.Concentrate  , "방어력이 25% 상승합니다. "},
                { CharBuff.Boasted , "공격력이 25% 상승합니다."},
                { CharBuff.Surprised , "제한 시간 동안 배치가 불가능합니다."},
                { CharBuff.Rage , "발언 쿨타임이 돌면 바로 발언, 발언 쿨타임 절반 감소"},
                { CharBuff.Horror , "무서움? 몰?루"},
                { CharBuff.Painful , "발언 시간 동안 지속적으로 데미지를 받습니다."},
                { CharBuff.Relaxed , "발언 시간 동안 지속적으로 회복됩니다."},
                { CharBuff.Spiked , "받는 공격이 반사가 됩니다. ."},
                { CharBuff.Brave , "아직 안정함. 머쓱 타드"},
                { CharBuff.Compassion , "보너스 블록 2배 효과"},
                { CharBuff.Famed , "1회 즉사 데미지를 막아줍니다."},

            };
    }

    public struct CharactorActionInfo
    {
        string _name;
        float _actionTime;
        string _animationName;

        public CharactorActionInfo(string name, float actionTime, string animationName)
        {
            _name = name;
            _actionTime = actionTime;
            _animationName = animationName;
        }
    }

}
