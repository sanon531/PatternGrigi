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
        public static ArtifactIDArtifactDic _currentArtifactDictionary =
            new ArtifactIDArtifactDic();

        //public static Dictionary<ArtifactID, ArtifactData> _currentActivateDictionary =
        //new Dictionary<ArtifactID, ArtifactData>();
        public static Enemy_Script _currentEnemy;

        //������ �����͸� ���� ��ġ �ϴ� �κ�. �ѹ��� ���� ���� �� ���������� �ƹ�ư.
        //���� ������ ������ ����.
        public static CharactorIDDataEntityDic _charactorAttackDic =new CharactorIDDataEntityDic();

        public static List<ArtifactID> _obtainableArtifactIDList = new List<ArtifactID>();
        //�Ÿ������� ������ 
        public static DataEntity _lengthMagnData = new DataEntity(DataEntity.Type.LengthMag, 1);
        public static DataEntity _chargeGaugeData = new DataEntity(DataEntity.Type.ChargeGauge, 8);
        public static DataEntity _chargeEXPData = new DataEntity(DataEntity.Type.ChargeEXP, 8);

        //�÷��̾� ������
        public static DataEntity _playerSize = new DataEntity(DataEntity.Type.PlayerSize, 1);

        #region//�ʱ�ȭ ����

        //������ ������ ���� �Ǿ��� �� Ȱ���Ѵ�.�����͸� �����ϴ°�쿡 Ȱ���Ѵ�.
        public static void ResetData() 
        {
            _currentArtifactDictionary.Clear();
        }

        //�̰����� ������ �������� �����͸� �����Ѵ�.
        public static void SetCampaginInitialize(CampaignData data) 
        {
            if(data._currentArtifactDictionary.Count !=0)
                _currentArtifactDictionary.CopyFrom(data._currentArtifactDictionary);
            Debug.Log(data._charactorAttackDic.GetType());
            _charactorAttackDic.CopyFrom(data._charactorAttackDic);
            _obtainableArtifactIDList = data._obtainableArtifactIDList.ToList();

            _lengthMagnData = new DataEntity(data._lengthMagnData.type, data._lengthMagnData.FinalValue);
            //Debug.Log(data._lengthMagnData.FinalValue);
            _chargeGaugeData = new DataEntity(data._chargeGaugeData.type, data._chargeGaugeData.FinalValue);
            _chargeEXPData = new DataEntity(data._chargeEXPData.type, data._chargeEXPData.FinalValue);
            _playerSize = new DataEntity(data._playerSize.type, data._playerSize.FinalValue);
        }
        #endregion
    }

  


}