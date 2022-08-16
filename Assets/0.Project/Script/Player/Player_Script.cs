using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.HealthSystemCM;
using DG.Tweening;
using PG.Event;
namespace PG.Battle 
{
    public class Player_Script : MonoBehaviour, IGetHealthSystem, ISetNontotalPause
    {
        public static Player_Script s_instance;
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
            s_instance = this;
            _healthSystem = new HealthSystem(healthAmountMax);
            _healthSystem.SetHealth(startingHealthAmount);
            _healthSystem.OnDead += HealthSystem_OnDead;
            Health_Refresh();
            Global_BattleEventSystem._onNonTotalPause += SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause += SetNonTotalPauseOff;
            _isDead = false;
        }
        private void OnDestroy()
        {
            Global_BattleEventSystem._onNonTotalPause -= SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause -= SetNonTotalPauseOff;
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
            if (s_instance._isDead)
                return;

            CameraShaker.ShakeCamera(0.5f, 1);
            s_instance._healthSystem.Damage(_amount);
            s_instance.currentHealth -= _amount;
            s_instance.Health_Refresh();
            s_instance._damageFX.Play();
            //s_instance._healthBar.DoFadeHealth(s_instance._healthFadeTime);
            DamageTextScript.Create(s_instance._thisSprite.transform.position, 0.5f, 0.3f, (int)_amount, Color.green);
            GlobalUIEventSystem.CallOnDamageUI();
            //DamageFXManager.Damage(_amount);

        }

        [SerializeField]
        float _healthFadeTime = 1f;
        private void HealthSystem_OnDead(object sender, System.EventArgs e)
        {
            //_thisSprite.DOFade(0, _healthFadeTime);
            VibrationManager.CallGameOverVib();
            Global_BattleEventSystem.CallOnGameOver();
            _isDead = true;
        }

        public HealthSystem GetHealthSystem()
        {
            return _healthSystem;
        }

        #endregion

        public Player_Status _playerStatus = new Player_Status(Data.EDrawPatternPreset.Default_Thunder);


        //현재의 플레이어 스테이터스를 인식으로 받는다.
        public static Player_Status GetPlayerStatus() 
        {
            return s_instance._playerStatus;
        }


        public static Vector3 ReturnCurrentTransform()
        {
            return s_instance.transform.position;
        }
        //일시정지시 정지할 행동들

        bool _isLevelupPaused = false;
        public void SetNonTotalPauseOn()
        {
            _isLevelupPaused = true;
        }

        public void SetNonTotalPauseOff()
        {
            _isLevelupPaused = false;
        }
    }


    //앞으로 플레이어의 공격 배율과 관련된 데이터를 담단 하는 부분.
    [System.Serializable]
    public class Player_Status 
    {
        public DataEntity _damageData = new DataEntity(DataEntity.Type.Damage,10);
        public DataEntity _chargeData = new DataEntity(DataEntity.Type.ChargeGauge, 10);
        public Data.EDrawPatternPreset _currentChargePattern = Data.EDrawPatternPreset.Default_Thunder;

        public Player_Status(Data.EDrawPatternPreset currentpattern) 
        {
            _currentChargePattern = currentpattern;
        }

    }

}
