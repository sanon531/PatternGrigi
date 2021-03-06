using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG.Battle
{
    public class Obstacle : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {

        }

        
        [SerializeField]
        protected float _passedTime = 0f;
        [SerializeField]
        protected float _maxLifetime, _lifeTime, _activetime;
        [SerializeField]
        protected bool _isPlaced = false;
        protected bool _isActive = false;
        [SerializeField]
        protected float _damageDeal = 8f;
        [SerializeField]
        protected Collider2D _thisCollider;

        public virtual void SetSpawnData(float lifeTime, float activetimes)
        {
            _maxLifetime = lifeTime;
            _lifeTime = _maxLifetime;
            _activetime = activetimes;
            _isPlaced = true;
        }
        protected virtual void SetActiveObstacle() 
        {
            _thisCollider.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            CheckStatus();
        }
        protected virtual void CheckStatus() 
        {
            if (!_isPlaced)
                return;

            _passedTime += Time.deltaTime;
            if (_passedTime > _activetime && !_isActive)
            {
                SetActiveObstacle();
                _isActive = true;
            }
            else if (_passedTime > _lifeTime)
            {
                ObstacleManager.DeleteObstacleOnList(this);
                Destroy(gameObject);
            }



        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                Player_Script.Damage(_damageDeal);
        }

    }
}