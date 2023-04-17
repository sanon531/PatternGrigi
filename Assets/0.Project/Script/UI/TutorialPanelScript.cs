using PG.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PG
{
    public class TutorialPanelScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject Pages;

        private void Awake()
        {
            MultiSceneUIScript.PublicFadeOut();
            
            for(int i = 0; i < Pages.transform.childCount; i++)
            {
                Rect page = Pages.transform.GetChild(i).GetComponent<RectTransform>().rect;
                page.width = Screen.width;
                page.height = Screen.height;
            }
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
                MultiSceneUIScript.PublicFadeIn();
                StartCoroutine(DelayedChangeScene(SceneMoveManager._instance.targetPlayScene));
            }
        }

        IEnumerator DelayedChangeScene(string targetScene)
        {
            yield return new WaitForSecondsRealtime(1.25f);
            SceneMoveManager.MoveSceneByCall(targetScene);
            //첫 플레이 일때만 자동으로 꺼주기
            if (SaveDataManager._instance.saveData.FirstPlay)
            {
                SaveDataManager._instance.saveData.ShowTutorial = false;
                SaveDataManager._instance.saveData.FirstPlay = false;
            }
        }
    }
}