using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using PG.Event;

namespace PG.Battle 
{
    //������Ʈ �Ŵ����� ���̿ܿ��� ����
    public class ProjectileManager : MonoSingleton<ProjectileManager>
    {
        //���߿� Ǯ���ϱ� ���ؼ� ���� ������ Ǯ�� �ʿ����.
        Dictionary<ProjectileID, GameObject> _projectileDictionary= new Dictionary<ProjectileID, GameObject>();
        Dictionary<ProjectileID, List<GameObject>> _activateProjectileDictionary = new Dictionary<ProjectileID, List<GameObject>>();
        Dictionary<ProjectileID, List<GameObject>> _deactivateProjectileDictionary = new Dictionary<ProjectileID, List<GameObject>>();

        //�÷��̾
        [SerializeField]
        int _targetEnemyCount = 1;
        [SerializeField]
        ProjectileID _currentProjectile = ProjectileID.NormalBullet;
        [SerializeField]
        List<GameObject> _temptenemyList = new List<GameObject>();


        protected override void CallOnAwake()
        {
            Global_BattleEventSystem._onCalcPlayerAttack += SetProjectileToEnemy;
            InitiallzeProjectileDic();
        }
        protected override void CallOnDestroy()
        {
            Global_BattleEventSystem._onCalcPlayerAttack -= SetProjectileToEnemy;
        }

        // ���ҽ����� �ε��Ͽ� �ΰ��� ��ųʸ��� �����Ѵ�.
        void InitiallzeProjectileDic() 
        {
            //���� �� ���� �Ŵ������Լ� ������ �����Ϳ� ���� ���� �������� ��.
            //������ �����Ϳ� ���� ���̸� ������
            _projectileDictionary.Add(_currentProjectile, Resources.Load<GameObject>("Projectile/" + _currentProjectile));
            
        }

        public static void AddProjectileDic(ProjectileID id)
        {
            _instance._projectileDictionary.Add(id, Resources.Load<GameObject>("Projectile/" + id));

        }


        //�÷��̾�� ���� �����Ҽ��ִ� ������ ��������
        void SetProjectileToEnemy(float val) 
        {
            if (_temptenemyList.Count == 0) 
            {
                Debug.Log("no Enemy");
                return;
            }
            int maxTargetNum = 0;
            List<int> _targetList = new List<int>();
            //������ ���� ���� ���� ��� ���� ���ϱ�������. 
            for (int i =0; i< _temptenemyList.Count || maxTargetNum>=_targetEnemyCount; i++) 
            {
                //������ Ÿ�� ǥ����.
                _targetList.Add(i);
                maxTargetNum++;
            }

            foreach (GameObject obj in _temptenemyList) 
            {
                obj.GetComponent<SpriteRenderer>().color = Color.white;
            }

            foreach (int i in _targetList) 
            {
                _temptenemyList[i].GetComponent<SpriteRenderer>().color = Color.red;

            }

        }



        void SetTargetCount(int val) 
        {
            _targetEnemyCount = val;
        }





    }

}
