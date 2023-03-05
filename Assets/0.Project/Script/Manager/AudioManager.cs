using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
namespace PG
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField]
        StringAudioDictionary _stringAudioDict = new StringAudioDictionary();
        [SerializeField]
        AudioSource _backgroundmusic;
        [SerializeField]
        List<AudioSource> _audioList = new List<AudioSource>();
        [SerializeField]
        List<AudioSource> _effectSoundList = new List<AudioSource>();

        [SerializeField] private List<AudioClip> _bgmList = new List<AudioClip>();

        // Start is called before the first frame update
        void Start()
        {

        }

        public float GetBackgroundVolume()
        {
            return _backgroundmusic.volume;
        }

        public float GetEffectVolume()
        {
            if(_effectSoundList.Count != 0)
            {
                return _effectSoundList[0].volume;
            }
            else
            {
                return 0;
            }
        }
        
        public static void ChangeBackgroundMusicOnSceneChange(int target)
        {
            _instance._backgroundmusic.DOFade(0, 1f);
            _instance.StartCoroutine(_instance.DelayedChange(target));
        }

        IEnumerator DelayedChange(int target)
        {
            yield return new WaitForSeconds(1f);
            _backgroundmusic.clip = _bgmList[target];
            _backgroundmusic.DOFade(1, 0.5f);
            _backgroundmusic.Play();
        }


        public void ChangeBackgroundVolume(float value) 
        {
            _backgroundmusic.volume = value;
        }

        public void ChangeEffectVolume(float value)
        {
            foreach(AudioSource source in _effectSoundList) 
            {
                source.volume = value;
            }
        }

        public void MuteBackgroundVolume(bool value)
        {
            _backgroundmusic.mute = value;
        }
        public void MuteEffectVolume(bool value)
        {
            foreach (AudioSource source in _effectSoundList)
            {
                source.mute = value;
            }
        }

        public static void CallSFX(string _string)
        {
        }
    }

}
