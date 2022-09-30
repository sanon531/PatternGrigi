using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Battle;
using System.Linq;
namespace PG.Data
{
    public struct Data_CampaignOption
    {
        public Data_CampaignOption( CharacterID characterID, ArtifactID? artifactID, bool masterKeyUsed)
        {
            character = characterID;
            startArtifact = artifactID;
            this.masterKeyUsed = masterKeyUsed;
        }
        public CharacterID character { get; }
        public ArtifactID? startArtifact { get; }
        public bool masterKeyUsed { get; }
    }



    //�ΰ��� ���ο��� �۵��� �ϴºκ���.
    [System.Serializable]
    public static class Global_CampaignData 
    {
        public static List<ArtifactID> _startArtifactList =  new List<ArtifactID>();

        public static ArtifactIDArtifactDic _currentArtifactDictionary =
            new ArtifactIDArtifactDic();

        //public static Dictionary<ArtifactID, ArtifactData> _currentActivateDictionary =
        //new Dictionary<ArtifactID, ArtifactData>();
        public static Enemy_Script _currentEnemy;

        //������ �����͸� ���� ��ġ �ϴ� �κ�. �ѹ��� ���� ���� �� ���������� �ƹ�ư.
        //���� ������ ������ ����.
        public static CharactorIDDataEntityDic _charactorAttackDic =new CharactorIDDataEntityDic();

        public static List<ArtifactID> _obtainableArtifactIDList = new List<ArtifactID>();




        #region//�÷��� ������ ����

        public static DrawPatternPresetID _currentChargePattern = DrawPatternPresetID.Empty_Breath;

        //�Ÿ������� ������ 
        public static DataEntity _lengthMagnData = new DataEntity(DataEntity.Type.LengthMag, 1);
        public static DataEntity _chargeGaugeData = new DataEntity(DataEntity.Type.ChargeGauge, 8);
        public static DataEntity _chargeEXPData = new DataEntity(DataEntity.Type.ChargeEXP, 8);
        //�÷��̾� ������
        public static DataEntity _playerSize = new DataEntity(DataEntity.Type.PlayerSize, 1);
        public static DataEntity _projectileSpeed = new DataEntity(DataEntity.Type.ProjectileSpeed, 5);
        public static DataEntity _projectileTargetNum = new DataEntity(DataEntity.Type.ProjectileCount, 1);
        public static DataEntity _randomPatternNodeCount = new DataEntity(DataEntity.Type.RandomPatternCount,3);
        //���� ��Ÿ�� ����
        public static DataEntity _coolTimeTokenCount = new DataEntity(DataEntity.Type.MaxCooltimeToken, 3);

        public static ProjectileIDDataDic _projectileIDDataDic = new ProjectileIDDataDic(){};
        public static List<float> _waveTimeList = new List<float>();
        public static List<WaveClass> _waveClassList = new List<WaveClass>();

        #endregion;

        #region



        #endregion

        #region//�ʱ�ȭ ����

        //������ ������ ���� �Ǿ��� �� Ȱ���Ѵ�.�����͸� �����ϴ°�쿡 Ȱ���Ѵ�.
        public static void ResetData() 
        {
            _currentArtifactDictionary.Clear();
            _charactorAttackDic.Clear();
            _obtainableArtifactIDList.Clear();

        }

        //�̰����� ������ �������� �����͸� �����Ѵ�.
        public static void SetCampaginInitialize(CampaignData data) 
        {
            ResetData();
            _startArtifactList = data._startArtifactList.ToList();
            //Debug.Log(data._charactorAttackDic.GetType());
            _charactorAttackDic.CopyFrom(data._charactorAttackDic);
            _obtainableArtifactIDList = data._obtainableArtifactIDList.ToList();
            _currentChargePattern = data._currentChargePattern;

            _lengthMagnData = new DataEntity(data._lengthMagnData);
            _chargeGaugeData = new DataEntity(data._chargeGaugeData);
            _chargeEXPData = new DataEntity(data._chargeEXPData);
            _playerSize = new DataEntity(data._playerSize);
            _projectileSpeed = new DataEntity(data._projectileSpeed);
            _projectileTargetNum = new DataEntity(data._projectileTargetNum);

            _projectileIDDataDic.CopyFrom(data._projectileIDDataDic);

            _randomPatternNodeCount = new DataEntity(data._randomPatternNodeCount);
            _waveTimeList = new List<float>(data._waveDic.Keys);
            //�����ε����Ϳ� ���̺� �ð� ������ �� ������������ �Է� ���ص� �ǵ��� ����+�̿��°� class����Ʈ ����
            _waveTimeList.Sort();
            foreach (float key in _waveTimeList)
            {
                _waveClassList.Add(data._waveDic[key]);
                //Debug.Log(key);
            }
            //���� �׷��ʿ������ �̰� _waveClassList = new List<WaveClass>(data._waveDic.Values);
        }
        #endregion
    }

  


}