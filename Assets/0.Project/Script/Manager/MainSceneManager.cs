using System.Collections;
using System.Collections.Generic;
using PG.Battle;
using UnityEngine;
using PG.Event;
using TMPro;
namespace PG 
{
    public class MainSceneManager : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _titleText;
        [SerializeField]
        TextMeshProUGUI _gameStartText;

        private void Start()
        {
            MultiSceneUIScript.PublicFadeOut();
        }

        bool _pressedStart = false;
        // Start is called before the first frame update
        public void StartGame() 
        {
            if (!_pressedStart) 
            {
                _pressedStart = true;
                if (SaveDataManager._instance.saveData.ShowTutorial)
                {
                    //GlobalUIEventSystem.CallTotalFade();
                    MultiSceneUIScript.PublicFadeIn();
                    StartCoroutine(DelayedChangeScene("Tutorial_Scene"));
                }
                else
                {
                    //GlobalUIEventSystem.CallTotalFade();
                    MultiSceneUIScript.PublicFadeIn();
                    StartCoroutine(DelayedChangeScene(SceneMoveManager._instance.targetPlayScene));
                }
            }
        }

        IEnumerator DelayedChangeScene(string targetScene) 
        {
            yield return new WaitForSecondsRealtime(1.25f);
            SceneMoveManager.MoveSceneByCall(targetScene);
            OptionSystem.SetGotoPlayScene();
            _pressedStart = false;
        }

    }

}
