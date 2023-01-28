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


        [SerializeField]
        int buyprice = 100;
        public int Buyprice { get { return buyprice; } set { this.buyprice = value; } }
        [SerializeField]
        int maxUpgrade;
        public int MaxUpgrade { get { return maxUpgrade; } set { this.maxUpgrade = value; } }
        [SerializeField]
        int upgradecount;
        public int UpgradeCount { get { return upgradecount; } set
            {
                if (value <= maxUpgrade)
                    this.upgradecount = value;
                else
                    this.upgradecount = maxUpgrade;
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
            this.maxUpgrade = 5;
        }


        public ArtifactData(ArtifactID id, int rarity, bool isonoff, int value)
        {
            this.id = id;
            this.rarity = rarity;
            this.isonoff = isonoff;
            this.upgradecount = value;
        }

        public ArtifactData(ArtifactID id, int rarity, bool isonoff, int value, int maxUpgrade)
        {
            this.id = id;
            this.rarity = rarity;
            this.isonoff = isonoff;
            this.upgradecount = value;
            this.maxUpgrade = maxUpgrade;
        }




    }
}
