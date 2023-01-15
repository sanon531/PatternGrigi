using System.Collections;
using System.Collections.Generic;
using PG.Data;
using UnityEngine;
using PG.Event;

namespace PG.Battle
{
    public class Obstacle_Missile : Obstacle
    {
        [SerializeField]
        private GameObject Range;

        [Header("Move Stats")]
        [SerializeField]
        private Vector2 moveDirection;
        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private ParticleSystem _flameParticle;
        [SerializeField]
        private SpriteRenderer _thisSpriteRd;
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (_isActived)
            {
                MoveObstacle();
            }
        }

        public override void SetSpawnData(float lifeTime, float activetimes,float damage, ObstacleID id)
        {
            base.SetSpawnData(lifeTime, activetimes, damage, id);
            _flameParticle.Stop();
            _thisSpriteRd.enabled = false;
            Range.SetActive(true);
        }
        
        protected override void SetActiveObstacle()
        {
            _flameParticle.Play();
            base.SetActiveObstacle();
            _thisSpriteRd.enabled = true;
            Range.SetActive(false);
        }


        private void MoveObstacle()
        {
            if (!_isActived) return;

            transform.Translate(moveDirection * moveSpeed);
        }

    }
}