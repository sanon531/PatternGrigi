using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;


namespace PG.Data
{
    [System.Serializable]
    public class ArtifactData
    {

        [FormerlySerializedAs("key")] [SerializeField]
        ArtifactID id;
        public ArtifactID ID { get { return id; } set { this.id = value; } }
        
        [SerializeField]
        int rarity;
        public int Rarity { get { return rarity; } set { this.rarity = value; } }

        [FormerlySerializedAs("maxUpgrade")] [SerializeField]
        int maxLevel;
        public int MaxLevel { get { return maxLevel; } set { this.maxLevel = value; } }
        [SerializeField]
        int upgradecount;
        
        [FormerlySerializedAs("upgradeValueList")] [SerializeField]
        List<float> arfifactLevelValueList;
        [FormerlySerializedAs("upgradeValueList")] [SerializeField]
        List<float> arfifactLevelValueList2;

        public List<float>  ArfifactLevelValueList { get { return arfifactLevelValueList; } set { this.arfifactLevelValueList = value; } }
        public List<float>  ArfifactLevelValueList2 { get { return arfifactLevelValueList2; } set { this.arfifactLevelValueList2 = value; } }

        public int ArtifactLevel { get { return upgradecount; } set
            {
                if (value <= maxLevel)
                    this.upgradecount = value;
                else
                    this.upgradecount = maxLevel;
            }
        }



        [SerializeField]
        bool isonoff;
        public bool Isonoff { get { return isonoff; } set { this.isonoff = value; } }



        public ArtifactData(ArtifactID id)
        {
            this.id = id;
            this.rarity = (int)ArtifactRarity.Common;
            this.isonoff = true;
            this.upgradecount = 0;
            this.maxLevel = 5;
        }


        public ArtifactData(ArtifactID id, int rarity, bool isonoff, int value)
        {
            this.id = id;
            this.rarity = rarity;
            this.isonoff = isonoff;
            this.upgradecount = value;
        }

        public ArtifactData(ArtifactID id, int rarity, bool isonoff, int value, int maxLevel)
        {
            this.id = id;
            this.rarity = rarity;
            this.isonoff = isonoff;
            this.upgradecount = value;
            this.maxLevel = maxLevel;
        }




    }
}
