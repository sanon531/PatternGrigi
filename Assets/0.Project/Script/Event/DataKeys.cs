using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Data
{
    public enum ECharacterID
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
    public enum EArtifactID
    {


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