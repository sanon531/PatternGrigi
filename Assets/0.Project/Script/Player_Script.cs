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
        public static Player_Script _instance;



        #region//damage related


        HealthSystemComponent _this_healthComponent;
        HealthSystem _healthSystem;
        [SerializeField]
        private float healthAmountMax, startingHealthAmount, currentHealth;

        private bool _isDead = false;
        [SerializeField]
        SpriteRenderer _thisSprite;
        [SerializeField]
        RengeGames.HealthBars.UltimateCircularHealthBar _healthBar;
        [SerializeField]
        ParticleSystem _damageFX;
        // Start is called before the first frame update
        void Awake()
        {
            _instance = this;
            _healthSystem = new HealthSystem(healthAmountMax);
            _healthSystem.SetHealth(startingHealthAmount);
            _healthSystem.OnDead += HealthSystem_OnDead;
            Health_Refresh();
            Global_BattleEventSystem._on�������Ͻ����� += SetLevelUpPauseOn;
            Global_BattleEventSystem._off�������Ͻ����� += SetLevelUpPauseOff;
            _isDead = false;
        }
        private void OnDestroy()
        {
            Global_BattleEventSystem._on�������Ͻ����� -= SetLevelUpPauseOn;
            Global_BattleEventSystem._off�������Ͻ����� -= SetLevelUpPauseOff;
        }
        void Health_Refresh()
        {
            //������ 20�� 1ĭ��.
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
            if (_instance._isDead)
                return;

            CameraShaker.ShakeCamera(0.5f, 1);
            _instance._healthSystem.Damage(_amount);
            _instance.currentHealth -= _amount;
            _instance.Health_Refresh();
            _instance._damageFX.Play();
            _instance._healthBar.DoFadeHealth(_instance._healthFadeTime);
            DamageTextScript.Create(_instance._thisSprite.transform.position, 0.5f, 0.3f, (int)_amount, Color.green);

        }

        [SerializeField]
        float _healthFadeTime = 1f;
        private void HealthSystem_OnDead(object sender, System.EventArgs e)
        {
            _thisSprite.DOFade(0, _healthFadeTime);
            VibrationManager.CallGameOverVib();
            Global_BattleEventSystem.CallOn���ӿ���();
            _isDead = true;
        }

        public HealthSystem GetHealthSystem()
        {
            return _healthSystem;
        }

        #endregion






        public static Vector3 ReturnCurrentTransform()
        {
            return _instance.transform.position;
        }
        //�Ͻ������� ������ �ൿ��

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
