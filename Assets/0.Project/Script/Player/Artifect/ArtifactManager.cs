using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
namespace PG.Battle 
{
    public class ArtifactManager : MonoSingleton<ArtifactManager>
    {

        void Start()
        {
            Global_BattleEventSystem._onBattleBegin += RefreshCurrentArtifact;
        }

        Dictionary<>

        void RefreshCurrentArtifact() 
        {
        
        
        }


    }
}
