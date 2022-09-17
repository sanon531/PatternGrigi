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

        List<DrawPatternPresetID> _allOwnningPattern = new List<DrawPatternPresetID>() { };
        Dictionary<DrawPatternPresetID, ParticleSystem> _patternPrefabDic = 
            new Dictionary<DrawPatternPresetID, ParticleSystem>() {};

        [SerializeField]
        Transform _patternTransform;
        [SerializeField]
        Transform _waitingTransform;
        // Start is called before the first frame update

        //����FX �� ��� ���� �ɰ�� ������ ������ �����ϱ� ���� ��Ű�� �����ص����� 

        protected override void CallOnAwake ()
        {
            Global_BattleEventSystem._onPatternSuccessed += CallPatternEvent;
            _allOwnningPattern = Enum.GetValues(typeof(DrawPatternPresetID)).Cast<DrawPatternPresetID>().ToList();
            foreach (DrawPatternPresetID val in _allOwnningPattern) 
            {
                GameObject _tempt = Instantiate(
                    Resources.Load<GameObject>("Effect/PatternFX/" + val.ToString()),
                    _waitingTransform.position, Quaternion.identity, transform);
                _patternPrefabDic.Add(val, _tempt.GetComponent<ParticleSystem>());
                _tempt.GetComponent<ParticleSystem>().Play();
            }

        }
        protected override void CallOnDestroy()
        {
            Global_BattleEventSystem._onPatternSuccessed -= CallPatternEvent;
        }
        private void CallPatternEvent(DrawPatternPresetID _patternPreset) 
        {
            _patternPrefabDic[_patternPreset].transform.position = _patternTransform.position;
            _patternPrefabDic[_patternPreset].Play();

        }

    }

}
