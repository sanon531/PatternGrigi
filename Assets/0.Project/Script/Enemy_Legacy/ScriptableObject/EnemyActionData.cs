using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Battle;
namespace PG.Data
{
    [CreateAssetMenu(menuName = "PG/EnemyActionData")]
    [System.Serializable]
    public class EnemyActionData_Scriptable: ScriptableObject
    {
        public EnemyActionID _enemyActionID;
        public EnemyActionData _data;
    }
}