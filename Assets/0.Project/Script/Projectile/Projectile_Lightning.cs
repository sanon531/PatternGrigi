using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle 
{
    public class Projectile_Lightning : Projectile_Script
    {

        [SerializeField]
        LazerLine _thisRay;
        [SerializeField]
        ParticleSystem _RayParticle;

        // Start is called before the first frame update
        void Start()
        {
            _RayParticle.Stop();
            _ongoingFX.Stop();
            _hitFX.Stop();
            _thisRay.SetActiveLazer(false);
        }

        protected override void LateUpdate()
        {
            if (_isPlaced)
            {
                if (_lifeTime <= 0)
                    OnObjectDisabled();
                else
                    _lifeTime -= Time.deltaTime;
            }
        }

        // Update is called once per frame
        public override void SetInitialProjectileData(MobScript target, float damage, float lifetime)
        {
            base.SetInitialProjectileData(target, damage, lifetime);

            OnObjectEnabled();
            if (_targetMob != null)
            {
                _thisRay.SetLazerEachPos(Player_Script.GetPlayerPosition(), _targetMob.GetMobPosition());
                _targetMob.Damage(damage);
            }
            else 
            {
                //나중에 번개가 그래도 나가도록 나와도 좋을것 같긴해도 뭐 일단은
                OnObjectDisabled();
            }


        }

        protected override void OnObjectEnabled()
        {
            base.OnObjectEnabled();
            _ongoingFX.Play();
            _hitFX.Play();
            _thisRay.SetActiveLazer(true);
        }

        protected override void OnObjectDisabled()
        {
            Debug.Log("What" + gameObject.name);

            _RayParticle.Stop();
            _thisRay.SetActiveLazer(false);
            base.OnObjectDisabled();
        }


    }

}
