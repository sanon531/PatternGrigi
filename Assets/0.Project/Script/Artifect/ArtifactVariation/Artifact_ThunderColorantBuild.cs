
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using PG.Battle;
using UnityEngine;
using PG.Event;
using PG.Data;
namespace PG
{
    
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
            Global_CampaignData._thunderCount.BaseValue = (int)ArfifactLevelValueList[ArtifactLevel - 1];
            BattleExtraAttackManager.SetThunderAttackTerm(ArfifactLevelValueList2[ArtifactLevel - 1]);
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
            //Global_CampaignData._thunderCount.Add증가량(1);
            Global_CampaignData._thunderCount.BaseValue = (int)ArfifactLevelValueList[ArtifactLevel - 1];
            BattleExtraAttackManager.SetThunderAttackTerm(ArfifactLevelValueList2[ArtifactLevel - 1]);
        }
    }
    public sealed class Artifact_Decalcomanie : Artifact
    {
        public Artifact_Decalcomanie() : base(ArtifactID.Decalcomanie)
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
            Global_CampaignData._projectileIDDataDic[ProjectileID.NormalBullet]._count += (int)ArfifactLevelValueList[ArtifactLevel - 1];
        }



        protected override void Disable()
        {
            base.Disable();
        }

        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();
            //Global_CampaignData._projectileIDDataDic[ProjectileID.NormalBullet]._count++;
            Global_CampaignData._projectileIDDataDic[ProjectileID.NormalBullet]._count += (int)ArfifactLevelValueList[ArtifactLevel - 1];
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
            Global_CampaignData._projectilePierce.Add증가량((int)ArfifactLevelValueList[ArtifactLevel - 1]);
        }



        protected override void Disable()
        {
            base.Disable();
        }

        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();
            Global_CampaignData._projectilePierce.Add증가량((int)ArfifactLevelValueList[ArtifactLevel - 1]);
        }
    }    
    public sealed class Artifact_Cubism  : Artifact
    {
        public Artifact_Cubism() : base(ArtifactID.Cubism)
        {
        }

        public override void OnGetArtifact()
        {
            base.OnGetArtifact();
            Global_CampaignData._isReflectable = true;
            Global_CampaignData._projectilePierce.Add증가량((int)ArfifactLevelValueList[ArtifactLevel - 1]);
            MaxLevel = 1;
            CompleteArtifact();
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

    public sealed class Artifact_Slime_Blob : Artifact
    {
        public Artifact_Slime_Blob() : base(ArtifactID.Slime_Blob)
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
            Global_CampaignData._CurrentBulletDeBuffs.Add(EMobDebuff.Slow);
            Global_CampaignData._slowAmount = (ArfifactLevelValueList[ArtifactLevel - 1]);
            Global_CampaignData._slowTime = (ArfifactLevelValueList2[ArtifactLevel - 1]);

        }

        protected override void Disable()
        {
            base.Disable();
            //Debug.Log("D");
            Global_CampaignData._CurrentBulletDeBuffs.Remove(EMobDebuff.Slow);
        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            Global_CampaignData._slowAmount = ((int)ArfifactLevelValueList[ArtifactLevel - 1]);
            Global_CampaignData._slowTime = ((int)ArfifactLevelValueList2[ArtifactLevel - 1]);
        }
    }

    
    
}