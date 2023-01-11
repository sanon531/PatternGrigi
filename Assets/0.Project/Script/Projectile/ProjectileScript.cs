using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using UnityEngine.Serialization;

namespace PG.Battle
{
    //����ü�� trigger���·� �ϸ� ���� �ݴ�� Ʈ���Ű� ����ä�� �����Ѵ�. 
    //������ ��Ȱ��ȭ �ǰ�
    //projectile �� �÷��̾ Ȱ���ϴ°����� �Ѵ�.
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
        /// <summary>
        /// �߻� �Ҷ����� �۵��ϴ� ��ũ��Ʈ.
        /// </summary>
        public virtual void SetFrequentProjectileData(MobScript target, float damage, float spreadCount)
        {
            IsActive = true;
            PierceCount = 0;
            CurrentLifeTime = MaxLifeTime;
            IstargetMobNotNull = targetMob != null;
            targetMob = target;
            Damage = damage;
            transform.position = (Player_Script.GetPlayerPosition() + new Vector3(spreadCount, 0, 0));
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
                // �̰����� ���� ���� �������� ó���ϴ� �ڵ带 §��.
                collision.GetComponent<MobScript>().Damage(Damage);
                PierceCount--;
                if (PierceCount <= 0)
                    IsActive=false;
            }
        }
    }
}