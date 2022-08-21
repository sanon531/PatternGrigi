using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
namespace PG.Battle
{
    public struct Data_CampaignOption
    {
        public Data_CampaignOption(int advDifficulty, CharacterID characterID, ArtifactID? artifactID, bool masterKeyUsed)
        {
            this.advDifficulty = advDifficulty;   //시련모드가 아니면 상관없는 값
            character = characterID;
            startArtifact = artifactID;
            this.masterKeyUsed = masterKeyUsed;
        }
        public int advDifficulty { get; }
        public CharacterID character { get; }
        public ArtifactID? startArtifact { get; }
        public bool masterKeyUsed { get; }
    }
    [System.Serializable]
    public static class Global_CampaignData
    {
        public static Dictionary<ArtifactID, Artifact> _currentArtifactDictionary = 
            new Dictionary<ArtifactID, Artifact>();

        //public static Dictionary<ArtifactID, ArtifactData> _currentActivateDictionary =
            //new Dictionary<ArtifactID, ArtifactData>();
        public static Enemy_Script _currentEnemy;

    }




}
