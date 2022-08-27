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
        //�÷��̾
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
        // ���ҽ����� �ε��Ͽ� ��ųʸ��� �����Ѵ��� 
        void InitiallzeProjectileDic() 
        {
            //���� �� ���� �Ŵ������Լ� ������ �����Ϳ� ���� ���� �������� ��.
            //������ �����Ϳ� ���� ���̸� ������
        
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
