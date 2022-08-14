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
        List<EDrawPatternPreset> _ownningPattern = new List<EDrawPatternPreset>() { };
        Dictionary<EDrawPatternPreset, ParticleSystem> _patternPrefabDic = 
            new Dictionary<EDrawPatternPreset, ParticleSystem>() {};

        [SerializeField]
        Transform _patternTransform;
        // Start is called before the first frame update
        
        //����FX �� ��� ���� �ɰ�� ������ ������ �����ϱ� ���� ��Ű�� �����ص����� 
        void Start()
        {
            Global_BattleEventSystem._onPatternSuccessed += CallPatternEvent;
            foreach (EDrawPatternPreset val in _ownningPattern) 
            {
                GameObject _tempt = Instantiate(
                    Resources.Load<GameObject>("Effect/PatternFX/" + val.ToString()), 
                    _patternTransform.position, Quaternion.identity, transform);
                _patternPrefabDic.Add(val, _tempt.GetComponent<ParticleSystem>());
            }

        }
        private void OnDestroy()
        {
            Global_BattleEventSystem._onPatternSuccessed -= CallPatternEvent;
        }
        private void CallPatternEvent(EDrawPatternPreset _patternPreset) 
        {
            _patternPrefabDic[_patternPreset].Play();
        }

    }

}
