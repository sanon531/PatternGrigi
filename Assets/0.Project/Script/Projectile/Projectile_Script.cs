using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;

namespace PG.Battle 
{
    //����ü�� trigger���·� �ϸ� ���� �ݴ�� Ʈ���Ű� ����ä�� �����Ѵ�. 
    //������ ��Ȱ��ȭ �ǰ�
    //projectile �� �÷��̾ Ȱ���ϴ°����� �Ѵ�.
    public class Projectile_Script : PoolableObject
    {
        [SerializeField]
        public ProjectileID _id = ProjectileID.NormalBullet;

        protected float _damage = 10f;
        protected float _lifeTime = 10f;
        protected Vector3 _movement;
        [SerializeField]
        protected Collider2D _collider2D;
        [SerializeField]
        protected Rigidbody2D _rigidBody2D;
        [SerializeField]
        protected SpriteRenderer _projectileImage;
        [SerializeField]
        ParticleSystem _ongoingFX;
        [SerializeField]
        TrailRenderer _ongoingTrail;
        [SerializeField]
        ParticleSystem _hitFX;
        [SerializeField]
        protected MobScript _targetMob;


        //���� Ƚ��.
        protected int _pierceCount = 0;
        protected List<GameObject> _piercedList = new List<GameObject>();


        public virtual void SetInitialProjectileData(MobScript _target, float damage, float lifetime) 
        {
            OnObjectEnabled();
            transform.position = Player_Script.GetPlayerPosition();
            _targetMob = _target;
            _damage = damage;
            _lifeTime = lifetime;

        }





        protected override void OnObjectEnabled()
        {
            base.OnObjectEnabled();
            _projectileImage.enabled = true;
            _ongoingTrail.enabled = true;
            _ongoingFX.Play();
        }

        protected override void OnObjectDisabled()
        {
            base.OnObjectDisabled();
            _projectileImage.enabled = false;
            _ongoingTrail.enabled = false;
            _ongoingFX.Stop();
            ProjectileManager.SetBackProjectile(gameObject,_id);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag =="Enemy" && !_piercedList.Contains(collision.gameObject)) 
            {
                // �̰����� ���� ���� �������� ó���ϴ� �ڵ带 §��.
                _hitFX.Play();
                collision.GetComponent<MobScript>().Damage(_damage);
                if (_pierceCount <=0)
                    OnObjectDisabled();
                _pierceCount--;
            }
        }

    }

}
