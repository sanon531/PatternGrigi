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
            BattleExtraAttackManager.SetEmitDamagePercent(ArfifactLevelValueList[ArtifactLevel-1]);
            BattleExtraAttackManager.SetStackDamagePercent(ArfifactLevelValueList2[ArtifactLevel-1]);
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
            Global_CampaignData._randomPatternNodeCount.Set증가량(ArfifactLevelValueList[ArtifactLevel - 1]);
        }

        protected override void Disable()
        {
            base.Disable();
        }
        
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();
            Global_CampaignData._randomPatternNodeCount.Set증가량(ArfifactLevelValueList[ArtifactLevel - 1]);
        }
    }
    
    public sealed class Artifact_AncestralBrushstroke : Artifact
    {

        private int nowStack = 0;
        private int maxStack = 0;

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
            nowStack = 0;
            maxStack = (int)ArfifactLevelValueList[ArtifactLevel - 1];
            Global_BattleEventSystem._onPatternFilled += OnPassingNode;
            Global_BattleEventSystem._onPatternSuccessed += OnPatternSuccess;
        }
        
        protected override void Disable()
        {
            base.Disable();
            Global_BattleEventSystem._onPatternFilled -= OnPassingNode;
            Global_BattleEventSystem._onPatternSuccessed -= OnPatternSuccess;
        }
        
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();
            maxStack = (int)ArfifactLevelValueList[ArtifactLevel - 1];
        }

        private void OnPassingNode(float fillRate)
        {
            if (nowStack < maxStack)
            {
                nowStack++;
                Global_CampaignData._charactorAttackDic[CharacterID.Player].Add배수(0.1f);
            }
            Debug.Log("현재 배수"+ Global_CampaignData._charactorAttackDic[CharacterID.Player]._배수);
        }
        
        private void OnPatternSuccess(DrawPatternPresetID patternPreset)
        {
            if (nowStack < maxStack)
            {
                nowStack++;
                Global_CampaignData._charactorAttackDic[CharacterID.Player].Add배수(0.1f);
            }
            
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add배수(-0.1f * nowStack);
            nowStack = 0;
        }
        
    }
    
    
}
