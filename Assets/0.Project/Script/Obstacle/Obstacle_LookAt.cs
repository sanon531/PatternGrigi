using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;

namespace PG.Battle
{
    public class Obstacle_LookAt : Obstacle
    {
        private Transform playerTr;
        private Vector2 forward = new Vector2(0, -1);   //sprite�� ���⿡ ���� �޶���

        [SerializeField]
        private float rotateSpeed;

        [SerializeField]
        private float moveSpeed;


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
        void Update()
        {
            CheckStatus();
            LookPlayer();
            Shoot();
        }

        private void LookPlayer()
        {
            if (_isActive) return;

            Vector2 direction = new Vector2(
                     transform.position.x - playerTr.position.x,
                     transform.position.y - playerTr.position.y
                 );

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);
            transform.rotation = rotation;
        }

        private void Shoot()
        {
            if (!_isActive) return;

            transform.Translate(forward * moveSpeed * Time.deltaTime);
        }
    }
}