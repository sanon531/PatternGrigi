using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PG.Battle 
{

    public class Projectile_GoForth : ProjectileScript
    {
        //[SerializeField]
        //TrailRenderer _ongoingTrail;

        [SerializeField] private float _initialSpeed = 1;
        private float _acceleration = 0;

        protected float InitialSpeed
        {
            get => _initialSpeed;
            set => _initialSpeed = value;
        }

        protected float Acceleration
        {
            get => _acceleration;
            set => _acceleration = value;
        }

        // Start is called before the first frame update


        // Update is called once per frame
        void FixedUpdate()
        {
            //if (_isPlaced) 
            //_rigidBody2D.velocity = _movement * ( (1 - _lifeTime) + Acceleration * Time.deltaTime);
        }

        public override void SetInitialProjectileData(IObjectPoolSW<ProjectileScript> objectPool, float lifetime)
        {
            base.SetInitialProjectileData(objectPool, lifetime);
            InitialSpeed = Data.Global_CampaignData._projectileSpeed.FinalValue;
            //Debug.Log(_lifeTime);
            Movement();
        }

        void Movement()
        {
            CurrentlifeTime -= Time.deltaTime;
            base.Movement = InitialSpeed * 10 * Time.deltaTime * Vector3.up;
            if (IsrigidBody2DNotNull)
            {
                rigidBody2D.velocity = base.Movement;
            }
            //InitialSpeed += Acceleration * Time.deltaTime;
        }



    }
}
