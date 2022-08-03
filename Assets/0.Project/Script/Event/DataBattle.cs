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
        Earth_Fighter,
        Fire_Mage,
        Dark_Artistian
    }
    [Serializable]
    public class ActionData 
    {
        public EnemyAction _action = EnemyAction.BasicAttack_1;
        public float _actionTime = 5;
        public SpawnType _spawnType = SpawnType.SetAtOnce_WithSame;
        /// <summary>
        /// gradually ������ ����.
        /// </summary>
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
                { 0, new StageInfo(StageKind.Dark_Artistian)}
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
