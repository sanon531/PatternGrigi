using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PG.Data;
using PG.Event;

namespace PG.Battle 
{
    //������Ʈ �Ŵ����� ���̿ܿ��� ����
    public class ProjectileManager : MonoSingleton<ProjectileManager>
    {
        //���߿� Ǯ���ϱ� ���ؼ� ���� ������ Ǯ�� �ʿ����.
        [SerializeField]
        ProjectileIDObjectDic _projectileDictionary = new ProjectileIDObjectDic();
        [SerializeField]
        ProjectileIDObjectListDic _activateProjectileDictionary = new ProjectileIDObjectListDic() 
        {
            {ProjectileID.NormalBullet ,new List<GameObject>(){ } },
            {ProjectileID.LightningShot ,new List<GameObject>(){ } }

        };
        [SerializeField]
        ProjectileIDObjectListDic _deactivateProjectileDictionary = new ProjectileIDObjectListDic() 
        {
            {ProjectileID.NormalBullet ,new List<GameObject>(){ } },
            {ProjectileID.LightningShot ,new List<GameObject>(){ } }
        };

        [SerializeField]
        ProjectileID _currentProjectile = ProjectileID.NormalBullet;
        [SerializeField]
        List<MobScript> _temptenemyList = new List<MobScript>();


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
        int _totalprojectileNum = 0;
        void InitiallzeProjectileDic() 
        {
            //���� �� ���� �Ŵ������Լ� ������ �����Ϳ� ���� ���� �������� ��.
            //������ �����Ϳ� ���� ���̸� ������
            GameObject _tempt;

            _projectileDictionary.Add(_currentProjectile, Resources.Load<GameObject>("Projectile/" + _currentProjectile));
            for (int i = 0; i < 1; i++) 
            {
                _tempt = Instantiate(_projectileDictionary[_currentProjectile], transform);
                _tempt.name = _currentProjectile.ToString() + i.ToString();
                _totalprojectileNum++;
                _deactivateProjectileDictionary[_currentProjectile].Add(_tempt);
            }
        }

        public static void AddProjectileDic(ProjectileID id)
        {
            _instance._projectileDictionary.Add(id, Resources.Load<GameObject>("Projectile/" + id));

        }


        //�ڵ����� 
        List<int> _targetList = new List<int>();
        //������ �� ����Ʈ.
        List<MobScript> _targetedMobList = new List<MobScript>();
        [SerializeField]
        ProjectileIDFloatDic _projectileLifeTimeDic = new ProjectileIDFloatDic()
        {
            { ProjectileID.NormalBullet,10f},
            { ProjectileID.LightningShot,0.5f},
            { ProjectileID.StraightKnife,10f},
            { ProjectileID.SatiliteOrbit,5f},

        };

        //�÷��̾�� ���� �����Ҽ��ִ� ������ ��������
        void SetProjectileToEnemy(float val) 
        {
            TargetTheEnemy();
            //Debug.Log(_dividedDamage);

            //������ �׳� instantiate�� ������ ���߿��� ������Ʈ Ǯ���� �����ϵ��� �����..

            for (int i = _targetList.Count -1; i>=0;i-- ) 
            {
                GameObject _obj = ShootProjectile();
                Projectile_Script _tempt = _obj.GetComponent<Projectile_Script>();
                //Vector3 _direction = _targetTransforms[i] - Player_Script.GetPlayerPosition();
                //_direction = _direction.normalized;
                _tempt.SetInitialProjectileData(_targetedMobList[i], val, _projectileLifeTimeDic[_currentProjectile]);
            }

        }
        public void TargetTheEnemy()
        {
            _temptenemyList = MobGenerator.GetMobList();

            if (_temptenemyList.Count == 0)
            {
                //Debug.Log("no Enemy");
                return;
            }
            int maxTargetNum = 0;
            _targetList.Clear();
            _targetedMobList.Clear();
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
            // Ÿ�� �Ǿ��ٴ°� ǥ���ϱ� + ��ġ ǥ�� ����
            for (int i = 0; i < _temptenemyList.Count; i++) 
            {
                _targetedMobList.Add(_temptenemyList[i]);

                if (_targetList.Contains(i))
                {
                    _temptenemyList[i].SetTargetted(true);
                }
                else 
                {
                    _temptenemyList[i].SetTargetted(false);
                }
            }

        }

        //����ü�� ��ٸ� ��Ƽ���̼ǿ��ٰ� ����
        GameObject ShootProjectile() 
        {
            GameObject _tempt;
            if (_deactivateProjectileDictionary[_currentProjectile].Count <= 0)
            {
                _tempt = Instantiate(_projectileDictionary[_currentProjectile], transform);
                //Debug.Log("build new " + _deactivateProjectileDictionary[_currentProjectile].Count.ToString());
            }
            else 
            {
                _tempt = _deactivateProjectileDictionary[_currentProjectile][0];
                _deactivateProjectileDictionary[_currentProjectile].Remove(_tempt);
            }
            _activateProjectileDictionary[_currentProjectile].Add(_tempt);
            return _tempt;
        }
        public static void SetBackProjectile(GameObject projectile, ProjectileID id) 
        {
            if (_instance._activateProjectileDictionary[id].Contains(projectile))
            {
                _instance._activateProjectileDictionary[id].Remove(projectile);
                //Debug.Log(" removed smoothly" + projectile.name);
            }

            _instance._deactivateProjectileDictionary[id].Add(projectile);
            projectile.transform.position = _instance.transform.position;
        }



    }

}
