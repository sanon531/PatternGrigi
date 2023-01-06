using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PG.Event;
using PG.Data;
namespace PG
{
    public sealed class Arfifact_PadThai : Artifact
    {
        public Arfifact_PadThai() : base(ArtifactID.PadThai)
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
            Global_CampaignData._playerSize.Add증가량(0.5f);
            //Debug.Log("Fragile_Rush LEL");
            Global_BattleEventSystem.CallOnSizeChanged();
        }



        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(-10f);
            Global_CampaignData._playerSize.Add증가량(-0.5f);
            Global_BattleEventSystem.CallOnSizeChanged();

        }
        public override void AddCountOnArtifact()
        {
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(10f);
            Global_CampaignData._playerSize.Add증가량(0.5f);
            Global_BattleEventSystem.CallOnSizeChanged();

            _upgradeCount++;
            //Debug.Log("Fragile_Rush LEL");
        }


    }


    public sealed class Arfifact_SesameOil : Artifact
    {
        public Arfifact_SesameOil() : base(ArtifactID.SesameOil)
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
            Global_CampaignData._playerSize.Add증가량(-0.5f);
            Global_BattleEventSystem.CallOnSizeChanged();
            //Debug.Log("Fragile_Rush LEL");
        }
        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(5f);
            Global_CampaignData._playerSize.Add증가량(0.5f);
            Global_BattleEventSystem.CallOnSizeChanged();
        }
        public override void AddCountOnArtifact()
        {
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(5f);
            Global_CampaignData._playerSize.Add증가량(-0.5f);
            _upgradeCount++;
            Global_BattleEventSystem.CallOnSizeChanged();
            //Debug.Log("Fragile_Rush LEL");
        }


    }
}
