using PG.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace PG.Battle
{
    public class OptionSystem :MonoSingleton<OptionSystem>
    {
        [SerializeField]
        private GameObject _pausePannel;
        [SerializeField]
        private GameObject _pauseButton;
        [SerializeField]
        private Slider _backgroundSoundSlider;
        [SerializeField]
        private Slider _effectSoundSlider;
        [SerializeField]
        private Button _gotoMenuButton;
        [SerializeField]
        private Toggle _tutorialToggle;


        [SerializeField] 
        private AudioMixer _audioMixer;
        
        bool _isPannelshow = false;
        private bool _isInMainMenu = true;
        private void Start()
        {
            _audioMixer.GetFloat("MusicVolume",out float bgmVol);
            _backgroundSoundSlider.value = Mathf.Pow(10, (bgmVol/20));
            
            _audioMixer.GetFloat("MusicVolume",out float effectVol);
            _effectSoundSlider.value = Mathf.Pow(10, (effectVol/20));
        }

        public void CallPausePanel()
        {
            if (Global_BattleEventSystem._isTotalPaused != _isPannelshow)
            {
                Debug.LogError("UnmatchedCall");
                return;
            }
            Global_BattleEventSystem.CallTotalPauseSwitch();
            if (_isPannelshow)
            {
                _pausePannel.transform.position = new Vector3(10000, 0, 0);
                _isPannelshow = false;
                _pauseButton.SetActive(true);
            }
            else
            {
                int i_width = Screen.width;
                int i_height = Screen.height;
                _pausePannel.transform.position = new Vector3(i_width / 2, i_height / 2, 0);
                _isPannelshow = true;
                _tutorialToggle.isOn = SaveDataManager._instance.saveData.ShowTutorial;
                _pauseButton.SetActive(false);
            }

        }

        public static void SetGotoPlayScene()
        {
            _instance._isInMainMenu = false;
            _instance._gotoMenuButton.interactable = true;
        }
        public static void SetGotoMainMenu()
        {
            _instance._isInMainMenu = true;
            _instance._gotoMenuButton.interactable = false;
        }

        public void GotoMainMenu()
        {
            if (_isInMainMenu)
                return;
            _isInMainMenu = true;
            //화면 끄고
            CallPausePanel();
            //메인으로 이동
            Global_BattleEventSystem.CallOnTouchMain();
            GlobalUIEventSystem.CallTotalFade();
            StartCoroutine(DelayedToMenu());
            AudioManager.ChangeBackgroundMusicOnSceneChange(0);
        }

        public void SetBackgroundVolume(float volume)
        {
            //-80~0 -> 0.0001~1
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        }
        public void SetEffectVolume(float volume)
        {
            _audioMixer.SetFloat("EffectVolume", Mathf.Log10(volume) * 20);
        }
        
        IEnumerator DelayedToMenu() 
        {
            yield return new WaitForSecondsRealtime(1.25f);
            SceneMoveManager.MoveSceneByCall("Main_Scene");
        }

        public void OnClickedTutorialToggle()
        {
            SaveDataManager._instance.saveData.ShowTutorial = _tutorialToggle.isOn;
        }

        
    }
}