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
            Vector2 direction = (Player_Script.GetPlayerPosition() - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
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