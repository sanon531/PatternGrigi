using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
using PG.Data;

namespace PG 
{
    public sealed class Artifact_Pinnochio : Artifact 
    {
        public Artifact_Pinnochio() : base(ArtifactID.Pinocchio)
        {

        }
        public override void OnGetArtifact()
        {
            base.OnGetArtifact();
            Enable();
        }
        float[] _addWeight = new float[3] {-0.5f,0,1 };
        float[] _deleteweight = new float[3] { 0.5f, 0, -1 };

        protected override void Enable()
        {
            base.Enable();
            Global_BattleEventSystem.CallOnNodeSetWeight(_addWeight);
        }

        protected override void Disable()
        {
            base.Disable();
            Global_BattleEventSystem.CallOnNodeSetWeight(_deleteweight);
        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            Global_BattleEventSystem.CallOnNodeSetWeight(_addWeight);

        }

    }
    public sealed class Artifact_AtomSetting : Artifact
    {
        public Artifact_AtomSetting() : base(ArtifactID.AtomSetting)
        {

        }
        public override void OnGetArtifact()
        {
            base.OnGetArtifact();
            Enable();
        }
        float[] _addWeight = new float[3] { -0.5f, 1, 0 };
        float[] _deleteweight = new float[3] { 0.5f, 1, 0 };

        protected override void Enable()
        {
            base.Enable();
            Global_BattleEventSystem.CallOnNodeSetWeight(_addWeight);
        }
        protected override void Disable()
        {
            base.Disable();
            Global_BattleEventSystem.CallOnNodeSetWeight(_deleteweight);
        }
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            Global_BattleEventSystem.CallOnNodeSetWeight(_addWeight);

        }

    }


}