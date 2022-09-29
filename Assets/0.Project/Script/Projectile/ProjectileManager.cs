using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using PG.Data;
using PG.Event;

namespace PG.Battle 
{
    //오브젝트 매니져는 이이외에도 그저
    public class ProjectileManager : MonoSingleton<ProjectileManager>
    {
        //나중에 풀링하기 위해서 만듬 지금은 풀할 필요없다.
        [SerializeField]
        ProjectileIDObjectDic _projectileDictionary = new ProjectileIDObjectDic();
        [SerializeField]
        ProjectileIDObjectListDic _activateProjectileDictionary = new ProjectileIDObjectListDic() {};
        [SerializeField]
        ProjectileIDObjectListDic _deactivateProjectileDictionary = new ProjectileIDObjectListDic() {};

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

        // 리소스에서 로드하여 두가지 딕셔너리에 저장한다.
        int _totalprojectileNum = 0;
        void InitiallzeProjectileDic() 
        {
            //먼저 몹 스폰 매니져에게서 적들의 데이터에 관한 값을 가져오게 됨.
            //적들의 데이터에 관한 것이면 적들의
            GameObject _tempt;

            foreach (ProjectileID projectile in Enum.GetValues(typeof(ProjectileID))) 
            {
                _projectileDictionary.Add(projectile, Resources.Load<GameObject>("Projectile/" + projectile));
                _activateProjectileDictionary.Add(projectile, new List<GameObject>() { });
                _deactivateProjectileDictionary.Add(projectile, new List<GameObject>() { });
                for (int i = 0; i < 10; i++)
                {
                    _tempt = Instantiate(_projectileDictionary[projectile], transform);
                    _tempt.name = projectile.ToString() + i.ToString();
                    _totalprojectileNum++;
                    _deactivateProjectileDictionary[projectile].Add(_tempt);
                }
            }
        }

        public static void AddProjectileDic(ProjectileID id)
        {
            _instance._projectileDictionary.Add(id, Resources.Load<GameObject>("Projectile/" + id));
        }


        //자동으로 
        List<int> _targetList = new List<int>();
        //지정된 몹 리스트.
        List<MobScript> _targetedMobList = new List<MobScript>();
        [SerializeField]
        ProjectileIDFloatDic _projectileLifeTimeDic = new ProjectileIDFloatDic()
        {
            { ProjectileID.NormalBullet,10f},
            { ProjectileID.LightningShot,0.5f},
            { ProjectileID.StraightShot,10f},
            { ProjectileID.TowerBullet,5f},
            { ProjectileID.CuttingKnife,1f},

        };

        //플레이어는 현재 공격할수있는 적에게 데미지를
        void SetProjectileToEnemy(float val) 
        {
            TargetTheEnemy();
            //지금은 그냥 instantiate를 하지만 나중에는 오브젝트 풀링이 가능하도록 만들것..

            for (int i = _targetList.Count -1; i>=0;i-- ) 
            {
                GameObject _obj = ShootProjectile();
                Projectile_Script _tempt = _obj.GetComponent<Projectile_Script>();
                //Vector3 _direction = _targetTransforms[i] - Player_Script.GetPlayerPosition();
                //_direction = _direction.normalized;
                //적이 죽었을때 만약에 연속적으로 호출 할경우 시간 타이밍에 따른 에러가 생성된다 
                //Debug.Log(_projectileLifeTimeDic[_currentProjectile]);
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
            //지금은 적의 스폰 순서 대로 대충 정하긴하지만. 
            //나중에는 몹제네레이터에서 몹들의 위치를 정해줌.
            for (int i = 0; i < _temptenemyList.Count; i++)
            {
                //몹에도 타겟 표시함.
                _targetList.Add(i);
                maxTargetNum++;
                if (maxTargetNum >= Global_CampaignData._projectileTargetNum.FinalValue)
                    break;
            }
            // 타겟 되었다는거 표시하기 + 위치 표시 관련
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

        //투사체를 쏜다면 앧티베이션에다가 놓고
        GameObject ShootProjectile() 
        {
            GameObject _tempt;
            if (_deactivateProjectileDictionary[_currentProjectile].Count <= 0)
            {
                _tempt = Instantiate(_projectileDictionary[_currentProjectile], transform);
                Debug.Log("build new " + _deactivateProjectileDictionary[_currentProjectile].Count.ToString());
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
                Debug.Log(" removed smoothly" + projectile.name);
            }
            else 
            {
                Debug.Log(" removed wierd");

            }

            _instance._deactivateProjectileDictionary[id].Add(projectile);
            projectile.transform.position = _instance.transform.position;
        }



    }

}
