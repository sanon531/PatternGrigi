using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;

namespace PG.Battle 
{
    //투사체는 trigger형태로 하며 몹은 반대로 트리거가 없는채로 설계한다. 
    //맞으면 비활성화 되고
    //projectile 은 플레이어만 활용하는것으로 한다.
    public class Projectile_Script : PoolableObject
    {
        [SerializeField]
        protected ProjectileID _id = ProjectileID.NormalBullet;

        protected float _damage = 10f;
        [SerializeField]
        protected float _lifeTime = 10f;
        protected Vector3 _movement;
        [SerializeField]
        protected Collider2D _collider2D;
        [SerializeField]
        protected Rigidbody2D _rigidBody2D;
        [SerializeField]
        protected SpriteRenderer _projectileImage;
        [SerializeField]
        protected ParticleSystem _ongoingFX;
        [SerializeField]
        protected ParticleSystem _hitFX;
        [SerializeField]
        protected MobScript _targetMob;


        //관통 횟수.
        protected int _pierceCount = 0;
        protected List<GameObject> _piercedList = new List<GameObject>();


        public virtual void SetInitialProjectileData(MobScript _target, float damage, float lifetime,float spreadCount)
        {
            if (_target == null)
                _targetMob = null;
            else
                _targetMob = _target;


            transform.position = (Player_Script.GetPlayerPosition() + new Vector3(spreadCount,0,0));

            _damage = damage;
            _lifeTime = lifetime;
        }

        protected virtual void LateUpdate()
        {
            if (_isPlaced)
            {
                if (_lifeTime <= 0)
                    OnObjectDisabled();
                else
                    _lifeTime -= Time.deltaTime;
            }
        }



        protected override void OnObjectEnabled()
        {
            base.OnObjectEnabled();
        }

        protected override void OnObjectDisabled()
        {
            base.OnObjectDisabled();
            _ongoingFX.Stop();
            _hitFX.Stop();
            _rigidBody2D.velocity = new Vector3();
            ProjectileManager.SetBackProjectile(gameObject,_id);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag =="Enemy" && !_piercedList.Contains(collision.gameObject)) 
            {
                // 이곳에서 적에 대한 데미지를 처리하는 코드를 짠다.
                _hitFX.Play();
                collision.GetComponent<MobScript>().Damage(_damage);
                if (_pierceCount <=0)
                    OnObjectDisabled();
                _pierceCount--;
            }
        }

    }

}
