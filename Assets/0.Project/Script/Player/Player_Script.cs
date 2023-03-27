using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.HealthSystemCM;
using DG.Tweening;
using PG.Event;
using PG.Data;

namespace PG.Battle 
{
    public class Player_Script : MonoSingleton<Player_Script>, IGetHealthSystem
    {
        

        HealthSystem _healthSystem;
        [SerializeField]
        private float healthAmountMax, startingHealthAmount, currentHealth;

        private bool _isDead = false;
        [SerializeField]
        SpriteRenderer _thisSprite;
        [SerializeField]
        Image _healthBar;
        [SerializeField]
        ParticleSystem _damageFX;
        [SerializeField]
        ParticleSystem _healFX;

        [SerializeField]
        Transform _UIShowPos;

        public float _knockbackForce;

        // Start is called before the first frame update
        protected override void CallOnAwake()
        {
            _healthSystem = new HealthSystem(healthAmountMax);
            _healthSystem.SetHealth(startingHealthAmount);
            _healthSystem.OnDead += HealthSystem_OnDead;
            Health_Refresh();
            Global_BattleEventSystem._onPlayerSizeChanged += SetPlayerSizeByEvent;
            _touchFollowScript = GetComponent<TouchFollowScript>();
            _isDead = false;
            SetPlayerHP(Global_CampaignData._playerHealth);
        }
        protected override void CallOnDestroy()
        {
            Global_BattleEventSystem._onPlayerSizeChanged -= SetPlayerSizeByEvent;
        }
        void Health_Refresh()
        {
            //단위는 20에 1칸임.
            float currentProperty = currentHealth / healthAmountMax;
            _healthBar.DOFillAmount(currentProperty,0.5f);

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
        #region damage related

        public static void Damage(float _amount)
        {
            if (_instance._isDead)
                return;

            if (_amount < 0)
            {
                Heal(-_amount);
                return;
            }
            CameraShaker.ShakeCamera(0.5f, 1);
            _instance._healthSystem.Damage(_amount);
            _instance.currentHealth -= _amount;
            _instance.Health_Refresh();
            _instance._damageFX.Play();
            DamageFXManager.ShowDamage(_instance._UIShowPos.position, Mathf.FloorToInt(_amount).ToString(),Color.red);


            GlobalUIEventSystem.CallOnDamageUI();
            //DamageFXManager.Damage(_amount);

        }

        public static void Heal(float _amount)
        {
            if (_instance._isDead)
                return;
            _instance._healthSystem.Damage(_amount);
            _instance.currentHealth += _amount;
            _instance.Health_Refresh();
            _instance._healFX.Play();
                
            DamageFXManager.ShowDamage(_instance._UIShowPos.position, Mathf.FloorToInt(_amount).ToString(),Color.green);
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

        public Player_Status _playerStatus = new Player_Status();
        #region get

        //현재의 플레이어 스테이터스를 인식으로 받는다.
        public static Player_Status GetPlayerStatus() 
        {
            return _instance._playerStatus;
        }
        public static Vector3 GetPlayerPosition()
        {
            return _instance.transform.position;
        }


        private TouchFollowScript _touchFollowScript;
        public static void SetThisSpeed()
        {
            _instance._touchFollowScript.SetSpeedAndTeleport(
                Global_CampaignData._playerSpeed,
                Global_CampaignData._playerTeleport
                );
        }
        
        public static void SetPlayerHP(float MaxHealth)
        {
            _instance.GetHealthSystem().SetHealthMax(MaxHealth,true);
            _instance.healthAmountMax = MaxHealth;
            _instance.currentHealth= MaxHealth;
        }



        #endregion
        #region PauseAction

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
            //Debug.Log(Global_CampaignData._playerSize.FinalValue);
            //������ �÷��̾��� ����� �� ���� �Ͽ��� �����.
            transform.localScale = (Global_CampaignData._playerSize.FinalValue * new Vector3(1, 1, 1));
        }


    }


    //앞으로 플레이어의 공격 배율과 관련된 데이터를 담단 하는 부분.
    [System.Serializable]
    public class Player_Status 
    {
        public DataEntity _damageData = new DataEntity(DataEntity.Type.Damage,10);
        public DataEntity _chargeData = new DataEntity(DataEntity.Type.ChargeGauge, 10);

        public Player_Status() 
        {
        }

    }

}
