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

        List<DrawPatternPreset> _allOwnningPattern = new List<DrawPatternPreset>() { };
        Dictionary<DrawPatternPreset, ParticleSystem> _patternPrefabDic = 
            new Dictionary<DrawPatternPreset, ParticleSystem>() {};

        [SerializeField]
        Transform _patternTransform;
        // Start is called before the first frame update
        
        //����FX �� ��� ���� �ɰ�� ������ ������ �����ϱ� ���� ��Ű�� �����ص����� 
        
        protected override void CallOnAwake ()
        {
            Global_BattleEventSystem._onPatternSuccessed += CallPatternEvent;
            _allOwnningPattern = Enum.GetValues(typeof(DrawPatternPreset)).Cast<DrawPatternPreset>().ToList();
            foreach (DrawPatternPreset val in _allOwnningPattern) 
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
