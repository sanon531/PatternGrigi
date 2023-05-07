using System.Collections;
using System.Collections.Generic;
using PG.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PG
{
    public class InGameBattleSelectPannel : MonoBehaviour
    {
        public StartCondition thisCondition = StartCondition.None;
        [SerializeField] private Image _thisImage;
        [SerializeField] private TextMeshProUGUI _thisText;
        
        public void OnClickedButton()
        {
            MainSceneManager.SetStartCondition(thisCondition,_thisText.text,_thisImage.sprite);
        }
    }
}