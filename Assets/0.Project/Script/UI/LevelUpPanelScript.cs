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
            Global_BattleEventSystem._on레벨업일시정지 += SetLevelUpPauseOn;
            Global_BattleEventSystem._off레벨업일시정지 += SetLevelUpPauseOff;
        }
        private void OnDestroy()
        {
            Global_BattleEventSystem._on레벨업일시정지 -= SetLevelUpPauseOn;
            Global_BattleEventSystem._off레벨업일시정지 -= SetLevelUpPauseOff;
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
            Global_BattleEventSystem.Call레벨업일시정지();
        }



    }


}
