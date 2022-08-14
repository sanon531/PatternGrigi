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
        private SpriteRenderer _thisSpriteRd;


        private void Start()
        {
            Global_BattleEventSystem._onNonTotalPause += SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause += SetNonTotalPauseOff;
        }
        private void OnDestroy()
        {
            Global_BattleEventSystem._onNonTotalPause -= SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause -= SetNonTotalPauseOff;
        }
        void Update()
        {
            CheckStatus();
            MoveObstacle();
        }

        protected override void SetActiveObstacle()
        {
            base.SetActiveObstacle();
            _thisSpriteRd.enabled = true;
            Destroy(Range);
        }


        private void MoveObstacle()
        {
            if (!_isActive) return;

            transform.Translate(moveDirection * moveSpeed);
        }

    }
}