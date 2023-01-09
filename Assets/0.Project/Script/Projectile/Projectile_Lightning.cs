using System.Collections;
using System.Collections.Generic;
using PG.Data;
using UnityEngine;
using UnityEngine.Pool;

namespace PG.Battle 
{
    public class Projectile_Lightning : ProjectileScript
    {

        [SerializeField]
        LazerLine _thisRay;

        // Start is called before the first frame update
        void Start()
        {
            //hitFX.Stop();
            //_thisRay.SetActiveLazer(false);
            id = ProjectileID.LightningShot;
            IsActive = false;
        }
        // Update is called once per frame
        public override void SetInitialProjectileData(IObjectPoolSW<ProjectileScript> objectPool, float lifetime)
        {
            base.SetInitialProjectileData(objectPool, lifetime);

        }
        protected override void LateUpdate()
        {
            if (CurrentLifeTime <= 0 || !IsActive)
            {
                //hitFX.Stop();
                ProjectilePool.SetBack(this);
            }
            else
                CurrentLifeTime -= Time.deltaTime;
        }

        public override void SetFrequentProjectileData(MobScript target, float damage, float spreadCount)
        {
            base.SetFrequentProjectileData(target, damage, spreadCount);
            IstargetMobNotNull = targetMob != null;
            //hitFX.Play();

            _thisRay._StartPos = transform.position;
            if (IstargetMobNotNull)
            {            
                _thisRay._EndPos = targetMob.GetMobPosition();
                target.Damage(damage);
            }
            else
            {                
                _thisRay._EndPos = _thisRay._StartPos;
            }

        }





    }

}
