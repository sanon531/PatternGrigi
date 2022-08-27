using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Data
{
    public enum ArtifacProperty
    {
        None = 0,
        CountValue = 1,
    }

    public enum ArtifactFlag
    {
        Active = 0,
        Inactive = 1,
    }

    public enum CharacterID
    {
        Player = 1,
        Enemy_Fireboy = 2,
        Enemy_WindShooter = 3,

    }
    public enum EffectID
    {
        공격력 = 0,
        방어력 = 1,
    }
    public enum ArtifactID
    {

        //유리대포처럼 적 공격 업 + 내 공격 업.
        FragileRush =10,

        //적도 - 약간공격력을 상승 시킴.
        Equatore =11,

        //장거리세트
        BubbleGun = 20,// 길어질때의 배율 추가.

        //단거리세트
        QuickSlice = 42,

        //사이즈 관련
        PadThai = 51,
        SesameOil = 52


    }
    public enum ProjectileID 
    {
        NormalBullet = 0,
    
    }
    public enum NodePlaceType
    {
        Random = 0,
        Close = 1,
        Far= 2,
    }

    public enum ArtifactRarity
    {
        None = -1,

        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Unique = 3,
    }
    public enum SceneType
    {
        Entry,
        Title,
        WorldMap,
        Battle
    }

    //charactor kinds 로 바꾸기
    public enum StageKind
    {
        Fire_TInyMage,
        Earth_Fighter,
        Dark_Artsian,
        Fire_Mage,
    }

    //이곳에서
    public enum DrawPatternPreset
    {
        Default_Thunder = 0,
        LoveAndPeace = 1,
        Sandglass = 2
    }

}