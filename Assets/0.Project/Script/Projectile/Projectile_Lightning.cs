using System.Collections;
using System.Collections.Generic;
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
            hitFX.Stop();
            _thisRay.SetActiveLazer(false);
        }
        // Update is called once per frame
        /*public override void SetInitialProjectileData(MobScript target, IObjectPool<ProjectileScript> objectPool, float lifetime, float spreadCount, float f)
        {
            base.SetInitialProjectileData(target, objectPool,damage, lifetime, spreadCount);

            OnObjectEnabled();
            if (targetMob != null)
            {
                if (targetMob.GetMobPosition() != null)
                {
                    _thisRay.SetLazerEachPos(transform.position, targetMob.GetMobPosition());
                    targetMob.Damage(damage);
                }
                else 
                {
                    OnObjectDisabled();

                }
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
            hitFX.Play();
            _thisRay.SetActiveLazer(true);
        }

        protected override void OnObjectDisabled()
        {
            //Debug.Log("What" + gameObject.name);
            _thisRay.SetActiveLazer(false);
            base.OnObjectDisabled();
        }

        */

    }

}
