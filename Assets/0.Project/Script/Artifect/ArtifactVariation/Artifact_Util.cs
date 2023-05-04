
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using PG.Battle;
using UnityEngine;
using PG.Event;
using PG.Data;

namespace PG
{
    
    public sealed class Artifact_Red_Flavor : Artifact
    {

        private int nowStack = 0;
        private int maxStack = 0;

        public Artifact_Red_Flavor() : base(ArtifactID.Red_Flavor)
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
            Global_BattleEventSystem._onMobDamaged += HealingProcess;
            _drainDamage = ArfifactLevelValueList[ArtifactLevel - 1];
        }

        private float _drainDamage = 0.01f;
        void HealingProcess(float val)
        {
            Player_Script.Heal(val*_drainDamage);
        }

        protected override void Disable()
        {
            base.Disable();
            Global_BattleEventSystem._onMobDamaged -= HealingProcess;
            _drainDamage = ArfifactLevelValueList[ArtifactLevel - 1];
        }
        
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();
        }
    }
        public sealed class Artifact_OrbitSword : Artifact 
        {

        public Artifact_OrbitSword() : base(ArtifactID.OrbitSword)
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
            BattleExtraAttackManager.StartOrbitSword();
            BattleExtraAttackManager.LevelUpOrbitSword(ArfifactLevelValueList[ArtifactLevel - 1],ArfifactLevelValueList2[ArtifactLevel - 1]);
           
        }

        protected override void Disable()
        {
            base.Disable();
        }
        
        public override void AddCountOnArtifact()
        {
            base.AddCountOnArtifact();
            BattleExtraAttackManager.LevelUpOrbitSword(ArfifactLevelValueList[ArtifactLevel - 1],ArfifactLevelValueList2[ArtifactLevel - 1]);
        }
    }
    
}