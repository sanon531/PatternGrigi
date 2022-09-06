using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Battle;
using PG.Event;

namespace PG 
{
    public sealed class PresetPattern_DefaultThunder : PresetPatternAction_Base
    {
        public override void StartPatternAction()
        {
            //���̽� ���� �� �־� �ֱ�.
            base.StartPatternAction();
            var lists = MobGenerator.GetMobList();
            for (int i = lists.Count - 1; i >= 0; i--)
            {
                lists[i].Damage(100f);
            }
        }

    }

}