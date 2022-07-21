using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.HealthSystemCM;
using DG.Tweening;
using MoreMountains.NiceVibrations;
public class Enemy_Script : MonoBehaviour, IGetHealthSystem
{
    public static Enemy_Script Instance;
    [SerializeField]
    private float healthAmountMax, startingHealthAmount, currentHealth;
    private HealthSystem _healthSystem;
    [SerializeField]
    SpriteRenderer _sprite;
    [SerializeField]
    ParticleSystem _damageFX;
    public List<PresetDemoItem> _VibrationItems;

    private void Start()
    {
        if (Instance != null)
            Debug.LogError("nore than one enemy error");
        Instance = this;
        _healthSystem = new HealthSystem(healthAmountMax);
        _healthSystem.SetHealth(startingHealthAmount);
        _healthSystem.OnDead += HealthSystem_OnDead;
    }


    private void Update()
    {

    }


    void CheckAction() 
    {
    
    }




    #region //Damage related
    public static void Damage(float _amount)
    {
        Instance._healthSystem.Damage(_amount);
        Instance._damageFX.Play();
        
        DamageTextScript.Create(Instance._sprite.transform.position, 0.5f, 0.3f, (int)_amount, Color.red);
        
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        CameraShaker.ShakeCamera(10, 1);
        _sprite.DOFade(0, 2);
    }

    public HealthSystem GetHealthSystem()
    {
        return _healthSystem;
    }

    public virtual void PlayAHAP(int index)
    {
        MMVibrationManager.AdvancedHapticPattern(_VibrationItems[index].AHAPFile.text,
                                         _VibrationItems[index].WaveFormAsset.WaveForm.Pattern, _VibrationItems[index].WaveFormAsset.WaveForm.Amplitudes, -1,
                                         _VibrationItems[index].RumbleWaveFormAsset.WaveForm.Pattern, _VibrationItems[index].RumbleWaveFormAsset.WaveForm.LowFrequencyAmplitudes,
                                         _VibrationItems[index].RumbleWaveFormAsset.WaveForm.HighFrequencyAmplitudes, -1,
                                         HapticTypes.LightImpact, this, -1, false);

    }

    #endregion


}


