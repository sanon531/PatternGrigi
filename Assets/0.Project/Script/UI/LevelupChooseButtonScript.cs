using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PG.Data;
using DG.Tweening;
namespace PG.Battle
{
    //레벨업, 아이템 얻었을때 나오는 창.
    public class LevelupChooseButtonScript : MonoBehaviour
    {
        [SerializeField]
        int _buttonNum = 0;
        [SerializeField]
        TextMeshProUGUI _title;
        [SerializeField]
        TextMeshProUGUI _content;
        [SerializeField]
        Image _contentImage;
        [SerializeField]
        Image _selectedImage;

        Button _button;
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(CallButton);
        }

        public void SetInterectiveButton(bool val)
        {
            _button.interactable = val;
        }

        void CallButton()
        {
            _selectedImage.enabled = true;
            LevelUpPanelScript.GetButtonPressed(_buttonNum);
        }
        public void SetTextAndImageOnButton(ArtifactID id)
        {
            _contentImage.sprite = Resources.Load<Sprite>("Artifact/" + id.ToString());
            //ArtifactData data= GlobalDataStorage.TotalArtifactTableDataDic[id];
            _title.text = GetLocalizedTextScript.GetArtifactDataFromJson(ArtifactJsonData.ArtifactName, id);
            _content.text = GetLocalizedTextScript.GetArtifactDataFromJson(ArtifactJsonData.ArtifactEffect, id);

        }
        public void SetActivateButton()
        {
            _selectedImage.enabled = true;
            transform.DOScale(1, 0.25f).SetUpdate(UpdateType.Late, true);
            transform.DOPunchRotation(new Vector3(0,0,10),0.25f).SetUpdate(UpdateType.Late, true);
        }
        public void SetInactivateButton()
        {
            _selectedImage.enabled = false;
            transform.DOScale(0.8f, 0.25f).SetUpdate(UpdateType.Late, true);

        }
        IEnumerator CheckPress()
        {
            yield return new WaitForSeconds(1f);
            // 여기서 관련한 정보가 보여진다.
        
        }

    }
}