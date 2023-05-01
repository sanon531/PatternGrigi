using System.Collections;
using System.Collections.Generic;
using PG.Battle;
using UnityEngine;
using PG.Event;
using PG.Data;

namespace PG
{
    public class Artifact_Default_HealthUp : Artifact
    {
        //무한으로 업그레이드 되도록 디자인함.
        public override int ArtifactLevel
        {
            get => _artifactLevel;
            set => _artifactLevel = value;
        }

        
        public Artifact_Default_HealthUp() : base(ArtifactID.Default_HealthUp)
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
            Player_Script.Damage(-10);
        }



        protected override void Disable()
        {
            base.Disable();

        }
        public override void AddCountOnArtifact()
        {
            Player_Script.Damage(-10);
            base.AddCountOnArtifact();   
            //Debug.Log("Fragile_Rush LEL");
        }
        
    }
    
    
    public class Artifact_Example_A : Artifact
    {
        public Artifact_Example_A() : base(ArtifactID.Example_A)
        {
        }
        
        public override void OnGetArtifact()
        {
            base.OnGetArtifact();
            BattleExtraAttackManager.PlaceBlockade();
            Enable();
        }

        protected override void Enable()
        {
            base.Enable();
        }

        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            BattleExtraAttackManager.PlaceBlockade();
            //Debug.Log("LEL"+ maxLevel+" s " + _artifactLevel);
        }
    }

    public class Artifact_Barricade : Artifact
    {
        public Artifact_Barricade() : base(ArtifactID.Barricade)
        {
        }
        
        public override void OnGetArtifact()
        {
            base.OnGetArtifact();
            BattleExtraAttackManager.PlaceBlockade();
            Enable();
        }

        protected override void Enable()
        {
            base.Enable();
        }

        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();   
            BattleExtraAttackManager.PlaceBlockade();
            //Debug.Log("LEL"+ maxLevel+" s " + _artifactLevel);
        }
    }


}