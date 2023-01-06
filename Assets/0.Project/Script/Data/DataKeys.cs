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



        Slime = 21,
        Tempt_Mob = 22


    }
    public enum EffectID
    {
        공격력 = 0,
        방어력 = 1,
    }
    public enum ArtifactID
    {
        //최초의 기본 유물 동남아 설화인 마하니파타 자카타를 모티브로 하였으며
        //번개 공격을기본적으로 주게 됨.
        Thunder_Manimekhala = 0,
        LoveAndPeace = 1,


        //유리대포처럼 적 공격 업 + 내 공격 업.
        FragileRush = 10,

        //적도 - 약간공격력을 상승 시킴.
        Equatore =11,

        //장거리세트
        BubbleGun = 20,// 길어질때의 배율 추가.
        BulletTeleportShooter = 21,

        //단거리세트
        QuickSlice = 42,

        //사이즈 관련
        PadThai = 51,
        SesameOil = 52,
        
        //장거리 노드 세팅
        Pinocchio = 60 ,
        AtomSetting = 61,


        Upgrade_AimShot = 100,
        Upgrade_StraightShot = 101,
        Upgrade_LightningShot = 102,
        Upgrade_TowerShot = 103,
        Upgrade_Knife = 104,



    }
    public enum ProjectileID 
    {
        NormalBullet = 0,
        LightningShot = 1,
        StraightShot = 2,
        TowerBullet = 3,
        CuttingKnife = 4

    }
    public enum NodePlaceType
    {
        Random = 0,
        Close = 1,
        Far= 2,
    }

    public enum ArtifactJsonData 
    {
        ArtifactName,
        ArtifactEffect,
        DevComment
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
    public enum DrawPatternPresetID
    {
        Thunder_Manimekhala = 0,
        LoveAndPeace = 1,
        Sandglass = 2,




        Empty_Breath= 999 // 최초의 시작에 호출되는 코드
    }


    public enum UITextID 
    {
    
        Main_GameTitle = 0,
        Main_GameStart = 1,



        Battle_PatternDelay = 21,

    }

}