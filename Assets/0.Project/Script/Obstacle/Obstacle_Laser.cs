using System.Collections;
using System.Collections.Generic;
using PG.Data;
using UnityEngine;
using PG.Event;

namespace PG.Battle
{
    public class Obstacle_Laser : Obstacle
    {

        [SerializeField] private SpriteRenderer _showSign;
        [SerializeField] private ParticleSystem _attackParticle;
        


        public override void SetSpawnData(float lifeTime, float activetimes, float damage, ObstacleID id)
        {
            base.SetSpawnData(lifeTime, activetimes, damage, id);
            _thisCollider.enabled = false;
            _showSign.enabled = true;
        }

        protected override void SetActiveObstacle()
        {
            base.SetActiveObstacle();
            _showSign.enabled = false;
            _thisCollider.enabled = true;
            _attackParticle.Play();
        }

    }
}