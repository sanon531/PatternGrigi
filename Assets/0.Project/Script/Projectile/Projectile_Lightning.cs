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
        LaserLine _thisRay;

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

        public override void SetFrequentProjectileData(MobScript target, float damage, Vector3 projectilePlace)
        {
            base.SetFrequentProjectileData(target, damage, projectilePlace);
            IstargetMobNotNull = targetMob != null;
            //hitFX.Play();

            _thisRay._StartPos = transform.position;
            var targetPos = Player_Script.GetPlayerPosition();

            if (IstargetMobNotNull)
            {            
                _thisRay._EndPos = targetMob.GetMobPosition();
                target.Damage(targetPos,damage);
            }
            else
            {                
                _thisRay._EndPos = _thisRay._StartPos;
            }

        }





    }

}
