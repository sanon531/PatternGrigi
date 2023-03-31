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
        //아이디만 저장해두면 나중에 시작할때
        public List<ArtifactID> _startArtifactList  =
            new List<ArtifactID>();

        public List<ArtifactID> _obtainableArtifactIDList = new List<ArtifactID>()
        {
            ArtifactID.PadThai,
            ArtifactID.FragileRush,
            ArtifactID.BubbleGun,
            ArtifactID.SesameOil,
            ArtifactID.MuscleGodBlessing,
            ArtifactID.QuickSlice,
            ArtifactID.BulletTeleportShooter

        };

        //적들의 데이터를 먼저 매치 하는 부분. 한버넹 강한 공격 도 공격이지만 아무튼.
        //값은 언제든 수정이 가능.
        public CharactorIDDataEntityDic _charactorAttackDic =
           new CharactorIDDataEntityDic()
           {
                { CharacterID.Player,new DataEntity(DataEntity.Type.Damage,10)},
                { CharacterID.Enemy_Fireboy,new DataEntity(DataEntity.Type.Damage,8)},
                { CharacterID.Enemy_WindShooter,new DataEntity(DataEntity.Type.Damage,16)},
                { CharacterID.Slime,new DataEntity(DataEntity.Type.Damage,8)},
                { CharacterID.Tempt_Mob,new DataEntity(DataEntity.Type.Damage,8)},
                { CharacterID.Tempt_Mob2,new DataEntity(DataEntity.Type.Damage,8)},
           };

        public TimeWaveDic _waveDic = new TimeWaveDic();


        public DrawPatternPresetID _currentChargePattern = DrawPatternPresetID.Empty_Breath;

        //거리에따른 배율임 

        public DataEntity _lengthMagnData = new DataEntity(DataEntity.Type.LengthMag, 1);
        public DataEntity _chargeGaugeData = new DataEntity(DataEntity.Type.ChargeGauge, 8);
        public DataEntity _chargeEXPData = new DataEntity(DataEntity.Type.ChargeEXP, 8);

        //플레이어 사이즈
        public DataEntity _playerSize = new DataEntity(DataEntity.Type.PlayerSize, 1);
        public float _playerHealth = 100;
        public ProjectileIDDataDic _projectileIDDataDic = new ProjectileIDDataDic() 
        {
            { ProjectileID.NormalBullet, new ProjectileData(0)},
            { ProjectileID.StraightShot, new ProjectileData(0)},
            { ProjectileID.TowerBullet, new ProjectileData(0)},
            { ProjectileID.LightningShot, new ProjectileData(0)},
            { ProjectileID.CuttingKnife, new ProjectileData(0)},
        };
        public float _playerSpeed = 30;
        public float _playerTeleport =  0.6f;
        public float _playerCollisionDamage =  1f;

        public DataEntity _projectileSpeed = new DataEntity(DataEntity.Type.ProjectileSpeed, 5);
        public DataEntity _projectileTargetNum = new DataEntity(DataEntity.Type.ProjectileCount, 1);
        public DataEntity _randomPatternNodeCount = new DataEntity(DataEntity.Type.RandomPatternCount, 3);
        public List<float> _levelMaxEXPList = new List<float>(){100};
        public float _killGetEXP = 1;

    }



}