using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace PG 
{
    public class SceneMoveManager : MonoSingleton<SceneMoveManager>
    {
        public string targetPlayScene;

        public static void MoveSceneByCall(string name) 
        {
            SceneManager.LoadScene(name);
        }

    }

}
