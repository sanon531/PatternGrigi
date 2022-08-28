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
        Dictionary<ProjectileID, List<GameObject>> _activateProjectileDictionary = new Dictionary<ProjectileID, List<GameObject>>();
        Dictionary<ProjectileID, List<GameObject>> _deactivateProjectileDictionary = new Dictionary<ProjectileID, List<GameObject>>();

        //플레이어가
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

        // 리소스에서 로드하여 두가지 딕셔너리에 저장한다.
        void InitiallzeProjectileDic() 
        {
            //먼저 몹 스폰 매니져에게서 적들의 데이터에 관한 값을 가져오게 됨.
            //적들의 데이터에 관한 것이면 적들의
            _projectileDictionary.Add(_currentProjectile, Resources.Load<GameObject>("Projectile/" + _currentProjectile));
            
        }

        public static void AddProjectileDic(ProjectileID id)
        {
            _instance._projectileDictionary.Add(id, Resources.Load<GameObject>("Projectile/" + id));

        }


        //자동으로 
        List<int> _targetList = new List<int>();
        public  void TargetTheEnemy() 
        {
            if (_temptenemyList.Count == 0)
            {
                Debug.Log("no Enemy");
                return;
            }
            int maxTargetNum = 0;
            _targetList = new List<int>();
            //지금은 적의 스폰 순서 대로 대충 정하긴하지만. 
            //나중에는 몹제네레이터에서 몹들의 위치를 정해줌.
            for (int i = 0; i < _temptenemyList.Count ; i++)
            {
                //몹에도 타겟 표시함.
                _targetList.Add(i);

                maxTargetNum++;
                if (maxTargetNum >= _targetEnemyCount)
                    break;
            }

            foreach (GameObject obj in _temptenemyList)
                obj.GetComponent<SpriteRenderer>().color = Color.white;

            foreach (int i in _targetList)
                _temptenemyList[i].GetComponent<SpriteRenderer>().color = Color.red;
        }

        //플레이어는 현재 공격할수있는 적에게 데미지를
        void SetProjectileToEnemy(float val) 
        {
            TargetTheEnemy();
            float _dividedDamage = Global_CampaignData._charactorAttackDic[CharacterID.Player].FinalValue / _targetList.Count;
            Debug.Log(_dividedDamage);

            //지금은 그냥 instantiate를 하지만 나중에는 오브젝트 풀링이 가능하도록 만들것..

            foreach (int i in _targetList) 
            {
                Projectile_Script _tempt = Instantiate(_projectileDictionary[_currentProjectile], Player_Script.GetPlayerPosition(), Quaternion.identity, transform).GetComponent<Projectile_Script>();
                Vector3 _direction = _temptenemyList[i].transform.position - Player_Script.GetPlayerPosition() ;
                _direction = _direction.normalized;
                _tempt.SetInitialProjectileData(_direction, _dividedDamage,Global_CampaignData._projectileSpeed.FinalValue,10);
            }

        }



        void SetTargetCount(int val) 
        {
            _targetEnemyCount = val;
        }





    }

}
