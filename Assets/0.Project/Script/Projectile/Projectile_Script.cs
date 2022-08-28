using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PG.Battle 
{
    //투사체는 trigger형태로 하며 몹은 반대로 트리거가 없는채로 설계한다. 
    //맞으면 비활성화 되고
    //projectile 은 플레이어만 활용하는것으로 한다.
    public class Projectile_Script : PoolableObject
    {
        [SerializeField]
        protected float _initialSpeed = 1;
        protected float _acceleration = 0 ;
        protected float _damage = 10f;
        protected float _lifeTime = 10f;
        protected Vector3 _movement;
        [SerializeField]
        protected Vector3 _direction;
        [SerializeField]
        protected Collider2D _collider2D;
        [SerializeField]
        protected Rigidbody2D _rigidBody2D;
        [SerializeField]
        protected SpriteRenderer _projectileImage;
        [SerializeField]
        ParticleSystem _ongoingFX;
        [SerializeField]
        ParticleSystem _hitFX;



        //관통 횟수.
        protected int _pierceCount = 0;
        protected List<GameObject> _piercedList = new List<GameObject>();
        public virtual void SetInitialProjectileData(Vector3 direction, float damage, float speed, float lifetime) 
        {
            OnObjectEnabled();
            _direction = direction;
            _damage = damage;
            _initialSpeed = speed;
            _lifeTime = lifetime;

        }



        protected virtual void FixedUpdate()
        {
            if (_isActive) 
            {
                Movement();
            }

        }

        public void Movement() 
        {
            if (_lifeTime > 0)
            {
                _lifeTime -= Time.deltaTime;
                _movement = _direction * (_initialSpeed / 10) * Time.deltaTime;
                if (_rigidBody2D != null)
                {
                    _rigidBody2D.MovePosition(this.transform.position + _movement);
                }
                _initialSpeed += _acceleration * Time.deltaTime;
            }
        }

        protected override void OnObjectEnabled()
        {
            base.OnObjectEnabled();
            _projectileImage.enabled = true;
            _ongoingFX.Play();
        }

        protected override void OnObjectDisabled()
        {
            base.OnObjectDisabled();
            _projectileImage.enabled = false;
            _ongoingFX.Stop();

        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag =="Enemy" && !_piercedList.Contains(collision.gameObject)) 
            {
                // 이곳에서 적에 대한 데미지를 처리하는 코드를 짠다.
                _hitFX.Play();
                if (_pierceCount <=0)
                    OnObjectDisabled();
                _pierceCount--;
            }
        }

    }

}
