using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDebugtextScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static ShowDebugtextScript _instance;
    Text _debugshower;
    void Start()
    {
        _debugshower = GetComponent<Text>();
        _instance = this;
    }


    public static void SetDebug(string _str) 
    {
        _instance._debugshower.text = "_";
        _instance._debugshower.text = _str;
    }

}

