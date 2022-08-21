using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;

namespace PG 
{
    public class MainSceneManager : MonoSingleton<MainSceneManager>
    {

        protected override void CallOnAwake()
        {
            if (GlobalUIEventSystem._isTotalFade)
                GlobalUIEventSystem.CallTotalFade();

        }


        bool _pressedStart = false;
        // Start is called before the first frame update
        public void StartGame() 
        {
            if (!_pressedStart) 
            {
                GlobalUIEventSystem.CallTotalFade();
                Debug.Log("pressed");
                _pressedStart = true;
                StartCoroutine(DelayedChangeScene());
            }
        }
        IEnumerator DelayedChangeScene() 
        {
            yield return new WaitForSecondsRealtime(1.25f);
            SceneMoveManager.MoveSceneByCall("Play_Scene");
            Debug.Log("called");

        }
    }

}
