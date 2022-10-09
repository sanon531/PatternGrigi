using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle
{
    public class Projectile_AimTarget : Projectile_Script
    {

        //[SerializeField]
        //TrailRenderer _ongoingTrail;

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
        public override void SetInitialProjectileData(MobScript target, float damage, float lifetime, float spreadCount)
        {
            base.SetInitialProjectileData(target, damage, lifetime, spreadCount);
            OnObjectEnabled();
            //Debug.Log("Pos"+ transform.position);

            if (_targetMob != null)
                _direction = target.GetMobPosition() - transform.position;
            else
                _direction = Vector3.up;

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
        protected override void OnObjectEnabled()
        {
            base.OnObjectEnabled();
            _projectileImage.enabled = true;
            //_ongoingTrail.enabled = true;
            
        }
        protected override void OnObjectDisabled()
        {
            _lifeTime = 0f;
            _projectileImage.enabled = false;
            //_ongoingTrail.enabled = false;
            base.OnObjectDisabled();
        }




    }
}
