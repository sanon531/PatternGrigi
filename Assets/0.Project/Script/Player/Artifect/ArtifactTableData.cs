using UnityEngine;
using System.Collections;


namespace PG.Data
{
    [System.Serializable]
    public class ArtifactTableData
    {


        [SerializeField]
        string key;
        public string Key { get { return key; } set { this.key = value; } }

        [SerializeField]
        string devcomment;
        public string Devcomment { get { return devcomment; } set { this.devcomment = value; } }

        [SerializeField]
        string devcomment2;
        public string Devcomment2 { get { return devcomment2; } set { this.devcomment2 = value; } }


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

        public ArtifactTableData(ArtifactID id, string devcomment, string devcomment2, int rarity, bool isonoff, int value)
        {
            this.key = id.ToString();
            this.devcomment = devcomment;
            this.devcomment2 = devcomment2;
            this.rarity = rarity;
            this.isonoff = isonoff;
            this.value = value;
        }

        public bool Isonoff { get { return isonoff; } set { this.isonoff = value; } }

    }
}
