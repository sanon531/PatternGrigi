using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using PG.Data;

namespace PG.Battle
{
    //투사체는 trigger형태로 하며 몹은 반대로 트리거가 없는채로 설계한다. 
    //맞으면 비활성화 되고
    //projectile 은 플레이어만 활용하는것으로 한다.
    public abstract class ProjectileScript : MonoBehaviour
    {
        #region variables
        [SerializeField]
        public ProjectileID id = ProjectileID.NormalBullet;

        protected float Damage = 10f;

        [SerializeField]
        protected float CurrentLifeTime = 10f;
        protected float MaxLifeTime = 10f;

        protected Vector3 velocity;

        [SerializeField]
        protected Collider2D collider2D;

         [SerializeField]
        protected Rigidbody2D rigidBody2D;

        [SerializeField]
        protected SpriteRenderer projectileImage;

        
        //���� Ƚ��.
        protected int PierceCount = 0;
        protected List<GameObject> PiercedList = new List<GameObject>();
        #endregion

        [SerializeField]
        protected MobScript targetMob;

        protected bool IsrigidBody2DNotNull = false;
        protected bool IstargetMobNotNull = false;

        protected IObjectPoolSW<ProjectileScript> ProjectilePool;

        public virtual void SetInitialProjectileData(IObjectPoolSW<ProjectileScript> objectPool,
            float lifetime)
        {
            IsrigidBody2DNotNull = rigidBody2D != null;
            IstargetMobNotNull = targetMob != null;
            ProjectilePool = objectPool;
            MaxLifeTime = lifetime;
            CurrentLifeTime = MaxLifeTime;
        }
        

        protected bool IsActive = true;
        public virtual void SetFrequentProjectileData([CanBeNull] MobScript target, float damage,Vector3 projectilePlace)
        {
            IsActive = true;
            PierceCount = 0;
            CurrentLifeTime = MaxLifeTime;
            IstargetMobNotNull = targetMob is null;
            targetMob = target;
            Damage = damage;
            PierceCount = (int)Global_CampaignData._projectilePierce.FinalValue;
            transform.position = (Player_Script.GetPlayerPosition() + projectilePlace);
        }

        protected virtual void LateUpdate()
        {
            if (CurrentLifeTime <= 0 || !IsActive )
                ProjectilePool.SetBack(this);
            else
                CurrentLifeTime -= Time.deltaTime;
        }
        
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsActive)
                return;
            if (collision.CompareTag("Enemy") && !PiercedList.Contains(collision.gameObject))
            {
                collision.GetComponent<MobScript>().Damage(Damage);
                PiercedList.Add(gameObject);
                PierceCount--;
                if (PierceCount <= 0)
                    IsActive=false;
            }
            if (collision.CompareTag($"Boundary"))
            {
                IsActive=false;
            }
        }
    }
}