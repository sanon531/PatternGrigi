using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using PG.Data;
namespace PG.Battle
{
    //레벨업, 아이템 얻었을때 나오는 창.
    public class LevelupChooseButtonScript : MonoBehaviour
    {
        [SerializeField]
        int _buttonNum=0;
        [SerializeField]
        TextMeshProUGUI _title;
        [SerializeField]
        TextMeshProUGUI _content;
        [SerializeField]
        Image _contentImage;
        Button _button;
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(CallButton);
        }

        public void SetActiveButton(bool val)
        {
            _button.interactable = val;
        }

        void CallButton() 
        {
            LevelUpPanelScript.GetButtonPressed(_buttonNum);
        }
        public void SetTextAndImageOnButton(ArtifactID id) 
        {
            _contentImage.sprite = Resources.Load<Sprite>("Artifact/" + id.ToString());
            ArtifactData data= GlobalDataStorage.TotalArtifactTableDataDic[id];
            _content.text = data.Devcomment;
        
        }



    }
}