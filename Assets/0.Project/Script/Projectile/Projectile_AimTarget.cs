using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle
{
    public class Projectile_AimTarget : Projectile_Script
    {
        [SerializeField]
        private float _initialSpeed = 1;
        private float _acceleration = 0;
        private Vector3 _direction = new Vector3();
        protected float InitialSpeed { get => _initialSpeed; set => _initialSpeed = value; }
        protected float Acceleration { get => _acceleration; set => _acceleration = value; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_lifeTime <= 0)
                OnObjectDisabled();
        }
        public override void SetInitialProjectileData(MobScript _target, float damage, float lifetime)
        {
            if (_target == null)
                return;

            base.SetInitialProjectileData(_target, damage, lifetime);

            _direction = _target.GetMobPosition() - Player_Script.GetPlayerPosition();
            _direction = _direction.normalized;
            InitialSpeed = Data.Global_CampaignData._projectileSpeed.FinalValue;
            Movement();
        }
        void Movement()
        {
            _lifeTime -= Time.deltaTime;
            _movement = InitialSpeed*10 * Time.deltaTime * _direction;
            if (_rigidBody2D != null)
            {
                _rigidBody2D.velocity = _movement;
            }
            //InitialSpeed += Acceleration * Time.deltaTime;
        }




    }
}
