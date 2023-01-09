using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace PG.Battle 
{

    public class Projectile_GoForth : ProjectileScript
    {
        //[SerializeField]
        //TrailRenderer _ongoingTrail;

        [FormerlySerializedAs("_initialSpeed")] 
        [SerializeField] private float initialSpeed = 1;
        private float _acceleration = 0;

        protected float InitialSpeed
        {
            get => initialSpeed;
            set => initialSpeed = value;
        }

        protected float Acceleration
        {
            get => _acceleration;
            set => _acceleration = value;
        }

        // Start is called before the first frame update


        // Update is called once per frame

        public override void SetInitialProjectileData(IObjectPoolSW<ProjectileScript> objectPool, float lifetime)
        {
            base.SetInitialProjectileData(objectPool, lifetime);
            InitialSpeed = Data.Global_CampaignData._projectileSpeed.FinalValue;
            //Debug.Log(_lifeTime);
            DoMove();
        }

        public override void SetFrequentProjectileData(MobScript target, float damage, float spreadCount)
        {
            base.SetFrequentProjectileData(target, damage, spreadCount);
            DoMove();
            
        }

        void DoMove()
        {
            CurrentLifeTime -= Time.deltaTime;
            if (IsrigidBody2DNotNull)
            {
                rigidBody2D.velocity = InitialSpeed  * Vector3.up;;
                print(rigidBody2D.velocity);
            }
            //InitialSpeed += Acceleration * Time.deltaTime;
        }



    }
}
