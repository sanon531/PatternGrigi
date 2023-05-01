using System;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;

namespace PG
{
    [Serializable]
    public class WaveClass
    {

        [SerializeField]
        private int _최소스폰수 = 0;
        [SerializeField]
        private MobIDSpawnDataDic _적배치 = new MobIDSpawnDataDic();
        [SerializeField]
        private SpawnType _스폰타입;


        public MobIDSpawnDataDic GetSpawnDataDic(){ return _적배치; }
        public int GetMinMobNum() { return _최소스폰수;  }

        public SpawnType GetSpawnType()
        {
            return _스폰타입;
        }

        public enum SpawnType
        {
            Nomal = 0,
            Rush = 1,
            SubBoss = 2,
            MainBoss = 3,
            GameClear = 4,

        }
    }
}
