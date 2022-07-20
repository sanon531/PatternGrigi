using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternNodeScript : MonoBehaviour
{
    //도달이 가능한 상황일 때
    [SerializeField]
    int _nodeId;

    [SerializeField]
    bool _isReachable = false;
    [SerializeField]
    SpriteRenderer _reachableImage;
    [SerializeField]
    AudioSource _swipeAudio;
    [SerializeField]
    ParticleSystem _flash;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIsReachable(bool _bool) 
    {
        _isReachable = _bool;
        _reachableImage.enabled = _bool;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShowDebugtextScript._instance.SetDebug(name+"Player in _11"+ collision.transform.tag);
        if (collision.transform.tag == "Player")
        {
            if (_isReachable)
            {
                _swipeAudio.Play();
                //ShowDebugtextScript._instance.SetDebug("Player in" + name );
                PatternManager.DamageCall(_nodeId);
                _flash.Play();
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {



    }
}
