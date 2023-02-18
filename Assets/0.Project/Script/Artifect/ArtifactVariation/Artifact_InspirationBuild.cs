using System.Collections;
using System.Collections.Generic;
using PG.Battle;
using UnityEngine;
using PG.Event;
using PG.Data;

namespace PG
{
    public sealed class Artifact_Spread_Inspiration : Artifact
    {
        public Artifact_Spread_Inspiration() : base(ArtifactID.Spread_Inspiration)
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
            BattleExtraAttackManager.EnableInspiration();
        }
        
        protected override void Disable()
        {
            base.Disable();
            BattleExtraAttackManager.DisableInspiration();
        }
        
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();
            
        }
    }
    
    public sealed class Artifact_SenseOfCreativity : Artifact
    {
        public Artifact_SenseOfCreativity() : base(ArtifactID.SenseOfCreativity)
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
            Global_CampaignData._randomPatternNodeCount.Add증가량(1);
        }

        protected override void Disable()
        {
            base.Disable();
        }
        
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();
            Global_CampaignData._randomPatternNodeCount.Add증가량(1);
        }
    }
    
    public sealed class Artifact_AncestralBrushstroke : Artifact
    {
        public Artifact_AncestralBrushstroke() : base(ArtifactID.AncestralBrushstroke)
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
        }
        
        protected override void Disable()
        {
            base.Disable();
            
        }
        
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();
            
        }
    }
    
    
}
