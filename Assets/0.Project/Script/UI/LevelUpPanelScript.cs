using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;
using PG.Data;
using DG.Tweening;
namespace PG.Battle 
{
    public class LevelUpPanelScript : MonoSingleton<LevelUpPanelScript>
    {
        [SerializeField]
        Image _panelBG;

        [SerializeField]
        List<Button> _upgradePanelList;

        [SerializeField]
        List<ArtifactID> _upgradeDataList = new List<ArtifactID>() { ArtifactID.FragileRush, ArtifactID.FragileRush, ArtifactID.FragileRush, ArtifactID.FragileRush };


        // Start is called before the first frame update
        protected override void CallOnAwake()
        {
            Global_BattleEventSystem._onLevelUpShow += SetLevelUpOn;
            Global_BattleEventSystem._onLevelUpHide += SetLevelUpOff;
        }
        protected override void CallOnDestroy()
        {
            Global_BattleEventSystem._onLevelUpShow -= SetLevelUpOn;
            Global_BattleEventSystem._onLevelUpHide -= SetLevelUpOff;
        }


        //여기서
        public void SetLevelUpOn()
        {
            _panelBG.enabled = true;
            foreach(Button i in _upgradePanelList) 
            {
                i.transform.DOScale(1, 0.5f);
                i.interactable = true;
            }
            SetRandomItemOnPannel();
        }

        public void SetLevelUpOff()
        {
            _panelBG.enabled = false;
            foreach (Button i in _upgradePanelList)
            {
                i.transform.DOScale(0, 0.5f);
                i.interactable = false;
            }
        }


        //새 랜덤 아이템들이 창으로 올라오도록 만들기.
        void SetRandomItemOnPannel() 
        {
            //지금은 그냥 무작위지만 나중에는 확률에따른 무작위로 만들자.
        
        }


        //선택을 
        public static void GetButtonPressed(int buttonNum) 
        {
            Debug.Log(buttonNum);
            ArtifactManager.AddArtifactToPlayer_tempUse(_instance._upgradeDataList[buttonNum]);
            Global_BattleEventSystem.CallOffLevelUp();
        }

    }


}
