using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
using TMPro;
namespace PG 
{
    public class MainSceneManager : MonoSingleton<MainSceneManager>
    {
        [SerializeField]
        TextMeshProUGUI _titleText;
        [SerializeField]
        TextMeshProUGUI _gameStartText;

        protected override void CallOnAwake()
        {
            if (GlobalUIEventSystem._isTotalFade)
                GlobalUIEventSystem.CallTotalFade();

        }
        private void Start()
        {
            _titleText.text = "<bounce>" + GetLocalizedTextScript.GetUIDataFromJson(Data.UITextID.Main_GameTitle) + "</>";
            _gameStartText.text = "<wave>" +GetLocalizedTextScript.GetUIDataFromJson(Data.UITextID.Main_GameStart) + "</>";
        }

        bool _pressedStart = false;
        // Start is called before the first frame update
        public void StartGame() 
        {
            if (!_pressedStart) 
            {
                _pressedStart = true;
                if (!SaveDataManager._instance.saveData.ShowTutorial)
                {
                    GlobalUIEventSystem.CallTotalFade();
                    StartCoroutine(DelayedChangeScene("Tutorial_Scene"));
                    SaveDataManager._instance.saveData.ShowTutorial = true;
                }
                else
                {
                    GlobalUIEventSystem.CallTotalFade();
                    StartCoroutine(DelayedChangeScene(SceneMoveManager._instance.targetPlayScene));
                }
            }
        }

        IEnumerator DelayedChangeScene(string targetScene) 
        {
            yield return new WaitForSecondsRealtime(1.25f);
            SceneMoveManager.MoveSceneByCall(targetScene);
            _pressedStart = false;
        }

    }

}
