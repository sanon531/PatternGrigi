using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
using PG.Data;
namespace PG
{
    public sealed class Arfifact_BulletTeleportShooter : Artifact
    {
        public Arfifact_BulletTeleportShooter() : base(ArtifactID.BulletTeleportShooter)
        {
        }

        public override void OnGetArtifact()
        {
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
            _value++;
            Global_CampaignData._projectileSpeed.Add배수(2);
            Global_CampaignData._projectileTargetNum.Add배수(2);
        }


    }


}
