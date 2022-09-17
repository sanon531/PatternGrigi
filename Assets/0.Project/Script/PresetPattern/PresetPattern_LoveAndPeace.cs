using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Battle;
using PG.Event;

namespace PG 
{
    public sealed class PresetPattern_LoveAndPeace: PresetPatternAction_Base
    {
        public override void StartPatternAction()
        {
            base.StartPatternAction();

            Player_Script.Damage(-40f);

        }

    }

}
