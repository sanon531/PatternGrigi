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
        List<LevelupChooseButtonScript> _upgradePanelList;

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
            foreach (LevelupChooseButtonScript i in _upgradePanelList)
            {
                i.transform.DOScale(1, 0.5f);
                i.SetActiveButton(true);
            }
            //나중에 변경하자
            SetRandomItemOnPannel();
        }

        public void SetLevelUpOff()
        {
            _panelBG.enabled = false;
            foreach (LevelupChooseButtonScript i in _upgradePanelList)
            {
                i.transform.DOScale(0, 0.5f);
                i.SetActiveButton(false); ;
            }
        }


        //새 랜덤 아이템들이 창으로 올라오도록 만들기.
        void SetRandomItemOnPannel()
        {
            int i = 0;
            //지금은 그냥 배치일뿐 나중에는 확률에따른 무작위로 만들자.
            foreach (LevelupChooseButtonScript script in _upgradePanelList)
            {
                script.SetTextAndImageOnButton(_upgradeDataList[i]);
                i++;
            }
        }


        //선택을 
        public static void GetButtonPressed(int buttonNum)
        {
            //Debug.Log(buttonNum);
            ArtifactManager.AddArtifactToPlayer_tempUse(_instance._upgradeDataList[buttonNum]);
            Global_BattleEventSystem.CallOffLevelUp();
        }

    }


}
