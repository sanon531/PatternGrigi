using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace PG.Data
{
    [System.Serializable]
    public class ArtifactData
    {

        [SerializeField]
        string key;
        public string Key { get { return key; } set { this.key = value; } }





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
            this.key = id.ToString();
            this.rarity = (int)ArtifactRarity.Common;
            this.isonoff = true;
            this.upgradecount = 0;
            this.maxUpgrade = 5;
        }


        public ArtifactData(ArtifactID id, int rarity, bool isonoff, int value)
        {
            this.key = id.ToString();
            this.rarity = rarity;
            this.isonoff = isonoff;
            this.upgradecount = value;
        }

        public ArtifactData(ArtifactID id, int rarity, bool isonoff, int value, int maxUpgrade)
        {
            this.key = id.ToString();
            this.rarity = rarity;
            this.isonoff = isonoff;
            this.upgradecount = value;
            this.maxUpgrade = maxUpgrade;
        }




    }
}
