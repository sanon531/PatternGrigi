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
        Dictionary<ProjectileID, List<GameObject>> _activateProjectileDictionary = new Dictionary<ProjectileID, List<GameObject>>() 
        {
            {ProjectileID.NormalBullet ,new List<GameObject>(){ } }
        };
        Dictionary<ProjectileID, List<GameObject>> _deactivateProjectileDictionary = new Dictionary<ProjectileID, List<GameObject>>() 
        {
            {ProjectileID.NormalBullet ,new List<GameObject>(){ } }
        };

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
            for (int i = 0; i < 30; i++) 
            {
                _deactivateProjectileDictionary[_currentProjectile].Add(Instantiate(_projectileDictionary[_currentProjectile], transform));
            }
        }

        public static void AddProjectileDic(ProjectileID id)
        {
            _instance._projectileDictionary.Add(id, Resources.Load<GameObject>("Projectile/" + id));

        }


        //�ڵ����� 
        List<int> _targetList = new List<int>();

        //�÷��̾�� ���� �����Ҽ��ִ� ������ ��������
        void SetProjectileToEnemy(float val) 
        {
            TargetTheEnemy();
            //Debug.Log(_dividedDamage);

            //������ �׳� instantiate�� ������ ���߿��� ������Ʈ Ǯ���� �����ϵ��� �����..

            foreach (int i in _targetList) 
            {
                float _dividedDamage = Global_CampaignData._charactorAttackDic[CharacterID.Player].FinalValue / _targetList.Count;
                GameObject _obj = ShootProjectile();
                Projectile_Script _tempt = _obj.GetComponent<Projectile_Script>();
                Vector3 _direction = _temptenemyList[i].transform.position - Player_Script.GetPlayerPosition();
                _direction = _direction.normalized;
                _tempt.SetInitialProjectileData(_direction, _dividedDamage, Global_CampaignData._projectileSpeed.FinalValue, 10);
            }

        }
        public void TargetTheEnemy()
        {
            _temptenemyList = MobGenerator.GetMobList();

            if (_temptenemyList.Count == 0)
            {
                Debug.Log("no Enemy");
                return;
            }
            int maxTargetNum = 0;
            _targetList = new List<int>();
            //������ ���� ���� ���� ��� ���� ���ϱ�������. 
            //���߿��� �����׷����Ϳ��� ������ ��ġ�� ������.
            for (int i = 0; i < _temptenemyList.Count; i++)
            {
                //������ Ÿ�� ǥ����.
                _targetList.Add(i);

                maxTargetNum++;
                if (maxTargetNum >= Global_CampaignData._projectileTargetNum.FinalValue)
                    break;
            }

            foreach (GameObject obj in _temptenemyList)
                obj.GetComponent<SpriteRenderer>().color = Color.white;
            foreach (int i in _targetList)
                _temptenemyList[i].GetComponent<SpriteRenderer>().color = Color.red;
        }

        //����ü�� ��ٸ� ��Ƽ���̼ǿ��ٰ� ����
        GameObject ShootProjectile() 
        {
            if (_deactivateProjectileDictionary[_currentProjectile].Count == 0) 
            {
                _deactivateProjectileDictionary[_currentProjectile].Add(Instantiate(_projectileDictionary[_currentProjectile], transform));
            }
            GameObject _tempt = _deactivateProjectileDictionary[_currentProjectile][0];
            _deactivateProjectileDictionary[_currentProjectile].Remove(_tempt);
            _activateProjectileDictionary[_currentProjectile].Add(_tempt);
            return _tempt;
        }
        public static void SetBackProjectile(GameObject projectile, ProjectileID id) 
        {
            if (_instance._activateProjectileDictionary[id].Contains(projectile)) 
            {
                _instance._activateProjectileDictionary[id].Remove(projectile);
                _instance._deactivateProjectileDictionary[id].Add(projectile);
            }
        }



    }

}
