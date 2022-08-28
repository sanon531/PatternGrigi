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
        protected float _initialSpeed;
        protected float _acceleration;
        protected float _damage;
        protected float _lifeTime;
        protected Vector3 _movement;
        protected Vector3 _direction;
        protected Collider2D _collider2D;
        protected Rigidbody2D _rigidBody2D;

        //���� Ƚ��.
        protected int _pierceCount = 0;
        protected List<GameObject> _piercedList = new List<GameObject>();
        public virtual void SetInitialProjectileData(Vector3 direction, float damage, float speed, float lifetime) 
        {
            _isActive = true;
            _collider2D = GetComponent<Collider2D>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _direction = direction;
            _damage = damage;
            _initialSpeed = speed;
            _lifeTime = lifetime;
        }



        protected virtual void FixedUpdate()
        {
            if (_isActive) 
            {
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

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag =="Enemy" && !_piercedList.Contains(collision.gameObject)) 
            {
            
            
            }
        }

    }

}
