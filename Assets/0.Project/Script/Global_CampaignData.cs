using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Battle;
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
    [System.Serializable]
    public static class Global_CampaignData
    {
        public static Dictionary<ArtifactID, Artifact> _currentArtifactDictionary = 
            new Dictionary<ArtifactID, Artifact>();

        //public static Dictionary<ArtifactID, ArtifactData> _currentActivateDictionary =
            //new Dictionary<ArtifactID, ArtifactData>();
        public static Enemy_Script _currentEnemy;

        //������ �����͸� ���� ��ġ �ϴ� �κ�. �ѹ��� ���� ���� �� ���������� �ƹ�ư.
        //���� ������ ������ ����.
        public static Dictionary<CharacterID, DataEntity> _charactorAttackDic = 
            new Dictionary<CharacterID, DataEntity>() 
            {
                { CharacterID.Player,new DataEntity(DataEntity.Type.Damage,10)},
                { CharacterID.Enemy_Fireboy,new DataEntity(DataEntity.Type.Damage,8)},
                { CharacterID.Enemy_WindShooter,new DataEntity(DataEntity.Type.Damage,16)},

            };

        //�Ÿ������� ������ 
        public static DataEntity _lengthMagnData = new DataEntity(DataEntity.Type.LengthMag, 1);
        public static DataEntity _chargeGaugeData = new DataEntity(DataEntity.Type.ChargeGauge, 8);
        public static DataEntity _chargeEXPData = new DataEntity(DataEntity.Type.ChargeGauge, 8);

        //�÷��̾� ������
        public static DataEntity _playerSize= new DataEntity(DataEntity.Type.LengthMag, 1);



    }




}
