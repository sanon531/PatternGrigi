using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle
{
    public class Obstacle_Fire : Obstacle
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        public override void SetSpawnData(float lifeTime, float activetimes)
        {
            _maxLifetime = lifeTime;
            _lifeTime = _maxLifetime;
            _activetime = activetimes;
            _isPlaced = true;
        }

        void Update()
        {
            CheckStatus();
        }


    }
}