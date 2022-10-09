using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle 
{
    public class Projectile_Lightning : Projectile_Script
    {

        [SerializeField]
        LazerLine _thisRay;

        // Start is called before the first frame update
        void Start()
        {
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
        public override void SetInitialProjectileData(MobScript target, float damage, float lifetime, float spreadCount)
        {
            base.SetInitialProjectileData(target, damage, lifetime, spreadCount);

            OnObjectEnabled();
            if (_targetMob != null)
            {
                if (_targetMob.GetMobPosition() != null)
                {
                    _thisRay.SetLazerEachPos(transform.position, _targetMob.GetMobPosition());
                    _targetMob.Damage(damage);
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
            _hitFX.Play();
            _thisRay.SetActiveLazer(true);
        }

        protected override void OnObjectDisabled()
        {
            //Debug.Log("What" + gameObject.name);
            _thisRay.SetActiveLazer(false);
            base.OnObjectDisabled();
        }


    }

}
