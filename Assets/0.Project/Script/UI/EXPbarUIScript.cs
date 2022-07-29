using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;

public class EXPbarUIScript : MonoBehaviour
{
    static EXPbarUIScript _instance;

    [SerializeField]
    Image _bar;
    [SerializeField]
    float _maxEXP = 100;
    [SerializeField]
    float _currentEXP = 0 ;
    int _plaverLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        _maxEXP = 100f;
        Global_BattleEventSystem._on����ġȹ�� += SetCurrentEXP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetFullEXP(float val) 
    {
        //�ϴ� 100�ּ� 20%�� �þ�ٰ� ����.
        _maxEXP = val;
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
        Global_BattleEventSystem.Call�������Ͻ�����();
        SetFullEXP(_maxEXP * 1.2f); 
    }

}
