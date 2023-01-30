using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Pool;
namespace PG.Battle
{
    public class Projectile_AimTarget : ProjectileScript
    {

        //[SerializeField]
        //TrailRenderer _ongoingTrail;

        [SerializeField]private float initialSpeed = 1;
        [SerializeField] private float acceleration = 0;
        private Vector3 _direction = new Vector3();

        protected float InitialSpeed { get => initialSpeed; set => initialSpeed = value; }
        protected float Acceleration { get => acceleration; set => acceleration = value; }

        // Start is called before the first frame update
        public override void SetFrequentProjectileData(MobScript target, float damage,Vector3 projectilePlace)
        {
            base.SetFrequentProjectileData(target, damage, projectilePlace);
            
            if (target is not null)
                _direction = target.GetMobPosition() - transform.position;
            else
                _direction = Vector3.up;

            _direction = _direction.normalized;
            InitialSpeed = Data.Global_CampaignData._projectileSpeed.FinalValue;
            DoMove();
        }
        

        void DoMove()
        {
            velocity = InitialSpeed * _direction*3;
            if (IsrigidBody2DNotNull)
            {
                rigidBody2D.velocity = velocity;
            }else
                throw new ArgumentException("no rigidbody val");

            //InitialSpeed += Acceleration * Time.deltaTime;
        }




    }
}
