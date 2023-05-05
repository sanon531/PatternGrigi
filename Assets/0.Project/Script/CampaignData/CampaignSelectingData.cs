using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Battle;
using System.Linq;
namespace PG.Data
{
    //그냥 해당하는버전을 여러개 만들어둘수있으면 좋을것 같아서 그럼
    [CreateAssetMenu(fileName = "Global_CampaignSelectingData", menuName = "PG/CampaignSelectingData")]
    public class CampaignSelectingData : ScriptableObject
    {
        public ArtifactID _startArtifact = ArtifactID.Default_Non;
        public ProjectileIDDataDic _projectileIDDataDic = new ProjectileIDDataDic();
    }



}