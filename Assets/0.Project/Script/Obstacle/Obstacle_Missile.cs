using System.Collections;
using System.Collections.Generic;
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

        private void Start()
        {
        }
        private void OnDestroy()
        {
        }
        protected override void FixedUpdate()
        {
            CheckStatus();
            MoveObstacle();
        }

        protected override void SetActiveObstacle()
        {
            _flameParticle.Play();
            base.SetActiveObstacle();
            _thisSpriteRd.enabled = true;
            Destroy(Range);
        }


        private void MoveObstacle()
        {
            if (!_isActived) return;

            transform.Translate(moveDirection * moveSpeed);
        }

    }
}