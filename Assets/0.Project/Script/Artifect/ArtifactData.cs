using UnityEngine;
using System.Collections;


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
        int value;
        public int Value { get { return value; } set { this.value = value; } }



        [SerializeField]
        bool isonoff;
        public bool Isonoff { get { return isonoff; } set { this.isonoff = value; } }

        public ArtifactData(ArtifactID id, int rarity, bool isonoff, int value)
        {
            this.key = id.ToString();
            this.rarity = rarity;
            this.isonoff = isonoff;
            this.value = value;
        }



    }
}
