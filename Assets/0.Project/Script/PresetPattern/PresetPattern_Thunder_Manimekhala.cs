using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Battle;
using PG.Event;

namespace PG 
{
    public sealed class PresetPattern_Thunder_Manimekhala : PresetPatternAction_Base
    {
        public override void StartPatternAction()
        {
            //베이스 패턴 꼭 넣어 주기.
            base.StartPatternAction();
            var lists = MobGenerator.GetMobList();
            var targetPos = Player_Script.GetPlayerPosition();

            for (int i = lists.Count - 1; i >= 0; i--)
            {
                lists[i].Damage(targetPos,100f);
            }
        }

    }

}
