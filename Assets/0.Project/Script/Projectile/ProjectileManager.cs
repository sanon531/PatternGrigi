using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using PG.Event;

namespace PG.Battle 
{
    //오브젝트 매니져는 이이외에도 그저
    public class ProjectileManager : MonoSingleton<ProjectileManager>
    {
        //나중에 풀링하기 위해서 만듬 지금은 풀할 필요없다.
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

        // 리소스에서 로드하여 두가지 딕셔너리에 저장한다.
        void InitiallzeProjectileDic() 
        {
            //먼저 몹 스폰 매니져에게서 적들의 데이터에 관한 값을 가져오게 됨.
            //적들의 데이터에 관한 것이면 적들의
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


        //자동으로 
        List<int> _targetList = new List<int>();

        //플레이어는 현재 공격할수있는 적에게 데미지를
        void SetProjectileToEnemy(float val) 
        {
            TargetTheEnemy();
            //Debug.Log(_dividedDamage);

            //지금은 그냥 instantiate를 하지만 나중에는 오브젝트 풀링이 가능하도록 만들것..

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

            foreach (GameObject obj in _temptenemyList)
                obj.GetComponent<SpriteRenderer>().color = Color.white;
            foreach (int i in _targetList)
                _temptenemyList[i].GetComponent<SpriteRenderer>().color = Color.red;
        }

        //투사체를 쏜다면 앧티베이션에다가 놓고
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
