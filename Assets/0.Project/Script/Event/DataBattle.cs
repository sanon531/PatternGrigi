using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace PG.Event 
{
    public interface ISetLevelupPause
    {
        void SetLevelUpPauseOn();
        void SetLevelUpPauseOff();
    }

}

namespace PG.Data
{
    /// <summary>
    /// 배경 화면의 이미지에대한 설정
    /// </summary>
    public enum BGImageKind
    {
        AGR_1,
        CYN_1,
        FRN_1,
        BOSS_ARG,
        BOSS_CYN
    }
 

    public enum StageKind
    {

    }
    [Serializable]
    public class ActionData 
    {
        public EnemyAction _action = EnemyAction.BasicAttack_1;
        public float _actionTime = 5;
        public SpawnType _spawnType = SpawnType.SetAtOnce_WithSame;
        /// <summary>
        /// gradually 에서만 쓰임.
        /// </summary>
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


    public enum EnemyAction 
    {
        Wait, 
        BasicAttack_1,
        BasicAttack_2,
        BasicAttack_3,
        SpecialAttack

    }
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
        public float _battleTime = 10;
        public float _spawnSpeed = 10;

        public float _startCoolTime = 10f;
        public float _battleCoolTime = 10f;

        //적의 정보
        public List<CharacterID> _stageEnemys;

        // float array는 순차적으로, 한 배틀 당의 시간 쿨타임간의 관계임 
        //그리고 아직은 그냥 대강 놓은 거고 대부분의 내용들은 아이템 얻어가면서 바꿔갈것.
        public StageInfo(string _argname,float[] _argFloatData,List<CharacterID> _argStageEnemys)
        {
            _stageName = _argname;
            _battleTime = _argFloatData[0];
            _spawnSpeed = _argFloatData[1];
            _startCoolTime = _argFloatData[2];
            _battleCoolTime = _argFloatData[3];
            _stageEnemys = _argStageEnemys;
        } 
    }

    // 스테이지의 적은 큐에 따라서 나올 것이며
    // 슬레이더 스파이어 처럼 정해진 적들이 나온다. 
    // 
    public static class StageDataPool
    {
        public static Dictionary<string, StageInfo> StageinfoDic
            = new Dictionary<string, StageInfo> {
                { "멸고단_1",
                    new StageInfo("멸고단_1",new float[]{
                        25f,1f,5f,10f},
                        new List<CharacterID>(){
                            CharacterID.멸고단원
                        }
                )},
            


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
