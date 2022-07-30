using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.HealthSystemCM;
using DG.Tweening;
using PG.Event;
namespace PG.Battle 
{
    public class Player_Script : MonoBehaviour, IGetHealthSystem, ISetLevelupPause
    {
        public static Player_Script Instance;



        #region//damage related


        HealthSystemComponent _this_healthComponent;
        HealthSystem _healthSystem;
        [SerializeField]
        private float healthAmountMax, startingHealthAmount, currentHealth;

        [SerializeField]
        SpriteRenderer _thisSprite;
        [SerializeField]
        RengeGames.HealthBars.UltimateCircularHealthBar _healthBar;
        [SerializeField]
        ParticleSystem _damageFX;
        // Start is called before the first frame update
        void Awake()
        {
            Instance = this;
            _healthSystem = new HealthSystem(healthAmountMax);
            _healthSystem.SetHealth(startingHealthAmount);
            _healthSystem.OnDead += HealthSystem_OnDead;
            Health_Refresh();
            Global_BattleEventSystem._on레벨업일시정지 += SetLevelUpPauseOn;
            Global_BattleEventSystem._on레벨업일시정지해제 += SetLevelUpPauseOff;

        }

        void Health_Refresh()
        {
            //단위는 20에 1칸임.
            float currentProperty = currentHealth / healthAmountMax;
            _healthBar.SetSegmentCount(healthAmountMax / 20);
            _healthBar.SetPercent(currentProperty);

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _healthBar.AddRemoveSegments(1f);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _healthBar.AddRemoveSegments(-1f);
            }



        }
        public static void Damage(float _amount)
        {
            CameraShaker.ShakeCamera(1, 1);
            Instance._healthSystem.Damage(_amount);
            Instance.currentHealth -= _amount;
            Instance.Health_Refresh();
            Instance._damageFX.Play();
            Instance._healthBar.DoFadeHealth(Instance._healthFadeTime);
            DamageTextScript.Create(Instance._thisSprite.transform.position, 0.5f, 0.3f, (int)_amount, Color.green);

        }

        [SerializeField]
        float _healthFadeTime = 1f;
        private void HealthSystem_OnDead(object sender, System.EventArgs e)
        {
            _thisSprite.DOFade(0, _healthFadeTime);

        }

        public HealthSystem GetHealthSystem()
        {
            return _healthSystem;
        }

        #endregion






        public static Vector3 ReturnCurrentTransform()
        {
            return Instance.transform.position;
        }
        //일시정지시 정지할 행동들

        bool _isLevelupPaused = false;
        public void SetLevelUpPauseOn()
        {
            _isLevelupPaused = true;
        }

        public void SetLevelUpPauseOff()
        {
            _isLevelupPaused = false;
        }
    }

}
