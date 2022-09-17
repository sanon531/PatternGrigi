using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;

namespace PG.Battle
{
    public class Obstacle_Fire : Obstacle
    {

        //�ִϸ����Ϳ� ����Ʈ�� ������� �ϳ��ϳ� �غ����� ����.
        [SerializeField]
        Animator _Animator;

        [SerializeField]
        ParticleSystem _spawnedParticle;
        [SerializeField]
        ParticleSystem _activeParticle;

        // Start is called before the first frame update
        private void Start()
        {
        }
        private void OnDestroy()
        {
        }


        public override void SetSpawnData(float lifeTime, float activetimes,float damage)
        {
            base.SetSpawnData(lifeTime, activetimes, damage);
            _Animator.SetBool("isActive",false);
            _spawnedParticle.Play();

        }
        protected override void SetActiveObstacle()
        {
            _thisCollider.enabled = true;
            _activeParticle.Play();
            _Animator.SetBool("isActive", true);
        }

        protected override void FixedUpdate()
        {
            CheckStatus();
        }



    }
}