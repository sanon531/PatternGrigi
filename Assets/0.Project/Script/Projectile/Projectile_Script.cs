using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PG.Battle 
{
    //����ü�� trigger���·� �ϸ� ���� �ݴ�� Ʈ���Ű� ����ä�� �����Ѵ�. 
    //������ ��Ȱ��ȭ �ǰ�
    //projectile �� �÷��̾ Ȱ���ϴ°����� �Ѵ�.
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



        //���� Ƚ��.
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
                // �̰����� ���� ���� �������� ó���ϴ� �ڵ带 §��.
                _hitFX.Play();
                if (_pierceCount <=0)
                    OnObjectDisabled();
                _pierceCount--;
            }
        }

    }

}
