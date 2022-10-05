using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
using PG.Data;
namespace PG
{

    public class Artifact_UpgradeNormalShot : Artifact
    {
        protected Artifact_UpgradeNormalShot() : base(ArtifactID.Upgrade_NormalShot)
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
            //������ Įũ �������� ���� ������
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add������(10f);
            Global_CampaignData._playerSize.Add������(0.5f);
            //Debug.Log("Fragile_Rush LEL");
            Global_BattleEventSystem.CallOnSizeChanged();
        }
        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add������(-10f);
            Global_CampaignData._playerSize.Add������(-0.5f);
            Global_BattleEventSystem.CallOnSizeChanged();

        }
        public override void AddCountOnArtifact()
        {
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add������(10f);
            Global_CampaignData._playerSize.Add������(0.5f);
            Global_BattleEventSystem.CallOnSizeChanged();

            _value++;
            //Debug.Log("Fragile_Rush LEL");
        }

    }

    public class Artifact_UpgradeStraightShot : Artifact
    {
        protected Artifact_UpgradeStraightShot() : base(ArtifactID.Upgrade_StraightShot)
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
            //������ Įũ �������� ���� ������
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add������(10f);
            Global_CampaignData._playerSize.Add������(0.5f);
            //Debug.Log("Fragile_Rush LEL");
            Global_BattleEventSystem.CallOnSizeChanged();
        }
        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add������(-10f);
            Global_CampaignData._playerSize.Add������(-0.5f);
            Global_BattleEventSystem.CallOnSizeChanged();

        }
        public override void AddCountOnArtifact()
        {
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add������(10f);
            Global_CampaignData._playerSize.Add������(0.5f);
            Global_BattleEventSystem.CallOnSizeChanged();

            _value++;
            //Debug.Log("Fragile_Rush LEL");
        }

    }
    public class Artifact_UpgradeLightningShot : Artifact
    {
        protected Artifact_UpgradeLightningShot() : base(ArtifactID.Upgrade_LightningShot)
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
            //������ Įũ �������� ���� ������
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add������(10f);
            Global_CampaignData._playerSize.Add������(0.5f);
            //Debug.Log("Fragile_Rush LEL");
            Global_BattleEventSystem.CallOnSizeChanged();
        }
        protected override void Disable()
        {
            base.Disable();
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add������(-10f);
            Global_CampaignData._playerSize.Add������(-0.5f);
            Global_BattleEventSystem.CallOnSizeChanged();

        }
        public override void AddCountOnArtifact()
        {
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add������(10f);
            Global_CampaignData._playerSize.Add������(0.5f);
            Global_BattleEventSystem.CallOnSizeChanged();

            _value++;
            //Debug.Log("Fragile_Rush LEL");
        }

    }


}