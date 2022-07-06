using System;
using System.Collections.Generic;
using UnityEngine;

namespace ToronPuzzle
{
    public partial class DataEntity
    {
        public void ResetAdd() { _증가량 = 0; }
        public void Add증가량(float amount) { _증가량 += amount; }
        public void Add증가량배수(float amount) { _증가량배수 *= amount; }
        public void Add배수(float amount) { _배수 *= amount; }
        public void Add추가량(int amount) { _추가량 += amount; }


        private float _기본값 = 0;
        private float _증가량 = 0;
        private float _증가량배수 = 1f;  //증가량에만 곱한다. (공격력 계수 등에 사용)
        private float _배수 = 1f;        //기본값에 증가량이 더해진 값에 곱한다.
        private float _추가량 = 0;         //나머지 계산이 다 완료 된 후, 값을 추가한다.
        public float FinalValue { get { return (float)((_기본값 + _증가량 * _증가량배수) * _배수) + _추가량; } }


        public Property properties { get; private set; }

        public void AddProperty(Property property){ properties |= property;}

        public Type type { get; }

                     

        public enum Type
        {
            없음 = 0,

            피해량 = 1,
            회복량 = 2,
            방어도 = 3,
            생명력직접대입 = 4,   //피해나 회복이 아닌 생명력을 N으로 만듭니다 등의 효과.
                           //피해나 회복에따른 이벤트를 발생시키지 않는다.
            방어도직접대입 = 5,   //방어도 획득이나 소모가아님.
                           //대표적으로 턴 시작시 방어도 초기화될때 사용.

            시작쿨타임 =6,
            발언쿨타임 =7,

            경험치 = 10,

            효과부여 = 11,
            효과회수 = 12,
            효과제거 = 13,

            소모품획득 = 20,

            유물수치변동 = 100,
            금화수치변동 = 101,
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
