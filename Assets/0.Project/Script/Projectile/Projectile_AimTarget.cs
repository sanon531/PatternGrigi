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
        public override void SetFrequentProjectileData(MobScript target, float damage, float spreadCount)
        {
            base.SetFrequentProjectileData(target, damage, spreadCount);
            
            if (IstargetMobNotNull)
                _direction = target.GetMobPosition() - transform.position;
            else
                _direction = Vector3.up;

            _direction = _direction.normalized;
            InitialSpeed = Data.Global_CampaignData._projectileSpeed.FinalValue;
            Movement();
        }
        

        new void Movement()
        {
            lifeTime -= Time.deltaTime;
            base.Movement = InitialSpeed*10 * Time.deltaTime * _direction;
            if (IsrigidBody2DNotNull)
            {
                rigidBody2D.velocity = base.Movement;
            }
            //InitialSpeed += Acceleration * Time.deltaTime;
        }




    }
}
