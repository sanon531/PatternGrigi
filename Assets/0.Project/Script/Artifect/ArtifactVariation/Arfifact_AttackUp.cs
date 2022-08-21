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
    }


    // 해당 아티펙트는 적과 플레이어의 공격력을 둘다 올려주는 거
    public sealed class Arfifact_FragileRush : Artifact
    {
        public Arfifact_FragileRush() : base(ArtifactID.FragileRush)
        {
        }

        public override void OnGetArtifact()
        {
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
                if(Global_CampaignData._charactorAttackDic.ElementAt(i).Key == CharacterID.Player)
                    Global_CampaignData._charactorAttackDic.ElementAt(i).Value.Add증가량(10f);
            }


        }
        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(-10f);
        }
        public override void AddCountOnArtifact()
        {
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(10f);
            _value++;
            for (int i = 0; i < Global_CampaignData._charactorAttackDic.Count; i++)
            {
                if (Global_CampaignData._charactorAttackDic.ElementAt(i).Key == CharacterID.Player)
                    Global_CampaignData._charactorAttackDic.ElementAt(i).Value.Add증가량(10f);
            }
            //Debug.Log("Fragile_Rush LEL");
        }


    }

}