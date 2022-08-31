using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
using PG.Data;

namespace PG.Battle 
{
    public class FXCallManager : MonoSingleton<FXCallManager>
    {

        [SerializeField]
        List<DrawPatternPreset> _ownningPattern = new List<DrawPatternPreset>() { };
        Dictionary<DrawPatternPreset, ParticleSystem> _patternPrefabDic = 
            new Dictionary<DrawPatternPreset, ParticleSystem>() {};

        [SerializeField]
        Transform _patternTransform;
        // Start is called before the first frame update
        
        //패턴FX 의 경우 생성 될경우 어차피 여러번 쓸꺼니까 생성 시키고 저장해두지뭐 
        void Start()
        {
            Global_BattleEventSystem._onPatternSuccessed += CallPatternEvent;
            foreach (DrawPatternPreset val in _ownningPattern) 
            {
                GameObject _tempt = Instantiate(
                    Resources.Load<GameObject>("Effect/PatternFX/" + val.ToString()), 
                    _patternTransform.position, Quaternion.identity, transform);
                _patternPrefabDic.Add(val, _tempt.GetComponent<ParticleSystem>());
            }

        }
        protected override void CallOnDestroy()
        {
            Global_BattleEventSystem._onPatternSuccessed -= CallPatternEvent;
        }
        private void CallPatternEvent(DrawPatternPreset _patternPreset) 
        {
            _patternPrefabDic[_patternPreset].Play();

        }

    }

}
