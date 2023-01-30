
using System.Collections;
using System.Collections.Generic;
using PG.Battle;
using UnityEngine;
using PG.Event;
using PG.Data;
namespace PG
{
    public sealed class Artifact_BulletTeleportShooter : Artifact
    {
        public Artifact_BulletTeleportShooter() : base(ArtifactID.BulletTeleportShooter)
        {
        }

        public override void OnGetArtifact()
        {
            base.OnGetArtifact();
            Enable();
        }

        protected override void Enable()
        {
            base.Enable();
            Global_CampaignData._projectileSpeed.Add배수(2);
            Global_CampaignData._projectileTargetNum.Add배수(2);
        }



        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._projectileSpeed.Add배수(2);
            Global_CampaignData._projectileTargetNum.Add배수(2);
        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            Global_CampaignData._projectileSpeed.Add배수(2);
            Global_CampaignData._projectileTargetNum.Add배수(2);
        }


    }

    public sealed class Artifact_Thunder_Colorant : Artifact
    {
        public Artifact_Thunder_Colorant() : base(ArtifactID.Thunder_Colorant)
        {
        }
        public override void OnGetArtifact()
        {
            base.OnGetArtifact();
            Enable();
        }

        protected override void Enable()
        {
            base.Enable();
            BattleExtraAttackManager.StartThunderCall();
        }



        protected override void Disable()
        {
            base.Disable();
            BattleExtraAttackManager.StopThunderCall();

        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            Global_CampaignData._thunderCount.Add증가량(1);
            
        }
    }
    public sealed class Artifact_Decalcomanie : Artifact
    {
        public Artifact_Decalcomanie(ArtifactID artifactID) : base(ArtifactID.Decalcomanie)
        {
        }

        public override void OnGetArtifact()
        {
            base.OnGetArtifact();
            Enable();
        }

        protected override void Enable()
        {
            base.Enable();
            Global_CampaignData._projectileIDDataDic[ProjectileID.NormalBullet]._count++;
        }



        protected override void Disable()
        {
            base.Disable();
        }

        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();
            Global_CampaignData._projectileIDDataDic[ProjectileID.NormalBullet]._count++;
        }



    }


    public sealed class Artifact_Magic_Lenz : Artifact
    {
        public Artifact_Magic_Lenz() : base(ArtifactID.Magic_Lenz)
        {
                
        }
            
        public override void OnGetArtifact()
        {
            base.OnGetArtifact();
            Enable();
        }

        protected override void Enable()
        {
            base.Enable();
            Global_CampaignData._projectilePierce.Add증가량(1);
        }



        protected override void Disable()
        {
            base.Disable();
        }

        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();
            Global_CampaignData._projectilePierce.Add증가량(1);
        }


            
            
            
    }    
}
