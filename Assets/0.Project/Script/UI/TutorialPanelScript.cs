using PG.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PG
{
    public class TutorialPanelScript : MonoBehaviour
    {
        private void Awake()
        {
            if (GlobalUIEventSystem._isTotalFade)
                GlobalUIEventSystem.CallTotalFade();
        }

        public void MovePage(GameObject targetPanel)
        {
            targetPanel.SetActive(true);
            EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
        }

        bool _pressedStart = false;
        // Start is called before the first frame update
        public void StartGame()
        {
            if (!_pressedStart)
            {
                _pressedStart = true;
                GlobalUIEventSystem.CallTotalFade();
                StartCoroutine(DelayedChangeScene(SceneMoveManager._instance.targetPlayScene));
            }
        }

        IEnumerator DelayedChangeScene(string targetScene)
        {
            yield return new WaitForSecondsRealtime(1.25f);
            SceneMoveManager.MoveSceneByCall(targetScene);
        }
    }
}