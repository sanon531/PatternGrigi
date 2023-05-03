using System;
using UnityEngine;

namespace PG.Battle
{
    public class PattenSwipeDamageCAller : MonoBehaviour
    {
        private float _damage = 0;
        [SerializeField]
        private BoxCollider2D thisCollider2d;

        public void SetPatternSwipeAttack(float damage)
        {
            _damage = damage;
            thisCollider2d.enabled = true;
        }

        public void ReleasePatternSwipeAttack()
        {
            thisCollider2d.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                var mob = col.GetComponent<MobScript>();
                mob.Damage(transform.position, _damage);
            }
        }
    }
}
