using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;
using DG.Tweening;
namespace PG.Battle 
{
    public class LevelUpPanelScript : MonoBehaviour, ISetNontotalPause
    {
        [SerializeField]
        Image _panelBG;

        [SerializeField]
        List<GameObject> _upgradePanelList;

        // Start is called before the first frame update
        void Start()
        {
            Global_BattleEventSystem._onNonTotalPause += SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause += SetNonTotalPauseOff;
        }
        private void OnDestroy()
        {
            Global_BattleEventSystem._onNonTotalPause -= SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause -= SetNonTotalPauseOff;
        }
        public void SetNonTotalPauseOn()
        {
            _panelBG.enabled = true;
            foreach(GameObject i in _upgradePanelList) 
            {
                i.transform.DOScale(1, 0.5f);
                i.GetComponent<Button>().interactable = true;
            }
        }

        public void SetNonTotalPauseOff()
        {
            _panelBG.enabled = false;

            foreach (GameObject i in _upgradePanelList)
            {
                i.transform.DOScale(0, 0.5f);
                i.GetComponent<Button>().interactable = false;

            }
        }

        public void GetButtonPressed() 
        {
            Global_BattleEventSystem.CallNonTotalPause();
        }



    }


}
