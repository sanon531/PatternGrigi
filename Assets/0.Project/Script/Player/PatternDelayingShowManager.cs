using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Data;
using TMPro;
using RengeGames.HealthBars;
namespace PG.Battle
{
    //그저 표현만을 담당하는 스크립트.
    public class PatternDelayingShowManager : MonoBehaviour
    {

        [SerializeField]
        UltimateCircularHealthBar _majorLateBar;
        [SerializeField]
        TextMeshProUGUI _waittext;

        [SerializeField]
        GridLayoutGroup _settingGrid;
        [SerializeField]
        List<UltimateCircularHealthBar> _listCircularBarList = new List<UltimateCircularHealthBar>();
        int _currentCount = 0;
        public void InitialzeShowManager()
        {
            _settingGrid.cellSize = new Vector2(Screen.width * 0.15f, Screen.width * 0.15f);
            Debug.Log(Screen.width * 0.15f);
        }

        bool _activatedMajorDelayBar = true;
        void ActivateMajorDelayBar(bool val)
        {
            _majorLateBar.enabled = val;
            _majorLateBar.GetComponent<Image>().enabled = val;
            _activatedMajorDelayBar = val;
            _waittext.enabled = val;
        }
        private void Start()
        {
            _waittext.text = GetLocalizedTextScript.GetUIDataFromJson(UITextID.Battle_PatternDelay);
        }

        int _beforeNum = 0; 
        //현재 차지 갯수와 차지 정도
        public void SetValueofDelay(float val, int count)
        {
            if (count == 0)
            {
                if (!_activatedMajorDelayBar)
                {
                    ActivateMajorDelayBar(true);
                }
                if (_beforeNum != count) 
                {
                    foreach (UltimateCircularHealthBar bar in _listCircularBarList)
                        bar.SetPercent(0);
                    _beforeNum = count;
                }
                _majorLateBar.SetPercent(val);
            }
            else
            {
                //Debug.Log(val + ":" + count);
                if (_activatedMajorDelayBar)
                {
                    ActivateMajorDelayBar(false);
                }


                if (_beforeNum != count)
                {
                    _beforeNum = count;
                    for (int i = 0; i< _listCircularBarList.Count;i++) 
                    {
                        if (i < count - 1)
                            _listCircularBarList[i].SetPercent(1);
                        else
                            _listCircularBarList[i].SetPercent(0);
                    }
                }
                _listCircularBarList[count- 1].SetPercent(val);


            }


        }




    }

}
