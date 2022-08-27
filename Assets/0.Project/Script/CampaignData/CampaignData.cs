using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Battle;
using System.Linq;
namespace PG.Data
{
    //�׳� �ش��ϴ¹����� ������ �����Ѽ������� ������ ���Ƽ� �׷�
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


        //������ �����͸� ���� ��ġ �ϴ� �κ�. �ѹ��� ���� ���� �� ���������� �ƹ�ư.
        //���� ������ ������ ����.
        public CharactorIDDataEntityDic _charactorAttackDic =
           new CharactorIDDataEntityDic()
           {
                { CharacterID.Player,new DataEntity(DataEntity.Type.Damage,10)},
                { CharacterID.Enemy_Fireboy,new DataEntity(DataEntity.Type.Damage,8)},
                { CharacterID.Enemy_WindShooter,new DataEntity(DataEntity.Type.Damage,16)},
           };

        //�Ÿ������� ������ 
        public DataEntity _lengthMagnData = new DataEntity(DataEntity.Type.LengthMag, 1);
        public DataEntity _chargeGaugeData = new DataEntity(DataEntity.Type.ChargeGauge, 8);
        public DataEntity _chargeEXPData = new DataEntity(DataEntity.Type.ChargeEXP, 8);

        //�÷��̾� ������
        public DataEntity _playerSize = new DataEntity(DataEntity.Type.PlayerSize, 1);



    }



}