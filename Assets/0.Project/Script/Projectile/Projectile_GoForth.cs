using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG.Battle 
{

    public class Projectile_GoForth : Projectile_Script
    {
        [SerializeField]
        TrailRenderer _ongoingTrail;

        [SerializeField]
        private float _initialSpeed = 1;
        private float _acceleration = 0;
        protected float InitialSpeed { get => _initialSpeed; set => _initialSpeed = value; }
        protected float Acceleration { get => _acceleration; set => _acceleration = value; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //if (_isPlaced) 
                //_rigidBody2D.velocity = _movement * ( (1 - _lifeTime) + Acceleration * Time.deltaTime);
        }
        public override void SetInitialProjectileData(MobScript target, float damage, float lifetime)
        {
            base.SetInitialProjectileData(target, damage, lifetime);
            OnObjectEnabled();
            InitialSpeed = Data.Global_CampaignData._projectileSpeed.FinalValue;
            Debug.Log(_lifeTime);
            Movement();
        }

        void Movement()
        {
            _lifeTime -= Time.deltaTime;
            _movement = InitialSpeed * 10 * Time.deltaTime * Vector3.up;
            if (_rigidBody2D != null)
            {
                _rigidBody2D.velocity = _movement;
            }
            //InitialSpeed += Acceleration * Time.deltaTime;
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
            _projectileImage.enabled = false;
            _ongoingTrail.enabled = false;

            base.OnObjectDisabled();
        }
    }

}
