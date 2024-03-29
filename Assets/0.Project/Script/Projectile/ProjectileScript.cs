using System;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] protected Color originalColor;
        
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
            IsrigidBody2DNotNull = rigidBody2D is not null;
            IstargetMobNotNull = targetMob  is not null;
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
            transform.position = (Player_Script.GetPlayerPosition() );
            projectileImage.color = GetCurrentColor();
        }

        private Color GetCurrentColor()
        {
            if (Global_CampaignData._CurrentBulletDeBuffs.Count==0)
                return originalColor;
            
            var eMobDebuff = Global_CampaignData._CurrentBulletDeBuffs.Min();
            switch (eMobDebuff)
            {
                case EMobDebuff.Slow:
                    return Color.green;
                default:
                    return originalColor;
            }
            
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
            if (collision.CompareTag("Enemy"))
            {
                var mob = collision.GetComponent<MobScript>();
                mob.Damage(transform.position, Damage);
                foreach (var debuff in Global_CampaignData._CurrentBulletDeBuffs)
                {
                    mob.SetDebuff(debuff);
                }
                PierceCount--;

                if (Global_CampaignData._isReflectable)
                    if (UnityEngine.Random.Range(0f, 1f) < 0.5f) {
                        OnTriggerBoundary(); 
                    } else {
                        OnTriggerBoundary_Side(); 
                    }                
                if (PierceCount <= 0)
                    IsActive=false;
            }
            if (collision.CompareTag($"Boundary"))
            {
                OnTriggerBoundary();
            }
            if (collision.CompareTag($"Boundary_Side"))
            {
                OnTriggerBoundary_Side();
            }

        }

        protected virtual void OnTriggerBoundary()
        {
            IsActive = false;
        }
        protected virtual void OnTriggerBoundary_Side()
        {
            IsActive = false;
        }

    }
}