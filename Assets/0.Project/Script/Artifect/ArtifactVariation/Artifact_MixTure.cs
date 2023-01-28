using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
using PG.Data;

namespace PG
{

    public class Artifact_Mix_PadSesame : Artifact 
    {
        public Artifact_Mix_PadSesame() : base(ArtifactID.Mix_PadSesame)
        {
            
        }

        public override void OnGetArtifact()
        {
            base.OnGetArtifact();
            Global_CampaignData._playerSize.Add증가량(5f);
            Global_CampaignData._charactorAttackDic[CharacterID.Player].Add증가량(10f);
            Global_BattleEventSystem.CallOnSizeChanged();
        }
    }

}