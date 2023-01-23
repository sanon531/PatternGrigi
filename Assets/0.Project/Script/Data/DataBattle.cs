using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PG.Data
{
    /// <summary>
    /// 배경 화면의 이미지에대한 설정
    /// </summary>

    //패턴과 관련한 정보들이 저장되어있는 구간.
    public static class GlobalDataStorage
    {
        public static Dictionary<DrawPatternPresetID, List<int>> PatternPresetDic =
            new Dictionary<DrawPatternPresetID, List<int>>()
            {
                {DrawPatternPresetID.Thunder_Manimekhala,new List<int>(){1,3,4,6,7,5} },
                {DrawPatternPresetID.LoveAndPeace,new List<int>(){4,2,5,7,3,0,4} },
                {DrawPatternPresetID.Sandglass,new List<int>(){2,0,8,6,2} },
                {DrawPatternPresetID.Empty_Breath,new List<int>(){4}},
            };
        public static Dictionary<DrawPatternPresetID, PresetPatternAction_Base> PatternWIthActionDic =
            new Dictionary<DrawPatternPresetID, PresetPatternAction_Base>()
            {
                {DrawPatternPresetID.Thunder_Manimekhala,new PresetPattern_Thunder_Manimekhala()},
                {DrawPatternPresetID.LoveAndPeace,new PresetPattern_LoveAndPeace()},
                {DrawPatternPresetID.Sandglass,new PresetPattern_Sandglass() },

                {DrawPatternPresetID.Empty_Breath,new PresetPatternAction_Base() }
            };

        #region// 유물

        //아티팩트의 수치와 행동을 분리하여야 한다.그래야 나중에 텍스트 처리할때 편하다.
        //그리고 이후에 테크 트리, 점진적인 레벨링 같은거 할때 다음과 같이 나올수가 있다면 좋을듯함.

        public static Dictionary<ArtifactID, Artifact> TotalArtifactClassDic =
            new Dictionary<ArtifactID, Artifact>()
            {

                #region//10월 업데이트 이전의 유물들
                {ArtifactID.Thunder_Manimekhala, new Artifact_Thunder_Manimekhala()},
                {ArtifactID.LoveAndPeace, new Artifact_LoveAndPeace()},
                {ArtifactID.FragileRush, new Artifact_FragileRush()},
                {ArtifactID.BubbleGun, new Artifact_BubbleGun()},
                {ArtifactID.Equatore, new Artifact_Equatore()},
                {ArtifactID.QuickSlice, new Artifact_QuickSlice()},
                {ArtifactID.PadThai, new Artifact_PadThai()},
                {ArtifactID.SesameOil, new Artifact_SesameOil()},
                {ArtifactID.BulletTeleportShooter, new Artifact_BulletTeleportShooter()},
                {ArtifactID.Pinocchio, new Artifact_Pinnochio()},
                {ArtifactID.AtomSetting, new Artifact_AtomSetting()},
                #endregion
                {ArtifactID.Upgrade_AimShot, new Artifact_UpgradeAimShot()},
                {ArtifactID.Upgrade_StraightShot, new Artifact_UpgradeStraightShot()},
                {ArtifactID.Upgrade_LightningShot, new Artifact_UpgradeLightningShot()},


            };

        public static Dictionary<DrawPatternPresetID, LaserKindID> PatternWIthLaserDic =
            new Dictionary<DrawPatternPresetID, LaserKindID>()
            {
                {DrawPatternPresetID.Thunder_Manimekhala,LaserKindID.Electric_lightening },
                {DrawPatternPresetID.Empty_Breath,LaserKindID.Default_laser},
                {DrawPatternPresetID.LoveAndPeace,LaserKindID.Love_Laser},
                {DrawPatternPresetID.Sandglass,LaserKindID.Default_laser},

            };

    }



    #endregion
    [System.Serializable]
    public class ProjectileData
    {
        public int _count;
        public int _repeat;
        public float _cooltime;
        public ProjectileData(int count) 
        {
            _count = count;
            _repeat = 0;
            _cooltime = 0.5f;
        }
        public ProjectileData(int count, int repeat)
        {
            _count = count;
            _repeat = repeat;
            _cooltime = 0.5f;
        }
        public ProjectileData(int count, int repeat, float cooltime)
        {
            _count = count;
            _repeat = repeat;
            _cooltime = cooltime;
        }
        public ProjectileData(ProjectileData data)
        {
            this._count = data._count;
            this._repeat = data._repeat;
            this._cooltime = data._cooltime;
        }
        public void SetCount(int val)
        {
            this._count = val;
        }
        public void SetRepeat(int val)
        {
            this._repeat = val;
        }
        public void SetCoolTime(float val)
        {
            this._cooltime = val;
        }

        public void IncreaseCount(int val)
        {
            this._count += val;
        }
        public void DecreaseCount()
        {
            _count--;
        }
    }

    //여기서 액션이란 적의 액션을 의미한다.
    [Serializable]
    public class EnemyActionData
    {
        public EnemyActionID _action = EnemyActionID.BasicAttack_1;
        public float _actionTime = 5;
        public SpawnType _spawnType = SpawnType.SetAtOnce_WithSame;
        // gradually 에서만 쓰임.
        public float _placeTimeGradual = 0.5f;
        public List<Vector2> _placeList = new List<Vector2>();
        public List<SpawnData> _spawnDataList = new List<SpawnData>();
        public List<float> _placetimeList = new List<float>();

    }
    [Serializable]
    public class SpawnData
    {
        //파괴되는 시간 또는 작동이 정지하는 시간.
        public ObstacleID _thisID;
        public float _lifeTime = 4;
        //발동전까지의 시간.
        public float _activeTime = 1;
        //public float _damageMag = 1;
    }



    //적 패턴은 많아야 4개고  중간 이상의 적에게는 필살기 1개 정도 있다고 보면 될듯함
    public enum EnemyActionID
    {
        Wait = 0,
        BasicAttack_1 = 1,
        BasicAttack_2 = 2,
        BasicAttack_3 = 3,
        SpecialAttack = 4,
        BasicAttack_4 = 5,
        BasicAttack_5 = 6,
        Stunned = 99

    }
    public enum LaserKindID 
    {
        Default_laser= 0,
        Electric_lightening =1,
        Love_Laser = 2,

    }


    public enum MobActionID
    {
        Wait = 0,
        Move = 1,
        Attack = 2,
        Stunned = 99

    }

    [Serializable]
    public class MobActionData
    {
        public float _actionTime = 5;
        //public float _speed = 3;
        public float _acceleration = 0;
        public List<MobAttackData> _mobAttackDataList;
    }

    [Serializable]
    public class MobAttackData
    {
        public bool _mobPosSpawn = false;
        public List<Vector2> _spawnPosList = new List<Vector2>();
        public SpawnData _spawnData = new SpawnData();
    }
    
    [Serializable]
    public class MobSpawnData
    {
        [Header("스폰")]
        public int _스폰수;
        public float _스폰대기시간;
        public float _리스폰딜레이;
        [Header("설정")]
        public float _속도;
        public int _체력;
        public float _공격력;
        public Color _색깔;
    }

    //스폰하는 방식을 의미함
    //한번에 같은거 소환하는가
    //그런 류를 정하는 곳
    public enum SpawnType
    {
        SetAtOnce_WithSame = 0,
        SetGradually_WithSame = 1,
        SetAtOnce_WithDifferent = 2,
        SetGradually_WithDifferent = 3,
        SetPresettime_WithSame = 4,
        SetPresettime_WithDifference = 5,

        SetRandomly = 99
    }
    public enum ObstacleID
    {
        Small_Fire = 0,
        LongThinFire_Vertical = 1,
        LongThinFire_Horizontal = 2,
        ThinLaser = 3,
        Chase_Obstacle = 4,
        Flack = 5,
        MovingLeafRightToleft = 6,
        MovingLeafLeftToRight = 7,
        MovingLeafUpToDown = 8,
        MovingLeafDownToUp = 9,

        LookAt_Arrow = 10,

    }


    [System.Serializable]
    public class StageInfo
    {
        //그냥 스테이지 데이터
        public string _stageName = "Sample_Map";
        public StageKind _stageKind = StageKind.Earth_Fighter;

        // float array는 순차적으로, 한 배틀 당의 시간 쿨타임간의 관계임 
        //그리고 아직은 그냥 대강 놓은 거고 대부분의 내용들은 아이템 얻어가면서 바꿔갈것.
        public StageInfo(StageKind _argname)
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
                { 0, new StageInfo(StageKind.Fire_TInyMage)},
                { 1, new StageInfo(StageKind.Earth_Fighter)},
                { 2, new StageInfo(StageKind.Dark_Artsian)},
                { 2, new StageInfo(StageKind.Fire_Mage)},
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
    [SerializeField]
    public interface ILazerOnoff
    {
        public void SetActiveLaser(bool var);
        public void SetLaserEachPos(Vector3 _start, Vector3 _end);
    }

}
