using System.Collections;
using System.Collections.Generic;
using PG.Data;
using UnityEngine;
using PG.Event;

namespace PG.Battle
{
    public class Obstacle_Fire : Obstacle
    {

        //애니메이터와 이펙트를 기반으로 하나하나 해보도록 하자.
        [SerializeField]
        Animator _Animator;

        [SerializeField]
        ParticleSystem _spawnedParticle;
        [SerializeField]
        ParticleSystem _activeParticle;


        public override void SetSpawnData(float lifeTime, float activetimes,float damage,ObstacleID id)
        {
            base.SetSpawnData(lifeTime, activetimes, damage, id);
            _Animator.SetBool("isActive",false);
            _spawnedParticle.Play();
        }
        protected override void SetActiveObstacle()
        {
            base.SetActiveObstacle();
            _activeParticle.Play();
            _Animator.SetBool("isActive", true);
        }
        
    }
}