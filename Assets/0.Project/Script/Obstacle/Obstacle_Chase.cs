using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;

namespace PG.Battle { 
    public class Obstacle_Chase : Obstacle
    {
        private Transform playerTr;

        [SerializeField]
        private float chaseSpeed;


        private void Start()
        {
            Global_BattleEventSystem._onNonTotalPause += SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause += SetNonTotalPauseOff;
            playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        }
        private void OnDestroy()
        {
            Global_BattleEventSystem._onNonTotalPause -= SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause -= SetNonTotalPauseOff;
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            ChasePlayer();
        }


        private void ChasePlayer()
        {
            if (!_isPlaced) return;

            transform.position = Vector2.MoveTowards(transform.position, playerTr.position, chaseSpeed * Time.deltaTime);
        }

    }
}
