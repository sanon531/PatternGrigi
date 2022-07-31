using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace PG 
{
    public class SceneMoveManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        public static void MoveSceneByCall(string name) 
        {
            SceneManager.LoadScene(name);
        }

    }

}
