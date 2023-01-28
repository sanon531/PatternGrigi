using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;




namespace PG.Data
{
    [Serializable]
    public class IDMixure
    {
        public ArtifactID Key;

        public List<ArtifactID> Value;
    }

    [CreateAssetMenu(fileName = "ArtifactMixtureData", menuName = "PG/ArtifactMixtureData")]

    public class ArtifactMixtureData : ScriptableObject
    {
        public List<IDMixure> mixDic;

    }
}