using System.Collections;
using System.Collections.Generic;
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
