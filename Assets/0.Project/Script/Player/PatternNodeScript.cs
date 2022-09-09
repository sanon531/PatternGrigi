using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PG.Battle
{
    public class PatternNodeScript : MonoBehaviour
    {
        //도달이 가능한 상황일 때
        [SerializeField]
        int _nodeId;

        [SerializeField]
        bool _isReachable = false;
        [SerializeField]
        AudioSource _swipeAudio;
        [SerializeField]
        ParticleSystem _flash;

        private void Start()
        {
        }

        public void SetIsReachable(bool active)
        {
            _isReachable = active;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //ShowDebugtextScript.SetDebug(name + "Player in _11" + collision.transform.tag);
            if (collision.transform.tag == "Player")
            {
                if (_isReachable)
                {
                    _swipeAudio.Play();
                    _isReachable = false;
                    //ShowDebugtextScript._instance.SetDebug("Player in" + name );
                    PatternManager.DamageCallWhenNodeReach(_nodeId);
                    _flash.Play();
                }
            }

        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.transform.tag == "Player")
            {
                if (_isReachable)
                {
                    _swipeAudio.Play();
                    _isReachable = false;
                    //ShowDebugtextScript._instance.SetDebug("Player in" + name );
                    PatternManager.DamageCallWhenNodeReach(_nodeId);
                    _flash.Play();
                }
            }
        }

    }
}
