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
        private float _�ּҽ����� = 0;
        [SerializeField]
        private MobIDSpawnDataDic _����ġ = new MobIDSpawnDataDic();
        [SerializeField]
        private SpawnType _����Ÿ��;


        public MobIDSpawnDataDic GetSpawnDataDic()
        {
            return _����ġ;
        }


        public enum SpawnType
        {
            Nomal = 0,
            Rush = 1,
            SubBoss = 2,
            MainBoss = 3,

        }
    }
}
