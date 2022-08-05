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
            Global_BattleEventSystem._onLevelUpPause += SetLevelUpPauseOn;
            Global_BattleEventSystem._offLevelUpPause += SetLevelUpPauseOff;
        }
        private void OnDestroy()
        {
            Global_BattleEventSystem._onLevelUpPause -= SetLevelUpPauseOn;
            Global_BattleEventSystem._offLevelUpPause -= SetLevelUpPauseOff;
        }


        public override void SetSpawnData(float lifeTime, float activetimes)
        {
            _Animator.SetBool("isActive",false);
            _maxLifetime = lifeTime;
            _lifeTime = _maxLifetime;
            _activetime = activetimes;
            _isPlaced = true;
            _spawnedParticle.Play();

        }
        protected override void SetActiveObstacle()
        {
            _thisCollider.enabled = true;
            _activeParticle.Play();
            _Animator.SetBool("isActive", true);
        }


        void Update()
        {
            CheckStatus();
        }


    }
}