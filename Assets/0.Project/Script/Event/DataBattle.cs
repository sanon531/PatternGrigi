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
    /// ��� ȭ���� �̹��������� ����
    /// </summary>

    //���ϰ� ������ �������� ����Ǿ��ִ� ����.
    public static class GlobalDataStorage
    {
        public static Dictionary<DrawPatternPreset, List<int>> PatternPresetDic =
            new Dictionary<DrawPatternPreset, List<int>>()
            {
                {DrawPatternPreset.Default_Thunder,new List<int>(){1,3,4,6,7,5} }
            };


        public static Dictionary<ArtifactID, ArtifactTableData> TotalArtifactTableDataDic =
            new Dictionary<ArtifactID, ArtifactTableData>()
            {
                {ArtifactID.FragileRush, new ArtifactTableData(
                    ArtifactID.FragileRush,
                    "","",
                    (int)Rarity.Common,true,0)},
                {ArtifactID.Equatore, new ArtifactTableData(
                    ArtifactID.Equatore,
                    "","",
                    (int)Rarity.Common,true,0)},
                {ArtifactID.PoloNord, new ArtifactTableData(
                    ArtifactID.PoloNord,
                    "","",
                    (int)Rarity.Common,true,0)},
                {ArtifactID.StrangeTropics, new ArtifactTableData(
                    ArtifactID.StrangeTropics,
                    "","",
                    (int)Rarity.Common,true,0)},
            };


    }


    //���⼭ �׼��̶� ���� �׼��� �ǹ��Ѵ�.
    [Serializable]
    public class EnemyActionData
    {
        public EnemyAction _action = EnemyAction.BasicAttack_1;
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
        public float _damage_Of_Spawn = 8;
    }


    //�� ������ ���ƾ� 4����  �߰� �̻��� �����Դ� �ʻ�� 1�� ���� �ִٰ� ���� �ɵ���
    public enum EnemyAction
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
    //�����ϴ� ����� �ǹ���
    //�ѹ��� ������ ��ȯ�ϴ°�
    //�׷� ���� ���ϴ� ��
    public enum SpawnType
    {
        SetAtOnce_WithSame=0,
        SetGradually_WithSame=1,
        SetAtOnce_WithDifferent=2,
        SetGradually_WithDifferent=3,
        SetPresettime_WithSame= 4,
        SetPresettime_WithDifference = 5,

        SetRandomly = 99
    }
    public enum ObstacleID
    {
        SmallFire = 0,
        LongThinFire_Vertical = 1,
        LongThinFire_Horizontal = 2,
        ThinLaser = 3,
        SmallMissile = 4,
        Flack = 5,
        MovingLeafRightToleft = 6,
        MovingLeafLeftToRight = 7,
        MovingLeafUpToDown = 8,
        MovingLeafDownToUp = 9,



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

}
