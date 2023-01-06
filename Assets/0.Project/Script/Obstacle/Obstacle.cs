using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
namespace PG.Battle
{
    public class Obstacle : MonoBehaviour
    {

        // Start is called before the first frame update
        private void Start()
        {
        }

        private void OnDestroy()
        {
        }

        [SerializeField]
        protected float _passedTime = 0f;
        [SerializeField]
        protected float _maxLifetime, _lifeTime, _activetime;
        [SerializeField]
        protected bool _isActived = false;


        [SerializeField]
        protected float _damageDeal = 8f;
        [SerializeField]
        protected Collider2D _thisCollider;

        public virtual void SetSpawnData(float lifeTime, float activetimes,float damage)
        {
            _maxLifetime = lifeTime;
            _lifeTime = _maxLifetime;
            _activetime = activetimes;
            _damageDeal = damage;
            _isActived = false;
        }
        protected virtual void SetActiveObstacle() 
        {
            _thisCollider.enabled = true;
            _isActived = true;
        }

        // Update is called once per frame
        protected virtual void FixedUpdate()
        {
            CheckStatus();
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
                ObstacleManager.DeleteObstacleOnList(this);
            }
        }

        public void SetLifeTime(float time) 
        {
            _lifeTime = time;
        }

        // 이부분 개선 할꺼임.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                Player_Script.Damage(_damageDeal);
        }

    }
}