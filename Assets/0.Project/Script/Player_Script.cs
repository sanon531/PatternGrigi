using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.HealthSystemCM;
using DG.Tweening;
public class Player_Script : MonoBehaviour
{
    public static Player_Script Instance;
    HealthSystemComponent _this_healthComponent;
    HealthSystem _this_health;
    [SerializeField]
    SpriteRenderer _thisSprite;
    [SerializeField]
    RengeGames.HealthBars.UltimateCircularHealthBar _healthBar;
    [SerializeField]
    ParticleSystem _damageFX;
    // Start is called before the first frame update
    void Start()
    {
        _this_healthComponent = GetComponent<HealthSystemComponent>();
        _this_health = _this_healthComponent.GetHealthSystem();
        _this_health.OnDead += HealthSystem_OnDead;
    }

    void Health_Refresh() 
    {
        //단위는 20에 1칸임.
        float max_health = _this_health.GetHealthMax();
        float currenthealth = _this_health.GetHealth();
        float currentProperty = currenthealth/ max_health;

        _healthBar.


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void Damage(float _amount)
    {
        Instance._this_health.Damage(_amount);
        Instance._this_health.GetHealth();
        Instance._damageFX.Play();
        Instance._healthBar.DoFadeHealth(Instance._healthFadeTime);
    }

    [SerializeField]
    float _healthFadeTime = 1f;
    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        _thisSprite.DOFade(0, _healthFadeTime);

    }

}
