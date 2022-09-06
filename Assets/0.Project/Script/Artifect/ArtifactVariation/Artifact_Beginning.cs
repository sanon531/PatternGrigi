using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using PG.Event;

namespace PG
{

    public sealed class Artifact_Thunder_Manimekhala : Artifact
    {
        public Artifact_Thunder_Manimekhala() : base(ArtifactID.Thunder_Manimekhala)
        {
        }
        public override void OnGetArtifact()
        {
            Enable();
        }
        protected override void Enable()
        {
            base.Enable();
            Global_CampaignData._currentChargePattern = DrawPatternPresetID.Thunder_Manimekhala;
            Debug.Log("asdf");
        }
        protected override void Disable()
        {
            base.Disable();
        }
        public override void AddCountOnArtifact()
        {
            _value++;
        }
    }

    public sealed class Artifact_LoveAndPeace : Artifact
    {
        public Artifact_LoveAndPeace() : base(ArtifactID.LoveAndPeace)
        {
        }
        public override void OnGetArtifact()
        {
            Enable();
        }
        protected override void Enable()
        {
            base.Enable();
            Global_CampaignData._currentChargePattern = DrawPatternPresetID.LoveAndPeace;
            Debug.Log("asdf");
        }
        protected override void Disable()
        {
            base.Disable();
        }
        public override void AddCountOnArtifact()
        {
            _value++;
        }
    }

}