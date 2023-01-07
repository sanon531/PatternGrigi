using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using UnityEngine.Serialization;

namespace PG.Battle
{
    //투사체는 trigger형태로 하며 몹은 반대로 트리거가 없는채로 설계한다. 
    //맞으면 비활성화 되고
    //projectile 은 플레이어만 활용하는것으로 한다.
    public abstract class ProjectileScript : MonoBehaviour
    {
        [FormerlySerializedAs("_id")] [SerializeField]
        public ProjectileID id = ProjectileID.NormalBullet;

        protected float Damage = 10f;

        [SerializeField]
        protected float CurrentlifeTime = 10f;
        protected float lifeTime = 10f;

        protected Vector3 Movement;

        [SerializeField]
        protected Collider2D collider2D;

         [SerializeField]
        protected Rigidbody2D rigidBody2D;

        [FormerlySerializedAs("_projectileImage")] [SerializeField]
        protected SpriteRenderer projectileImage;

        [FormerlySerializedAs("_hitFX")] [SerializeField]
        protected ParticleSystem hitFX;

        [FormerlySerializedAs("_targetMob")] [SerializeField]
        protected MobScript targetMob;

        protected bool IsrigidBody2DNotNull = false;
        protected bool IstargetMobNotNull = false;


        protected IObjectPoolSW<ProjectileScript> ProjectilePool;

        //관통 횟수.
        protected int PierceCount = 0;
        protected List<GameObject> PiercedList = new List<GameObject>();

        public void Start()
        {
            IstargetMobNotNull = targetMob != null;
            IsrigidBody2DNotNull = rigidBody2D != null;
        }


        public virtual void SetInitialProjectileData(IObjectPoolSW<ProjectileScript> objectPool,
            float lifetime)
        {
            IstargetMobNotNull = false;
            ProjectilePool = objectPool;
            lifeTime = lifetime;
            CurrentlifeTime = lifeTime;
        }
        public virtual void SetFrequentProjectileData(MobScript target, float damage, float spreadCount)
        {
            CurrentlifeTime = lifeTime;
            targetMob = target;
            IstargetMobNotNull = targetMob != null;
            Damage = damage;
            transform.position = (Player_Script.GetPlayerPosition() + new Vector3(spreadCount, 0, 0));
        }

        protected virtual void LateUpdate()
        {
            if (CurrentlifeTime <= 0)
                ProjectilePool.SetBack(this);
            else
                CurrentlifeTime -= Time.deltaTime;
        }
        

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy") && !PiercedList.Contains(collision.gameObject))
            {
                // 이곳에서 적에 대한 데미지를 처리하는 코드를 짠다.
                hitFX.Play();
                collision.GetComponent<MobScript>().Damage(Damage);
                if (PierceCount <= 0)
                    ProjectilePool.SetBack(this);
                PierceCount--;
            }
        }
    }
}