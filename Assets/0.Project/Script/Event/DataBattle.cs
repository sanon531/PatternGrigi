using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace PG.Event 
{
    public interface ISetNontotalPause
    {
        void SetNonTotalPauseOn();
        void SetNonTotalPauseOff();
    }

}

namespace PG.Data
{
    /// <summary>
    /// 배경 화면의 이미지에대한 설정
    /// </summary>

    public enum EStageKind
    {
        Fire_TInyMage,
        Earth_Fighter,
        Dark_Artsian,
        Fire_Mage,
    }

    //이곳에서
    public enum EDrawPatternPreset 
    {
        Default_Thunder,
    }

    //패턴과 관련한 정보들이 저장되어있는 구간.
    public static class S_PatternStorage
    {
        public static Dictionary<EDrawPatternPreset, List<int>> S_PatternPresetDic = 
            new Dictionary<EDrawPatternPreset, List<int>>() 
            {
                {EDrawPatternPreset.Default_Thunder,new List<int>(){1,3,4,6,7,5} }
            };
    }


    //여기서 액션이란 적의 액션을 의미한다.
    [Serializable]
    public class EnemyActionData 
    {
        public EnemyAction _action = EnemyAction.BasicAttack_1;
        public float _actionTime = 5;
        public SpawnType _spawnType = SpawnType.SetAtOnce_WithSame;
        // gradually 에서만 쓰임.
        public float _placeTimeGradual = 0.5f;
        public List<Vector2> _placeList = new List<Vector2>();


        public List <SpawnData> _spawnDataList = new List<SpawnData>();
    }
    [Serializable]
    public class SpawnData
    {
        //파괴되는 시간 또는 작동이 정지하는 시간.
        public ObstacleID _thisID;
        public float _lifeTime= 4;
        //발동전까지의 시간.
        public float _activeTime = 1;
        public float _damage_Of_Spawn= 8;
    }


    //적 패턴은 많아야 4개고  중간 이상의 적에게는 필살기 1개 정도 있다고 보면 될듯함
    public enum EnemyAction 
    {
        Wait, 
        BasicAttack_1,
        BasicAttack_2,
        BasicAttack_3,
        SpecialAttack

    }
    //스폰하는 방식을 의미함
    //한번에 같은거 소환하는가
    //그런 류를 정하는 곳
    public enum SpawnType 
    {
        SetAtOnce_WithSame,
        SetGradually_WithSame,
        SetAtOnce_WithDifferent,
        SetGradually_WithDifferent,
        SetRandomly
    }


    [System.Serializable]
    public class StageInfo
    {
        //그냥 스테이지 데이터
        public string _stageName = "Sample_Map";
        public EStageKind _stageKind = EStageKind.Earth_Fighter;

        // float array는 순차적으로, 한 배틀 당의 시간 쿨타임간의 관계임 
        //그리고 아직은 그냥 대강 놓은 거고 대부분의 내용들은 아이템 얻어가면서 바꿔갈것.
        public StageInfo(EStageKind _argname)
        {
            _stageKind = _argname;
        } 
    }

    // 스테이지의 적은 큐에 따라서 나올 것이며
    // 슬레이더 스파이어 처럼 정해진 적들이 나온다. 
    // 
    public static class StageDataPool
    {
        public static Dictionary<int, StageInfo> StageinfoDic
            = new Dictionary<int, StageInfo> {
                { 0, new StageInfo(EStageKind.Fire_TInyMage)},
                { 1, new StageInfo(EStageKind.Earth_Fighter)},
                { 2, new StageInfo(EStageKind.Dark_Artsian)},
                { 2, new StageInfo(EStageKind.Fire_Mage)},
            };

        public static Dictionary<string, List<CharactorActionInfo>> StrActionListDic = 
            new Dictionary<string, List<CharactorActionInfo>>() {
                { "Basic",new List<CharactorActionInfo>() {new CharactorActionInfo()}
                }
            };

        public static Dictionary<string, CharactorActionInfo> StrActionDic =
            new Dictionary<string, CharactorActionInfo>() {
                {"Attack_1", new CharactorActionInfo() }

            };

    }
    
}
