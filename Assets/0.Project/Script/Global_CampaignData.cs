using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
namespace PG.Battle
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

        //적들의 데이터를 먼저 매치 하는 부분. 한버넹 강한 공격 도 공격이지만 아무튼.
        //값은 언제든 수정이 가능.
        public static Dictionary<CharacterID, DataEntity> _charactorAttack = 
            new Dictionary<CharacterID, DataEntity>() 
            {
                { CharacterID.player,new DataEntity(DataEntity.Type.Damage,10)},
                { CharacterID.Enemy_Fireboy,new DataEntity(DataEntity.Type.Damage,8)},
                { CharacterID.Enemy_WindShooter,new DataEntity(DataEntity.Type.Damage,16)},

            };

        public static DataEntity _chargeGaugeData = new DataEntity(DataEntity.Type.ChargeGauge, 8);

    }




}
