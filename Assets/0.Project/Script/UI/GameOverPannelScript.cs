using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;
using DG.Tweening;
using TMPro;
using Febucci.UI;
namespace PG.Battle 
{
    public class GameOverPannelScript : MonoBehaviour
    {
        [SerializeField]
        Image _backGround;
        [SerializeField]
        TextMeshProUGUI _gameOverText;
        [SerializeField]
        GameObject _buttonSet;
        // Start is called before the first frame update
        void Start()
        {
            Global_BattleEventSystem._onGameOver += StartGameOverScene;
            _backGround.enabled = false;
            _gameOverText.enabled = false;
            _buttonSet.SetActive(false);
            _isBackScene = false;
        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
                Player_Script.InstantKill();
        }

        private void OnDestroy()
        {
            Global_BattleEventSystem._onGameOver -= StartGameOverScene;

        }

        void StartGameOverScene()
        {
            _backGround.enabled = true;
            _gameOverText.enabled = true;
            _backGround.DOFade(1f, 1f);
            StartCoroutine(Delayed());
        }

        IEnumerator Delayed() 
        {
            yield return new WaitForSecondsRealtime(2f);
            _buttonSet.SetActive(true);
        }


        bool _isBackScene = false;

        public void CallReloadScene() 
        {
            if (!_isBackScene) 
            {
                //GlobalUIEventSystem.CallTotalFade();
                MultiSceneUIScript.PublicFadeIn();
                StartCoroutine(DelayedMove(gameObject.scene.name));
                AudioManager.ChangeBackgroundMusicOnSceneChange(1);
                _isBackScene = true;
            }
        }


        public void CallTurnBacktoScene() 
        {
            if (!_isBackScene)
            {
                //GlobalUIEventSystem.CallTotalFade();
                MultiSceneUIScript.PublicFadeIn();
                StartCoroutine(DelayedMove("Main_Scene"));
                AudioManager.ChangeBackgroundMusicOnSceneChange(0);

                _isBackScene = true;
            }
        }

        IEnumerator DelayedMove(string scenename) 
        {
            yield return new WaitForSecondsRealtime(1.25f);
            SceneMoveManager.MoveSceneByCall(scenename);
        }


    }

}
