using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;

namespace PG.Battle 
{

    public class ArtifactExample : MonoBehaviour
    {
        void Start()
        {
            Global_BattleEventSystem._onCalcDamage += SetDamageAlert;
        }

        private void OnDestroy()
        {
            Global_BattleEventSystem._onCalcDamage -= SetDamageAlert;
        }

        void SetDamageAlert(float val)
        {
            ShowDebugtextScript.SetDebug("LEL DAMAGE SIR : " + val);

        }

    }





}

