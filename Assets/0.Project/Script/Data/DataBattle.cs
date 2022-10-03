using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PG.Data
{
    /// <summary>
    /// ��� ȭ���� �̹��������� ����
    /// </summary>

    //���ϰ� ������ �������� ����Ǿ��ִ� ����.
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

        //��Ƽ��Ʈ�� ��ġ�� �ൿ�� �и��Ͽ��� �Ѵ�.�׷��� ���߿� �ؽ�Ʈ ó���Ҷ� ���ϴ�.
        //�׸��� ���Ŀ� ��ũ Ʈ��, �������� ������ ������ �Ҷ� ������ ���� ���ü��� �ִٸ� ��������.

        public static Dictionary<ArtifactID, ArtifactData> TotalArtifactTableDataDic =
            new Dictionary<ArtifactID, ArtifactData>()
            {
                
                #region//���� ����.
                {ArtifactID.Thunder_Manimekhala, new ArtifactData(
                    ArtifactID.Thunder_Manimekhala,
                    (int)ArtifactRarity.Common,
                    true,
                    0)},
                {ArtifactID.LoveAndPeace, new ArtifactData(
                    ArtifactID.LoveAndPeace,
                    (int)ArtifactRarity.Common,
                    true,
                    0)},


                #endregion


                #region//�ӽö� ���� �Ϸ�

                {ArtifactID.FragileRush, new ArtifactData(
                    ArtifactID.FragileRush,
                    (int)ArtifactRarity.Common,
                    true,
                    0)},
                {ArtifactID.Equatore, new ArtifactData(
                    ArtifactID.Equatore,
                    (int)ArtifactRarity.Common,
                    true,
                    0)},
                {ArtifactID.BubbleGun, new ArtifactData(
                    ArtifactID.BubbleGun,
                    (int)ArtifactRarity.Common,
                    true,
                    0)},
                {ArtifactID.QuickSlice, new ArtifactData(
                    ArtifactID.QuickSlice,
                    (int)ArtifactRarity.Common,
                    true,
                    0)},
                {ArtifactID.BulletTeleportShooter, new ArtifactData(
                    ArtifactID.BulletTeleportShooter,
                    (int)ArtifactRarity.Common,
                    true,
                    0)},
               
                {ArtifactID.PadThai, new ArtifactData(
                    ArtifactID.PadThai,
                    (int)ArtifactRarity.Common,
                    true,
                    0)},
                {ArtifactID.SesameOil, new ArtifactData(
                    ArtifactID.SesameOil,
                    (int)ArtifactRarity.Common,
                    true,
                    0)},
                {ArtifactID.Pinocchio, new ArtifactData(
                    ArtifactID.Pinocchio,
                    (int)ArtifactRarity.Common,
                    true,
                    0)},
                {ArtifactID.AtomSetting, new ArtifactData(
                    ArtifactID.AtomSetting,
                    (int)ArtifactRarity.Common,
                    true,
                    0)},
               
                #endregion

            };

        public static Dictionary<ArtifactID, Artifact> TotalArtifactClassDic =
            new Dictionary<ArtifactID, Artifact>()
            {
                {ArtifactID.Thunder_Manimekhala, new Artifact_Thunder_Manimekhala()},
                {ArtifactID.LoveAndPeace, new Artifact_LoveAndPeace()},
                {ArtifactID.FragileRush, new Arfifact_FragileRush()},
                {ArtifactID.BubbleGun, new Arfifact_BubbleGun()},
                {ArtifactID.Equatore, new Arfifact_Equatore()},
                {ArtifactID.QuickSlice, new Arfifact_QuickSlice()},
                {ArtifactID.PadThai, new Arfifact_PadThai()},
                {ArtifactID.SesameOil, new Arfifact_SesameOil()},
                {ArtifactID.BulletTeleportShooter, new Arfifact_BulletTeleportShooter()},
                {ArtifactID.Pinocchio, new Arfifact_Pinnochio()},
                {ArtifactID.AtomSetting, new Arfifact_AtomSetting()},

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

    //���⼭ �׼��̶� ���� �׼��� �ǹ��Ѵ�.
    [Serializable]
    public class EnemyActionData
    {
        public EnemyActionID _action = EnemyActionID.BasicAttack_1;
        public float _actionTime = 5;
        public SpawnType _spawnType = SpawnType.SetAtOnce_WithSame;
        // gradually ������ ����.
        public float _placeTimeGradual = 0.5f;
        public List<Vector2> _placeList = new List<Vector2>();
        public List<SpawnData> _spawnDataList = new List<SpawnData>();
        public List<float> _placetimeList = new List<float>();

    }
    [Serializable]
    public class SpawnData
    {
        //�ı��Ǵ� �ð� �Ǵ� �۵��� �����ϴ� �ð�.
        public ObstacleID _thisID;
        public float _lifeTime = 4;
        //�ߵ��������� �ð�.
        public float _activeTime = 1;
        //public float _damageMag = 1;
    }



    //�� ������ ���ƾ� 4����  �߰� �̻��� �����Դ� �ʻ�� 1�� ���� �ִٰ� ���� �ɵ���
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

        public List<Vector2> _placeList = new List<Vector2>();
        public List<SpawnData> _spawnDataList = new List<SpawnData>();

    }
    [Serializable]
    public class MobSpawnData
    {
        [Header("����")]
        public int _������;
        public float _�������ð�;
        public float _������������;
        [Header("����")]
        public float _�ӵ�;
        public int _ü��;
        public float _���ݷ�;
        public Color _����;
    }

    //�����ϴ� ����� �ǹ���
    //�ѹ��� ������ ��ȯ�ϴ°�
    //�׷� ���� ���ϴ� ��
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
        //�׳� �������� ������
        public string _stageName = "Sample_Map";
        public StageKind _stageKind = StageKind.Earth_Fighter;

        // float array�� ����������, �� ��Ʋ ���� �ð� ��Ÿ�Ӱ��� ������ 
        //�׸��� ������ �׳� �밭 ���� �Ű� ��κ��� ������� ������ ���鼭 �ٲ㰥��.
        public StageInfo(StageKind _argname)
        {
            _stageKind = _argname;
        }
    }

    // ���������� ���� ť�� ���� ���� ���̸�
    // �����̴� �����̾� ó�� ������ ������ ���´�. 
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
        public void SetActiveLazer(bool var);
        public void SetLazerEachPos(Vector3 _start, Vector3 _end);
    }

}