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
            base.OnGetArtifact();
            Enable();
        }
        protected override void Enable()
        {
            base.Enable();
            Global_CampaignData._currentChargePattern = DrawPatternPresetID.Thunder_Manimekhala;
            Global_BattleEventSystem._onPatternSuccessed += CallRandomPatternSuccessed;
            //Debug.Log("asdf");
        }
        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._currentChargePattern = DrawPatternPresetID.Empty_Breath;
            Global_BattleEventSystem._onPatternSuccessed -= CallRandomPatternSuccessed;
        }
        public override void AddCountOnArtifact()
        {
            _value++;
        }

        void CallRandomPatternSuccessed(DrawPatternPresetID patternPreset) 
        {
            if (patternPreset == DrawPatternPresetID.Empty_Breath) 
            {
                Global_BattleEventSystem.CallOnCalcPlayerAttack(10f);
                Global_BattleEventSystem.CallOnCalcPlayerAttack(10f);
                Global_BattleEventSystem.CallOnCalcPlayerAttack(10f);
                Global_BattleEventSystem.CallOnCalcPlayerAttack(10f);
            }
        }

    }

    public sealed class Artifact_LoveAndPeace : Artifact
    {
        public Artifact_LoveAndPeace() : base(ArtifactID.LoveAndPeace)
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
            Global_CampaignData._currentChargePattern = DrawPatternPresetID.LoveAndPeace;
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