using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using PG.Data;
namespace PG.Battle
{
    //������, ������ ������� ������ â.
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

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(CallButton);
        }

        void CallButton() 
        {
            LevelUpPanelScript.GetButtonPressed(_buttonNum);
        }
        void SetTextAndImageOnButton(ArtifactID id) 
        {
            _contentImage.sprite = Resources.Load<Sprite>("Artifact/" + id.ToString());
            ArtifactData data= GlobalDataStorage.TotalArtifactTableDataDic[id];
            _content.text = data.Devcomment;
        
        }



    }
}