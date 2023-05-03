using System;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    [System.Serializable]
    public partial class DataEntity
    {
        public void ResetAdd() { _증가량 = 0; }
        public void Add증가량(float amount) { _증가량 += amount; }
        public void Add증가량배수(float amount) { _증가량배수 *= amount; }
        public void Set증가량(float amount) { _증가량 = amount; }
        public void Mult배수(float amount) { _배수 *= amount; }
        public void Add배수(float amount) { _배수 += amount; }
        public void Set배수(float amount) { _배수 = amount; }

        public void Add추가량(int amount) { _추가량 += amount; }

        [SerializeField]
        private float _기본값 = 0;
        [SerializeField]
        private float _증가량 = 0;
        [SerializeField]
        private float _증가량배수 = 1f;  //증가량에만 곱한다. (공격력 계수 등에 사용)
        [SerializeField]
        public float _배수 = 1f;        //기본값에 증가량이 더해진 값에 곱한다.
        [SerializeField]
        private float _추가량 = 0;         //나머지 계산이 다 완료 된 후, 값을 추가한다.
        public float FinalValue { get {
                //Debug.Log("기본값: " + _기본값 + ", 증가량" + _증가량 +", 증가량 배수"+ _증가량배수 + ", 추가량 "+ _추가량+" , 배수" +_배수);
                //Debug.Log(" a"+(float)((_기본값 + _증가량 * _증가량배수) * _배수));
                return (float)((_기본값 + _증가량 * _증가량배수) * _배수 + _추가량); 
            
            } }
        public int BaseValue {
            get { return (int)_기본값; }
            set { _기본값 = value; }
        }
        public void PrintCurrent() { Debug.Log("기본값: " + _기본값 + ", 증가량" + _증가량 + ", 증가량 배수" + _증가량배수 + ", 추가량 " + _추가량 + " , 배수" + _배수); }
        public Property properties { get; private set; }

        public void AddProperty(Property property){ properties |= property;}

        [SerializeField]
        public Type type;

                     

        public enum Type
        {
            None = 0,

            Damage = 1,
            ChargeGauge = 2,
            ChargeEXP = 3,
            생명력직접대입 = 4,   //피해나 회복이 아닌 생명력을 N으로 만듭니다 등의 효과.
                           //피해나 회복에따른 이벤트를 발생시키지 않는다.
            방어도직접대입 = 5,   //방어도 획득이나 소모가아님.
                           //대표적으로 턴 시작시 방어도 초기화될때 사용.
            MaxCooltimeToken=6,
            RandomPatternCount= 7,

            PlayerSpeed = 10,

            효과부여 = 11,
            효과회수 = 12,
            효과제거 = 13,


            유물수치변동 = 100,
            ProjectilePierce = 62,
            ProjectileCount = 63,
            ProjectileSpeed = 64,
            LengthMag = 65, // 이동한 거리에따른 배율
            PlayerSize = 66, // 플레이어 사이즈 
        }
        public enum Property
        {
            None = 0,
            방어도무시 = 1, //피해량일때만 사용
            고정수치 = 2,   //다른효과로부터 증감되는 영향을 받지않음
                        //엔터티 생성시와, 실제적용의 수치가 항상 동일 (무적등의 예외상황엔 데미지가 0으로바뀜)
            크리티컬 = 4,
            반격무시 = 8,

            //->고유플래그
            낙인데미지 = 16,


            효과면역됨 = 32, //면역 텍스트 띄우기 위함

            충격파주체 = 64, //충격파 효과 띄우기 위함

            연참고정감소 = 128, //아크로배틱류에 영향 받지 않는 고정 연참 감소의 경우 사용
        }

    

    }
}
