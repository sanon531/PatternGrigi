using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.HealthSystemCM;
using DG.Tweening;
public class Enemy_Script : MonoBehaviour
{
    public static Enemy_Script Instance;
    HealthSystemComponent _this_health;
    [SerializeField]
    SpriteRenderer _sprite;
    [SerializeField]
    ParticleSystem _damageFX;


    private void Start()
    {
        if (Instance != null)
            Debug.LogError("nore than one enemy error");
        Instance = this;
        _this_health = GetComponent<HealthSystemComponent>();
        _this_health.GetHealthSystem().OnDead += HealthSystem_OnDead;
    }

    public static void Damage(float _amount)
    {
        Instance._this_health.GetHealthSystem().Damage(_amount);
        Instance._damageFX.Play();
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        _sprite.DOFade(0, 2);
    }


}


