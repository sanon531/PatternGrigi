using System;
using UnityEngine;

namespace PG.Battle
{
    
    public class OrbitSwordScript : MonoBehaviour
    {
        [SerializeField] private bool isActive = false;
        private Transform _targetTransform;
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] private float attackDamage = 10;

        public void StartOrbitSword()
        {
            _targetTransform = Player_Script._instance.transform;
            transform.position = Player_Script.GetPlayerPosition() + new Vector3(0.5f, 0.5f, 1);
            isActive = true;
        }

        public void UpgradeOrbitSword(float size , float damage)
        {
            transform.localScale = new Vector3(size,size,size);
            attackDamage = damage;
        }


        private void FixedUpdate()
        {
            if(isActive)
                transform.RotateAround(_targetTransform.position,Vector3.back, Time.fixedDeltaTime*moveSpeed);   
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                var mob = col.GetComponent<MobScript>();
                mob.Damage(transform.position, attackDamage);
            }        
        }
    }
}
