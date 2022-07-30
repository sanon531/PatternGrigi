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
        TextAnimatorPlayer _gameOverTextPlayer;
        [SerializeField]
        GameObject _buttonSet;
        // Start is called before the first frame update
        void Start()
        {
            Global_BattleEventSystem._on게임오버 += StartGameOverScene;
            _backGround.enabled = false;
            _gameOverText.enabled = false;
            _buttonSet.SetActive(false);

        }

        void StartGameOverScene()
        {
            _backGround.enabled = true;
            _gameOverText.enabled = true;
            _gameOverTextPlayer.ShowText("게임오버");
            _backGround.DOFade(1f, 1f);
            StartCoroutine(Delayed());
        }

        IEnumerator Delayed() 
        {
            yield return new WaitForSecondsRealtime(2f);
            _buttonSet.SetActive(true);
        }


        public void CallReloadScene() 
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameObject.scene.name);
        }

        public void CallTurnBacktoScene() 
        {
        
        
        }


    }

}
