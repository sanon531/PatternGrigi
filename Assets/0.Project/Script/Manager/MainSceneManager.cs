using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PG.Battle;
using PG.Data;
using UnityEngine;
using PG.Event;
using TMPro;
using UnityEngine.UI;

namespace PG
{
    public class MainSceneManager : MonoSingleton<MainSceneManager>
    {
        [SerializeField] TextMeshProUGUI _titleText;
        [SerializeField] TextMeshProUGUI _gameStartText;


        [SerializeField] private RectTransform _selectPanel;
        [SerializeField] private Vector3 _selectPanelPos = new Vector3();

        [SerializeField] private Button startButton;

        private StartCondition currentStartCondition = StartCondition.None;


        [SerializeField] Image _currentSelectedImage;

        [SerializeField] TextMeshProUGUI _selectText;


        public static void SetStartCondition(StartCondition val,string valtext, Sprite targetImage )
        {
            _instance.currentStartCondition = val;
            _instance._selectText.SetText(valtext);
            _instance.startButton.interactable = true;
            _instance._currentSelectedImage.sprite = targetImage;
            CurrentSelectedDataManager.SetCurrentCondition(val);
        }


        private void Start()
        {
            MultiSceneUIScript.PublicFadeOut();
            _selectPanelPos = _selectPanel.anchoredPosition;
            startButton.interactable = false;
        }

        bool _pressedStart = false;
        // Start is called before the first frame update


        public void ShowSelectionPanel()
        {
            _selectPanel.DOAnchorPos(Vector2.zero, 0.5f);
        }

        public void HideSelectionPanel()
        {
            _selectPanel.DOAnchorPos(_selectPanelPos, 0.5f);
        }


        public void StartGame()
        {
            if (!_pressedStart && MultiSceneUIScript.GetIsFadeEnd())
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