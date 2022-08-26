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

    //인게임 내부에서 작동을 하는부분임.
    [System.Serializable]
    public static class Global_CampaignData
    {
        public static ArtifactIDArtifactDic _currentArtifactDictionary =
            new ArtifactIDArtifactDic();

        //public static Dictionary<ArtifactID, ArtifactData> _currentActivateDictionary =
        //new Dictionary<ArtifactID, ArtifactData>();
        public static Enemy_Script _currentEnemy;

        //적들의 데이터를 먼저 매치 하는 부분. 한번에 강한 공격 도 공격이지만 아무튼.
        //값은 언제든 수정이 가능.
        public static ArtifactIDDataEntityDic _charactorAttackDic =
            new ArtifactIDDataEntityDic()
            {
            };

        public static List<ArtifactID> _obtainableArtifactIDList = new List<ArtifactID>();
        //거리에따른 배율임 
        public static DataEntity _lengthMagnData = new DataEntity(DataEntity.Type.LengthMag, 1);
        public static DataEntity _chargeGaugeData = new DataEntity(DataEntity.Type.ChargeGauge, 8);
        public static DataEntity _chargeEXPData = new DataEntity(DataEntity.Type.ChargeEXP, 8);

        //플레이어 사이즈
        public static DataEntity _playerSize = new DataEntity(DataEntity.Type.PlayerSize, 1);

        #region//초기화 관련

        //완전히 게임이 종료 되었을 때 활용한다.데이터를 리셋하는경우에 활용한다.
        public static void ResetData() 
        {
            _currentArtifactDictionary.Clear();
        }

        //이곳에서 최초의 스테이지 데이터를 수정한다.
        public static void SetCampaginInitialize(CampaignData data) 
        {
            _currentArtifactDictionary.CopyFrom(data._currentArtifactDictionary);
            _charactorAttackDic.CopyFrom(data._charactorAttackDic);
            _obtainableArtifactIDList = data._obtainableArtifactIDList.ToList();

            _lengthMagnData = new DataEntity(data._lengthMagnData.type, data._lengthMagnData.FinalValue);
            Debug.Log(data._lengthMagnData.FinalValue);
            _chargeGaugeData = new DataEntity(data._chargeGaugeData.type, data._chargeGaugeData.FinalValue);
            _chargeEXPData = new DataEntity(data._chargeEXPData.type, data._chargeEXPData.FinalValue);
            _playerSize = new DataEntity(data._playerSize.type, data._playerSize.FinalValue);
        }
        #endregion
    }

    //그냥 해당하는버전을 여러개 만들어둘수있으면 좋을것 같아서 그럼
    [CreateAssetMenu(fileName = "Global_CampaignData", menuName = "PG/GlobalCampaignData")]
    public class CampaignData : ScriptableObject
    {
        public ArtifactIDArtifactDic _currentArtifactDictionary =
            new ArtifactIDArtifactDic();

        public List<ArtifactID> _obtainableArtifactIDList = new List<ArtifactID>();


        //적들의 데이터를 먼저 매치 하는 부분. 한버넹 강한 공격 도 공격이지만 아무튼.
        //값은 언제든 수정이 가능.
        public ArtifactIDDataEntityDic _charactorAttackDic =
           new ArtifactIDDataEntityDic()
           {
                { CharacterID.Player,new DataEntity(DataEntity.Type.Damage,10)},
                { CharacterID.Enemy_Fireboy,new DataEntity(DataEntity.Type.Damage,8)},
                { CharacterID.Enemy_WindShooter,new DataEntity(DataEntity.Type.Damage,16)},
           };

        //거리에따른 배율임 
        public DataEntity _lengthMagnData = new DataEntity(DataEntity.Type.LengthMag, 1);
        public DataEntity _chargeGaugeData = new DataEntity(DataEntity.Type.ChargeGauge, 8);
        public DataEntity _chargeEXPData = new DataEntity(DataEntity.Type.ChargeEXP, 8);

        //플레이어 사이즈
        public DataEntity _playerSize = new DataEntity(DataEntity.Type.PlayerSize, 1);



    }


}