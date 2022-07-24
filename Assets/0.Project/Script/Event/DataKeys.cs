using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Data
{
    public enum CharacterID
    {
        //플레이어는 1 적은 다양한 바리에이션이 있다.
        플레이어_정세진 = 1,


        //잡졸
        멸고단원 = 2,
        멸고단_허깨비,
        반멸고단원,
        반멸고단_허깨비,
        자생_허깨비,
        사기꾼,

        //보스
        파괴_중간보스_멸망희망자,
        유지_중간보스_허깨비,
        창조_중간보스_광대도공,
        유지_보스_어제,
        파괴_보스_부마,
        공허_보스_체념


    }
    public enum ModuleID
    {
        //디폴트 값 
        없음=0,
        불길한_공허 = 0,

        //일반 모듈
        카리스마Lv1 = 1,
        분석력Lv1 = 2,
        따뜻함Lv1 = 3,
        정의감Lv1 = 4,//흡혈
        경멸감Lv1 = 5,//방어 공격
        책임감Lv1 = 6, //회복
        천방지축 = 7,
        쇄빙=8,
        비단결_선율 = 9,
        무자비 = 10,

        //희귀 유물
        카리스마Lv2 = 21,
        분석력Lv2 = 22,
        따뜻함Lv2 = 23,
        정의감Lv2 = 24,
        경멸감Lv2 = 25,
        책임감Lv2 = 26,
        기선제압 = 28,
        궤변_파쇄기 = 29,
        진실_천명=30,
        현란한_임기응변=31,
        승화=32,
        충동 = 33,
        착취 = 34,


        //유니크
        빙점,
        경칩,
        개미지옥,


        //부정행위 - 패널티도 있지만 나름 쓸모있도록 할것.
        
        양자역학적_결론,
        과도한_확대해석,
        부조리한_연막,
        억지강요,
        함정투성이전제,
        흑백논리,
        꼬리무는_순환논증,
        성급한_일반화,
        추잡한_인신공격,
        공허한_지껄임,
        주제돌리기,
        반이성적_권위



    }
    public enum EffectID
    {
        공격력 = 0,
        방어력 = 1,



    }
    public enum SceneType
    {
        Entry,
        Title,
        WorldMap,
        Battle
    }

}