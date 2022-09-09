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
        string _targetScene = "Play_Scene";

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
                GlobalUIEventSystem.CallTotalFade();
                Debug.Log("pressed");
                _pressedStart = true;
                StartCoroutine(DelayedChangeScene());
            }
        }
        IEnumerator DelayedChangeScene() 
        {
            yield return new WaitForSecondsRealtime(1.25f);
            SceneMoveManager.MoveSceneByCall(_targetScene);
            Debug.Log("called");

        }
    }

}
