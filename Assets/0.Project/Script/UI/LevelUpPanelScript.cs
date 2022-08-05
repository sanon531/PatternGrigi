using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;
using DG.Tweening;
namespace PG.Battle 
{
    public class LevelUpPanelScript : MonoBehaviour, ISetLevelupPause
    {
        [SerializeField]
        Image _panelBG;

        [SerializeField]
        List<GameObject> _upgradePanelList;

        // Start is called before the first frame update
        void Start()
        {
            Global_BattleEventSystem._onLevelUpPause += SetLevelUpPauseOn;
            Global_BattleEventSystem._offLevelUpPause += SetLevelUpPauseOff;
        }
        private void OnDestroy()
        {
            Global_BattleEventSystem._onLevelUpPause -= SetLevelUpPauseOn;
            Global_BattleEventSystem._offLevelUpPause -= SetLevelUpPauseOff;
        }
        public void SetLevelUpPauseOn()
        {
            _panelBG.enabled = true;
            foreach(GameObject i in _upgradePanelList) 
            {
                i.transform.DOScale(1, 0.5f);
                i.GetComponent<Button>().interactable = true;
            }
        }

        public void SetLevelUpPauseOff()
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
            Global_BattleEventSystem.CallLevelUpPause();
        }



    }


}
