using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Battle;
using System.Linq;
namespace PG.Data
{
    //그냥 해당하는버전을 여러개 만들어둘수있으면 좋을것 같아서 그럼
    [CreateAssetMenu(fileName = "Global_CampaignData", menuName = "PG/GlobalCampaignData")]
    public class CampaignData : ScriptableObject
    {
        public ArtifactIDArtifactDic _currentArtifactDictionary =
            new ArtifactIDArtifactDic();

        public List<ArtifactID> _obtainableArtifactIDList = new List<ArtifactID>()
        {
            ArtifactID.PadThai,
            ArtifactID.FragileRush,
            ArtifactID.BubbleGun,
            ArtifactID.SesameOil,
            ArtifactID.Equatore,
            ArtifactID.QuickSlice

        };


        //적들의 데이터를 먼저 매치 하는 부분. 한버넹 강한 공격 도 공격이지만 아무튼.
        //값은 언제든 수정이 가능.
        public CharactorIDDataEntityDic _charactorAttackDic =
           new CharactorIDDataEntityDic()
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