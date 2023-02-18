
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
            Global_CampaignData._projectileSpeed.Mult배수(2);
            Global_CampaignData._projectileTargetNum.Mult배수(2);
        }



        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._projectileSpeed.Mult배수(2);
            Global_CampaignData._projectileTargetNum.Mult배수(2);
        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            Global_CampaignData._projectileSpeed.Mult배수(2);
            Global_CampaignData._projectileTargetNum.Mult배수(2);
        }


    }


}
