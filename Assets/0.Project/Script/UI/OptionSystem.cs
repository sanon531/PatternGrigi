using PG.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PG.Battle
{
    public class OptionSystem : MonoBehaviour
    {
        [SerializeField]
        GameObject _pausePannel;
        [SerializeField]
        GameObject _pauseButton;
        [SerializeField]
        Slider _backgroundSoundSlider;
        [SerializeField]
        Slider _effectSoundSlider;


        bool _isPannelshow = false;

        private void Start()
        {
            _backgroundSoundSlider.value = AudioManager._instance.GetBackgroundVolume();
            _effectSoundSlider.value = AudioManager._instance.GetEffectVolume();
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
                _pauseButton.SetActive(false);
            }

        }

        public void GotoMainMenu()
        {
            //화면 끄고
            CallPausePanel();
            //메인으로 이동
            Global_BattleEventSystem.CallOnTouchMain();
            SceneMoveManager.MoveSceneByCall("Main_Scene");
        }

        public void SetBackgroundVolume(float volume)
        {
            AudioManager._instance.ChangeBackgroundVolume(volume);
        }
        public void SetEffectVolume(float volume)
        {
            AudioManager._instance.ChangeEffectVolume(volume);
        }

        public void MuteBackgroundSound(bool value)
        {
            AudioManager._instance.MuteBackgroundVolume(value);
        }
        public void MuteEffectSound(bool value)
        {
            AudioManager._instance.MuteEffectVolume(value);
        }

    }
}