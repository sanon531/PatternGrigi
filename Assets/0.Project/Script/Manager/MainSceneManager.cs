using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;

namespace PG 
{
    public class MainSceneManager : MonoBehaviour
    {

        private void Start()
        {
            if (GlobalUIEventSystem._is암전)
                GlobalUIEventSystem.CallOn암전스위치();

        }


        bool _pressedStart = false;
        // Start is called before the first frame update
        public void StartGame() 
        {
            if (!_pressedStart) 
            {
                GlobalUIEventSystem.CallOn암전스위치();
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
