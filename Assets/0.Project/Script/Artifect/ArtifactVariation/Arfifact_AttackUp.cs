using PG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PG.Event;

namespace PG
{
    public sealed class Arfifact_AttackUp : Artifact
    {
        public Arfifact_AttackUp(ArtifactID artifactID) : base(artifactID)
        {
        }
    }


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
            //������ Įũ �������� ���� ������
            Global_BattleEventSystem.CallOnAddCalcDamage(10f);
            //Debug.Log("Fragile_Rush LEL");
        }
        protected override void Disable()
        {
            base.Disable();
            Global_BattleEventSystem.CallOnAddCalcDamage(-10f);
        }
        public override void AddCountOnArtifact()
        {
            Global_BattleEventSystem.CallOnAddCalcDamage(10f);
            _value++;
            //Debug.Log("Fragile_Rush LEL");
        }


    }

}