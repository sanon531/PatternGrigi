using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PG.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveAlertPanelScript : MonoBehaviour
{

    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float panelDuration;
    
    private void Awake()
    {
        Global_BattleEventSystem._onWaveChange += showAlertPanel;
    }

    private void OnDestroy()
    {
        Global_BattleEventSystem._onWaveChange -= showAlertPanel;
    }

    private void showAlertPanel(int nowWaveOrder)
    {
        if (nowWaveOrder == 0) return;
        
        background.DOFade(1.0f, 0.8f);
        text.DOFade(1.0f, 0.8f);

        StartCoroutine(alertDelay());
        
    }

    IEnumerator alertDelay()
    {
        yield return new WaitForSeconds(panelDuration);
        
        background.DOFade(0.0f, 0.8f);
        text.DOFade(0.0f, 0.8f);
    }
}
