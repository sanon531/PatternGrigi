using PG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PG.Event;
using System.Linq;

namespace PG
{
    public sealed class Arfifact_AttackUp : Artifact
    {
        public Arfifact_AttackUp(ArtifactID artifactID) : base(artifactID)
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
            //간단히 칼크 데미지를 전부 실행함
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(5f);
        }
        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(-5f);
        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   

            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(5f);
        }
    }
    public sealed class Artifact_Equatore : Artifact
    {
        public Artifact_Equatore() : base(ArtifactID.Equatore)
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
            //간단히 칼크 데미지를 전부 실행함
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(5f);


        }
        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(-5f);
        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(5f);
        }

    }
    // 해당 아티펙트는 적과 플레이어의 공격력을 둘다 올려주는 거
    public sealed class Artifact_FragileRush : Artifact
    {
        public Artifact_FragileRush() : base(ArtifactID.FragileRush)
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
            //간단히 칼크 데미지를 전부 실행함
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(10f);
            //Debug.Log("Fragile_Rush LEL");
            for (int i = 0; i < Global_CampaignData._charactorAttackDic.Count; i++)
            {
                if(Global_CampaignData._charactorAttackDic.ElementAt(i).Key != CharacterID.Player)
                    Global_CampaignData._charactorAttackDic.ElementAt(i).Value.Add증가량(10f);
            }
        }
        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(-10f);
            for (int i = 0; i < Global_CampaignData._charactorAttackDic.Count; i++)
            {
                if (Global_CampaignData._charactorAttackDic.ElementAt(i).Key != CharacterID.Player)
                    Global_CampaignData._charactorAttackDic.ElementAt(i).Value.Add증가량(-10f);
            }
        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(10f);
            for (int i = 0; i < Global_CampaignData._charactorAttackDic.Count; i++)
            {
                if (Global_CampaignData._charactorAttackDic.ElementAt(i).Key != CharacterID.Player)
                    Global_CampaignData._charactorAttackDic.ElementAt(i).Value.Add증가량(10f);
            }
            //Debug.Log("Fragile_Rush LEL");
        }


    }




    //거리에따라 그 길이의 배율이 증가하나 기본 데미지가 감소함.
    public sealed class Artifact_BubbleGun : Artifact
    {
        public Artifact_BubbleGun() : base(ArtifactID.BubbleGun)
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
            //간단히 칼크 데미지를 전부 실행함
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(-5f);
            Global_CampaignData._lengthMagnData.Add증가량(0.75f);

        }
        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(5f);
            Global_CampaignData._lengthMagnData.Add증가량(-0.75f);

        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(-5f);
            Global_CampaignData._lengthMagnData.Add증가량(0.75f);
        }

    }
    public sealed class Artifact_QuickSlice : Artifact
    {
        public Artifact_QuickSlice() : base(ArtifactID.QuickSlice)
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
            //간단히 칼크 데미지를 전부 실행함
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(5f);
            Global_CampaignData._lengthMagnData.Add증가량(-0.2f);
        }
        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(-5f);
            Global_CampaignData._lengthMagnData.Add증가량(0.2f);

        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(5f);
            Global_CampaignData._lengthMagnData.Add증가량(-0.2f);
        }

    }
}