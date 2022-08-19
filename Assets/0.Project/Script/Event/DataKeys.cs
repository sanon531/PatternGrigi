using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Data
{
    public enum CharacterID
    {
        player = 1,


    }
    public enum EffectID
    {
        공격력 = 0,
        방어력 = 1,



    }
    public enum ArtifactID
    {

        //기본 유물.
        FragileRush =10,
        //적도 - 공격력을 상승 시킴.
        Equatore =11,
        PoloNord = 12,
        StrangeTropics =13,
        BlackAndWhite = 14

    }


    public enum Rarity
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