using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
using PG.Data;
namespace PG
{

    public sealed class Artifact_UpgradeAimShot : Artifact
    {
        public Artifact_UpgradeAimShot() : base(ArtifactID.Upgrade_AimShot)
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
            AddCountOnArtifact();
        }
        protected override void Disable()
        {
            base.Disable();
        }
        public override void AddCountOnArtifact()
        {

            switch (ArtifactLevel) 
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    ArtifactLevel = MaxLevel;
                    break;
            }



            ArtifactLevel++;
        }

    }

    public sealed class Artifact_UpgradeStraightShot : Artifact
    {
        public Artifact_UpgradeStraightShot() : base(ArtifactID.Upgrade_StraightShot)
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
            ArtifactLevel++;
        }
        protected override void Disable()
        {
            base.Disable();

        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            //Debug.Log("Fragile_Rush LEL");
        }

    }
    public sealed class Artifact_UpgradeLightningShot : Artifact
    {
        public Artifact_UpgradeLightningShot() : base(ArtifactID.Upgrade_LightningShot)
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
            ArtifactLevel++;
        }
        protected override void Disable()
        {
            base.Disable();

        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            //Debug.Log("Fragile_Rush LEL");
        }

    }


}