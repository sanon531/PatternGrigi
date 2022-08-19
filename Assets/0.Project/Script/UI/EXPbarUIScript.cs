using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;
using TMPro;
using DG.Tweening;

public class EXPbarUIScript : MonoBehaviour
{

    [SerializeField]
    Image _bar;
    [SerializeField]
    float _maxEXP = 100;
    [SerializeField]
    float _currentEXP = 0 ;
    int _plaverLevel = 1;
    [SerializeField]
    TextMeshProUGUI _levelShow;

    // Start is called before the first frame update
    void Start()
    {
        _maxEXP = 100f;
        Global_BattleEventSystem._onGainEXP += SetCurrentEXP;
    }
    private void OnDestroy()
    {
        Global_BattleEventSystem._onGainEXP -= SetCurrentEXP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetFullEXP(float val) 
    {
        //일단 100애서 20%씩 늘어난다고 하자.
        _maxEXP = val;
        _plaverLevel++;
        _levelShow.text = "Lv." + _plaverLevel.ToString();
    }
    void SetCurrentEXP(float val) 
    {
        _currentEXP += val;
        if (_currentEXP >= _maxEXP) 
        {
            _currentEXP -= _maxEXP;
            SetLevelUp();
        }
        _bar.fillAmount = _currentEXP / _maxEXP;
    }
    void SetLevelUp() 
    {
        Global_BattleEventSystem.CallOnLevelUp();
        SetFullEXP(_maxEXP * 1.2f); 
    }

}
