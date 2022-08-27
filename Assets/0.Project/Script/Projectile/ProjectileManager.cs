using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using PG.Event;

namespace PG.Battle 
{
    public class ProjectileManager : MonoSingleton<ProjectileManager>
    {
        Dictionary<ProjectileID, GameObject> _projectileDictionary;
        //플레이어가
        [SerializeField]
        int _targetEnemyCount = 1;
        protected override void CallOnAwake()
        {
            Global_BattleEventSystem._onCalcDamage += SetProjectileToEnemy;
            InitiallzeProjectileDic();
        }
        protected override void CallOnDestroy()
        {
            Global_BattleEventSystem._onCalcDamage -= SetProjectileToEnemy;
        }
        // 리소스에서 로드하여 딕셔너리에 저장한다음 
        void InitiallzeProjectileDic() 
        {
            //먼저 몹 스폰 매니져에게서 적들의 데이터에 관한 값을 가져오게 됨.
            //적들의 데이터에 관한 것이면 적들의
        
        }


        //
        void SetProjectileToEnemy(float val) 
        {

        }

        void SetTargetCount(int val) 
        {
            _targetEnemyCount = val;
        }





    }

}
