using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
using PG.Data;
using System;
using System.Linq;

namespace PG.Battle 
{
    public class FXCallManager : MonoSingleton<FXCallManager>
    {

        protected override void CallOnAwake ()
        {
            Global_BattleEventSystem._onPatternSuccessed += CallPatternEvent;
            _allOwnningPattern = Enum.GetValues(typeof(DrawPatternPresetID)).Cast<DrawPatternPresetID>().ToList();
            foreach (DrawPatternPresetID val in _allOwnningPattern) 
            {
                GameObject tempt = Instantiate(
                    Resources.Load<GameObject>("Effect/PatternFX/" + val.ToString()),
                    _waitingTransform.position, Quaternion.identity, _waitingTransform);
                _patternPrefabDic.Add(val, tempt.GetComponent<ParticleSystem>());
                tempt.GetComponent<ParticleSystem>().Play();
            }

            _slashFXContainer = new NormalObjectPool<ParticleSystem>(
                CreateSlash,
                OnGetSlash,
                OnReleaseSlash
            );
            _deadFXContainer = new NormalObjectPool<ParticleSystem>(
                CreateDeadParticle,
                OnGetDead,
                OnReleaseDead
            );
            for(int i = 0 ; i<5 ;i++)
                _slashFXContainer.FillStack();
            for(int i = 0 ; i<5 ;i++)
                _deadFXContainer.FillStack();

        }
        protected override void CallOnDestroy()
        {
            Global_BattleEventSystem._onPatternSuccessed -= CallPatternEvent;
        }
        #region patternComplete

        
        List<DrawPatternPresetID> _allOwnningPattern = new List<DrawPatternPresetID>() { };
        Dictionary<DrawPatternPresetID, ParticleSystem> _patternPrefabDic = 
            new Dictionary<DrawPatternPresetID, ParticleSystem>() {};

        [SerializeField]
        Transform _patternTransform;
        [SerializeField]
        Transform _waitingTransform;
        // Start is called before the first frame update

        //패턴FX 의 경우 생성 될경우 어차피 여러번 쓸꺼니까 생성 시키고 저장해두지뭐 

       
        private void CallPatternEvent(DrawPatternPresetID patternPreset) 
        {
            _patternPrefabDic[patternPreset].transform.position = _patternTransform.position;
            _patternPrefabDic[patternPreset].Play();

        }
        #endregion

        #region patternSlash

        [SerializeField] private GameObject slashParticle;
        [SerializeField] private GameObject deadParticle;
        private NormalObjectPool<ParticleSystem> _slashFXContainer;
        private NormalObjectPool<ParticleSystem> _deadFXContainer;

        public static void PlaySlashFX(Vector2 origin,Vector2 end,float damage)
        {
            ParticleSystem target = _instance._slashFXContainer.PickUp();
            Vector2 pos = origin + end;
            pos /= 2;
            target.transform.position = pos;
            pos = end - origin;
            float angle = Mathf.Atan2(pos.y,pos.x)* Mathf.Rad2Deg;
            target.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            target.GetComponent<PattenSwipeDamageCAller>().SetPatternSwipeAttack(damage);
            _instance.PlaySlash(target);
        }

        public static void PlayDeadFX(Vector2 origin)
        {
            ParticleSystem target = _instance._deadFXContainer.PickUp();
            target.transform.position = origin;
            _instance.PlayDead(target);
        }

        void PlaySlash(ParticleSystem target )
        {
            target.Play();
            StartCoroutine(SlashAutoReturner(target));
        }
        
        
        IEnumerator  SlashAutoReturner(ParticleSystem target)
        {            
            yield return new WaitForSeconds(1f);
            _slashFXContainer.SetBack(target);
            target.GetComponent<PattenSwipeDamageCAller>().ReleasePatternSwipeAttack();

        }
        void PlayDead(ParticleSystem target )
        {
            target.Play();
            StartCoroutine(DeadAutoReturner(target));
        }
        IEnumerator DeadAutoReturner(ParticleSystem target)
        {
            yield return new WaitForSeconds(1f);
            _deadFXContainer.SetBack(target);
        }



        private ParticleSystem CreateSlash()
        {
            ParticleSystem particle = Instantiate(slashParticle, _waitingTransform).GetComponent<ParticleSystem>();
            return particle;
        }

        private void OnGetSlash(ParticleSystem particle)
        {
            particle.gameObject.SetActive(true);
        }    
        private void OnReleaseSlash(ParticleSystem particle)
        {
            particle.gameObject.SetActive(false);
        }

        private ParticleSystem CreateDeadParticle()
        {
            ParticleSystem particle = Instantiate(deadParticle, _waitingTransform).GetComponent<ParticleSystem>();
            return particle;
        }

        private void OnGetDead(ParticleSystem particle)
        {
            particle.gameObject.SetActive(true);
        }    
        private void OnReleaseDead(ParticleSystem particle)
        {
            particle.gameObject.SetActive(false);
        }


        #endregion




    }

}
