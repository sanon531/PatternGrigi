using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMoveUndestructable_onlyOne : MonoBehaviour
{
    public static SceneMoveUndestructable_onlyOne _instance;
    public bool _aaa = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null )
        {
            if (_instance != this)
            {
                Destroy(gameObject);
                Debug.Log("a");
            }
        }
        else 
        {
            //Debug.Log("aad");
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    void Start()
    {
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
