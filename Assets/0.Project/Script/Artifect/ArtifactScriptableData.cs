using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PG.Data
{    
    [CreateAssetMenu(fileName = "Global_ArtifactScriptableData", menuName = "PG/ArtifactScriptableData")]
    public class ArtifactScriptableData : ScriptableObject
    {
        [SerializeField] public ArtifactIDArtifactDataDic idArtifactDataDic;

        private void Reset()
        {
            foreach (ArtifactID id in Enum.GetValues(typeof(ArtifactID))) 
                {
                    idArtifactDataDic.Add(id,new ArtifactData(id,0,false,0,0));
                }
        }
    }
    
    
}
