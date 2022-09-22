using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle 
{
    public class Projectile_Lightning : Projectile_Script
    {

        [SerializeField]
        LazerParticle _thisRay;
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

        // Update is called once per frame
        public override bool SetInitialProjectileData(MobScript target, float damage, float lifetime)
        {
            if (!base.SetInitialProjectileData(target, damage, lifetime))
                return false;
            OnObjectEnabled();
            _thisRay._EndPos = target.GetMobPosition();
            _thisRay._StartPos = Player_Script.GetPlayerPosition();
            target.Damage(damage);


            return true;
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
            _RayParticle.Stop();
            _ongoingFX.Stop();
            _hitFX.Stop();
            _thisRay.SetActiveLazer(false);
            base.OnObjectDisabled();
        }



    }

}
