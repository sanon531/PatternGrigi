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
    /// ��� ȭ���� �̹��������� ����
    /// </summary>

    public enum EStageKind
    {
        Earth_Fighter,
        Fire_Mage,
        Dark_Artistian
    }

    //�̰�����
    public enum EDrawPatternPreset 
    {
        Default_Thunder,
        
       
    }

    public static class S_PatternStorage
    {
        public static Dictionary<EDrawPatternPreset, List<int>> S_PatternPresetDic = 
            new Dictionary<EDrawPatternPreset, List<int>>() 
            {
                {EDrawPatternPreset.Default_Thunder,new List<int>(){1,3,4,6,2,5,7 } }
            };
    }

    [Serializable]
    public class ActionData 
    {
        public EnemyAction _action = EnemyAction.BasicAttack_1;
        public float _actionTime = 5;
        public SpawnType _spawnType = SpawnType.SetAtOnce_WithSame;
        // gradually ������ ����.
        public float _placeTimeGradual = 0.5f;
        public List<Vector2> _placeList = new List<Vector2>();


        public List <SpawnData> _spawnDataList = new List<SpawnData>();
    }
    [Serializable]
    public class SpawnData
    {
        //�ı��Ǵ� �ð� �Ǵ� �۵��� �����ϴ� �ð�.
        public ObstacleID _thisID;
        public float _lifeTime= 4;
        //�ߵ��������� �ð�.
        public float _activeTime = 1;
        public float _damage_Of_Spawn= 8;
    }


    //�� ������ ���ƾ� 4����  �߰� �̻��� �����Դ� �ʻ�� 1�� ���� �ִٰ� ���� �ɵ���
    public enum EnemyAction 
    {
        Wait, 
        BasicAttack_1,
        BasicAttack_2,
        BasicAttack_3,
        SpecialAttack

    }
    //�����ϴ� ����� �ǹ���
    //�ѹ��� ������ ��ȯ�ϴ°�
    //�׷� ���� ���ϴ� ��
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
        //�׳� �������� ������
        public string _stageName = "Sample_Map";
        public EStageKind _stageKind = EStageKind.Earth_Fighter;

        // float array�� ����������, �� ��Ʋ ���� �ð� ��Ÿ�Ӱ��� ������ 
        //�׸��� ������ �׳� �밭 ���� �Ű� ��κ��� ������� ������ ���鼭 �ٲ㰥��.
        public StageInfo(EStageKind _argname)
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
                { 0, new StageInfo(EStageKind.Dark_Artistian)}
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
