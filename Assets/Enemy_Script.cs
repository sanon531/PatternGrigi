using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.HealthSystemCM;
public class Enemy_Script : MonoBehaviour
{
    public static Enemy_Script Instance;
    HealthSystemComponent _this_health;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("nore than one enemy error");
        Instance = this;
        _this_health = GetComponent<HealthSystemComponent>();
    }

    public static void Damage(float _amount) 
    {
        Instance._this_health.GetHealthSystem().Damage(_amount);
    }


}


