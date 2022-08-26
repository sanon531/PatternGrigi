using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;
using PG.Data;
using DG.Tweening;
using System.Linq;
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
        }
        protected override void CallOnDestroy()
        {
        }


        //여기서
        public static void LevelUpPannelOn()
        {
            _instance._panelBG.enabled = true;
            foreach (LevelupChooseButtonScript i in _instance._upgradePanelList)
            {
                i.transform.DOScale(0.8f, 0.5f);
                i.SetActiveButton(true);
            }
        }

        public static void LevelUpPannelOff()
        {

            _instance._panelBG.enabled = false;
            foreach (LevelupChooseButtonScript i in _instance._upgradePanelList)
            {
                i.transform.DOScale(0, 0.5f);
                i.SetActiveButton(false); ;
            }
        }


        //새 랜덤 아이템들이 창으로 올라오도록 만들기.
        public static void SetRandomItemOnPannel(List<ArtifactID> artifactIDs)
        {
            _instance._upgradeDataList = artifactIDs.ToList();
            int i = 0;
            //지금은 그냥 배치일뿐 나중에는 확률에따른 무작위로 만들자.
            foreach (LevelupChooseButtonScript script in _instance._upgradePanelList)
            {
                script.SetTextAndImageOnButton(artifactIDs[i]);
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
