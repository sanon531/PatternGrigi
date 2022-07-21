using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager _instance;
        [SerializeField]
        StringAudioDictionary _stringAudioDict = new StringAudioDictionary();
        [SerializeField]
        AudioSource _backgroundmusic;
        [SerializeField]
        List<AudioSource> _audioList = new List<AudioSource>();

        // Start is called before the first frame update
        void Start()
        {

        }

        public static void CallSFX(string _string)
        {
        }
    }

}
