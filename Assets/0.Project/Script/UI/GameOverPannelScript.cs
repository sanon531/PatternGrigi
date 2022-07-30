using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;
using DG.Tweening;
using PG.Event;
public class GameOverPannelScript : MonoBehaviour
{

    Image _BackGround;

    // Start is called before the first frame update
    void Start()
    {
        Global_BattleEventSystem._on게임오버 += StartGameOverScene;
    }

    void StartGameOverScene() 
    {
    
    }



}
