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
        Image _levelTitle;
        [SerializeField]
        GridLayoutGroup _gridlayout;

        [SerializeField]
        List<LevelupChooseButtonScript> _upgradePanelList;

        [SerializeField]
        List<ArtifactID> _upgradeDataList = new List<ArtifactID>() { ArtifactID.FragileRush, ArtifactID.FragileRush, ArtifactID.FragileRush, ArtifactID.FragileRush };


        // Start is called before the first frame update
        protected override void CallOnAwake()
        {
            _gridlayout.cellSize = new Vector2(Screen.height * 0.2f, Screen.width * 0.45f);
            _gridlayout.spacing = new Vector2(Screen.height * 0.05f, Screen.width * 0.05f);
            _confirmedButton = GameObject.Find("LevelUpSelectionButton").GetComponent<Button>();
            _confirmedButton.onClick.AddListener(ConfirmButtonPressed);
            _confirmedButton.interactable = false;
            _instance._levelTitle.enabled = false;

        }
        protected override void CallOnDestroy()
        {
            _confirmedButton.onClick.RemoveAllListeners();
        }


        //여기서
        public static void LevelUpPannelOn()
        {
            _instance._panelBG.enabled = true;
            _instance._levelTitle.enabled = true;

            foreach (LevelupChooseButtonScript script in _instance._upgradePanelList)
            {
                script.ButtonCallOnOff(true);
            }
            _instance._confirmedButton.transform.DOScale(0.8f, 0.5f).SetUpdate(UpdateType.Late,true);
            _instance._confirmedButton.transform.DOShakeRotation(0.5f).SetUpdate(UpdateType.Late, true);

        }

        public static void LevelUpPannelOff()
        {
            _instance._panelBG.enabled = false;
            _instance._levelTitle.enabled = false;
            foreach (LevelupChooseButtonScript script in _instance._upgradePanelList)
            {
                script.ButtonCallOnOff(false);
            }
            _instance._confirmedButton.transform.DOScale(0f, 0.5f).SetUpdate(UpdateType.Late, true);
        }


        //새 랜덤 아이템들이 창으로 올라오도록 만들기.
        public static void SetRandomItemOnPannel(List<ArtifactID> artifactIDs)
        {
            _instance._upgradeDataList = artifactIDs.ToList();

            //선택을 할경우 동시에 선택과
            //
            int i = 0;
            foreach (LevelupChooseButtonScript script in _instance._upgradePanelList)
            {

                if (artifactIDs.Count() > i)
                {                
                    script.SetTextAndImageOnButton(artifactIDs[i]);
                }
                else
                {
                    if (i == 0)
                        script.SetTextAndImageOnButton(ArtifactID.Default_HealthUp);
                    else
                        script.SetDeactivateOnButton();
                }
                i++;
            }
        }


        //-1 이면 아예 선택이 안된거임
        [SerializeField]
        Button _confirmedButton;
        [SerializeField]
        int _choosedButtonNum = -1;
        //선택을 
        public static void GetButtonPressed(int buttonNum)
        {
            _instance._choosedButtonNum = buttonNum;
            _instance._confirmedButton.interactable = true;
            for (int i = 0; i < _instance._upgradePanelList.Count; i++)
            {
                if (buttonNum != i)
                    _instance._upgradePanelList[i].SetInactivateButton();
                else
                    _instance._upgradePanelList[i].SetActivateButton();
            }

        }

        void ConfirmButtonPressed()
        {
            if (_choosedButtonNum > -1)
            {
                _instance._confirmedButton.interactable = false;
                ArtifactManager.AddArtifactToPlayer_tempUse(_instance._upgradeDataList[_choosedButtonNum]);
                _choosedButtonNum = -1;
                Global_BattleEventSystem.CallOffLevelUp();
                for (int i = 0; i < _instance._upgradePanelList.Count; i++)
                {
                    _instance._upgradePanelList[i].SetInactivateButton();
                }

            }
            else
            {
                Debug.LogError("Error: Confirm Button is activated without Selection of Artifact");
            }


        }


    }


}
