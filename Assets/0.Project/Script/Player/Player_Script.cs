using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.HealthSystemCM;
using DG.Tweening;
using PG.Event;
using PG.Data;
namespace PG.Battle 
{
    public class Player_Script : MonoSingleton<Player_Script>, IGetHealthSystem, ISetNontotalPause
    {
       

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
        protected override void CallOnAwake()
        {
            _healthSystem = new HealthSystem(healthAmountMax);
            _healthSystem.SetHealth(startingHealthAmount);
            _healthSystem.OnDead += HealthSystem_OnDead;
            Health_Refresh();
            Global_BattleEventSystem._onNonTotalPause += SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause += SetNonTotalPauseOff;
            Global_BattleEventSystem._onPlayerSizeChanged += SetPlayerSizeByEvent;
            _isDead = false;
        }
        protected override void CallOnDestroy()
        {
            Global_BattleEventSystem._onNonTotalPause -= SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause -= SetNonTotalPauseOff;
            Global_BattleEventSystem._onPlayerSizeChanged -= SetPlayerSizeByEvent;
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
        {/*
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _healthBar.AddRemoveSegments(1f);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _healthBar.AddRemoveSegments(-1f);
            }

            */

        }
        #region//damage related

        public static void Damage(float _amount)
        {
            if (_instance._isDead)
                return;

            CameraShaker.ShakeCamera(0.5f, 1);
            _instance._healthSystem.Damage(_amount);
            _instance.currentHealth -= _amount;
            _instance.Health_Refresh();
            _instance._damageFX.Play();
            //s_instance._healthBar.DoFadeHealth(s_instance._healthFadeTime);
            //DamageTextScript.Create(s_instance._thisSprite.transform.position, 0.5f, 0.3f, (int)_amount, Color.green);
            DamageFXManager.ShowDamage(Player_Script.ReturnCurrentTransform(), 1f, Mathf.FloorToInt(_amount),
                Color.green, _instance.transform, _instance.transform);


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

        public Player_Status _playerStatus = new Player_Status(Data.DrawPatternPreset.Default_Thunder);
        #region //get

        //현재의 플레이어 스테이터스를 인식으로 받는다.
        public static Player_Status GetPlayerStatus() 
        {
            return _instance._playerStatus;
        }
        public static Vector3 ReturnCurrentTransform()
        {
            return _instance.transform.position;
        }

        #endregion
        #region//일시정지시 정지할 행동들

        bool _isLevelupPaused = false;
        public void SetNonTotalPauseOn()
        {
            _isLevelupPaused = true;
        }

        public void SetNonTotalPauseOff()
        {
            _isLevelupPaused = false;
        }
        #endregion


        public void SetPlayerSizeByEvent() 
        {
            Debug.Log(Global_CampaignData._playerSize.FinalValue);
            //현재의 플레이어의 사이즈를 잘 조절 하여서 만든다.
            transform.localScale = (Global_CampaignData._playerSize.FinalValue * new Vector3(1, 1, 1));
        }


    }


    //앞으로 플레이어의 공격 배율과 관련된 데이터를 담단 하는 부분.
    [System.Serializable]
    public class Player_Status 
    {
        public DataEntity _damageData = new DataEntity(DataEntity.Type.Damage,10);
        public DataEntity _chargeData = new DataEntity(DataEntity.Type.ChargeGauge, 10);
        public Data.DrawPatternPreset _currentChargePattern = Data.DrawPatternPreset.Default_Thunder;

        public Player_Status(Data.DrawPatternPreset currentpattern) 
        {
            _currentChargePattern = currentpattern;
        }

    }

}
