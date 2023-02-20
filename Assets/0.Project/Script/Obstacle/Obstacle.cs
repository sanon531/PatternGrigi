using System;
using System.Collections;
using System.Collections.Generic;
using PG.Data;
using UnityEngine;
using PG.Event;
namespace PG.Battle
{
    public class Obstacle : MonoBehaviour
    {
        private void OnEnable()
        {
            //life start
            _passedTime = 0f;
            _isActived = false;
        }

        [SerializeField]
        protected float _passedTime = 0f;
        [SerializeField]
        protected float _maxLifetime, _lifeTime, _activetime;
        [SerializeField]
        protected bool _isActived = false;

        public ObstacleID _id;

        [SerializeField]
        protected float _damageDeal = 8f;
        [SerializeField]
        protected Collider2D _thisCollider;

        public virtual void SetSpawnData(float lifeTime, float activetimes,float damage, ObstacleID id)
        {
            _maxLifetime = lifeTime;
            _lifeTime = _maxLifetime;
            _activetime = activetimes;
            _damageDeal = damage;

            _isActived = false;
            _id = id;
        }
        protected virtual void SetActiveObstacle() 
        {
            _thisCollider.enabled = true;
            _isActived = true;
        }
        
        protected virtual void FixedUpdate()
        {
            if (enabled)
            {
                CheckStatus();
            }
        }
        protected virtual void CheckStatus() 
        {
            _passedTime += Time.deltaTime;
            if (_passedTime > _activetime && !_isActived)
            {
                SetActiveObstacle();
            }
            else if (_passedTime > _lifeTime)
            {
                Delete();
            }
        }

        public void SetLifeTime(float time) 
        {
            _lifeTime = time;
        }

        protected void Delete()
        {
            ObstacleManager.DeleteObstacleOnList(this);
        }
        
        protected virtual void HitPlayer()
        {
            Player_Script.Damage(_damageDeal);
        }
        
        // 이부분 개선 할꺼임.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                HitPlayer();
        }

    }
}