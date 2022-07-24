using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle
{
    public class Obstacle_Fire : Obstacle
    {

        //애니메이터와 이펙트를 기반으로 하나하나 해보도록 하자.
        [SerializeField]
        Animator _Animator;

        // Start is called before the first frame update
        void Start()
        {

        }

        public override void SetSpawnData(float lifeTime, float activetimes)
        {
            _Animator.SetBool("isActive",false);
            _maxLifetime = lifeTime;
            _lifeTime = _maxLifetime;
            _activetime = activetimes;
            _isPlaced = true;
        }
        protected override void SetActiveObstacle()
        {
            _thisCollider.enabled = true;
            _Animator.SetBool("isActive", true);
        }


        void Update()
        {
            CheckStatus();
        }


    }
}