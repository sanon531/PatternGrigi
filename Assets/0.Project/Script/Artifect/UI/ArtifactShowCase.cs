using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PG.Data;
namespace PG.Battle
{
    public class ArtifactShowCase : MonoBehaviour
    {
        [SerializeField]
        Image _artifactImage;
        [SerializeField]
        TextMeshProUGUI _CurrentCount;

        public void SetDataOnCase(int val)
        {
            _CurrentCount.SetText(val.ToString());
        }
        public void SetDataOnCase(ArtifactID id,int val) 
        {
            _artifactImage.sprite = ArtifactManager.GetSpriteFromImage(id);
            _CurrentCount.SetText(val.ToString());
        }
        public void SetDataOnCase(ArtifactID id)
        {
            _artifactImage.sprite = ArtifactManager.GetSpriteFromImage(id);
            //Debug.Log(_artifactImage.sprite);
        }


    }
}