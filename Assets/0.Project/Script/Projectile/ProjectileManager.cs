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
        ProjectileIDObjectListDic _totalProjectileDictionary = new ProjectileIDObjectListDic() {};
        [SerializeField]
        ProjectileIDObjectListDic _deactivateProjectileDictionary = new ProjectileIDObjectListDic() {};

        [SerializeField]
        List<MobScript> _temptenemyList = new List<MobScript>();

        //큐 플로트 값으로 저장한다음에하나하나씩 팝한다. 숫자는 정해진 대로 나오고.
        //만약 큐의 수가 일반 리스트 보다 작을 경우
        Dictionary<ProjectileID, Queue<float>> _currentShotAmmoDic = new Dictionary<ProjectileID, Queue<float>>() { };
        Dictionary<ProjectileID, bool> _shotCoroutineDic = new Dictionary<ProjectileID, bool>() {};


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
                _currentShotAmmoDic.Add(projectile, new Queue<float>());
                _shotCoroutineDic.Add(projectile, false);
                _projectileDictionary.Add(projectile, Resources.Load<GameObject>("Projectile/" + projectile));
                _totalProjectileDictionary.Add(projectile, new List<GameObject>() { });
                _deactivateProjectileDictionary.Add(projectile, new List<GameObject>() { });
                for (int i = 0; i < 10; i++)
                {
                    _tempt = Instantiate(_projectileDictionary[projectile], transform);
                    _tempt.name = projectile.ToString() + i.ToString();
                    _totalprojectileNum++;
                    _totalProjectileDictionary[projectile].Add(_tempt);
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

        //일단 어찌 될지 모르니까 통합 하자.그게 유지보수에 훨씬 쉬울듯함
        // 탄창이 1, 한번에 쏘는 갯수로 나뉜다
        void SetProjectileToEnemy(float val) 
        {
            foreach (ProjectileID id in Enum.GetValues(typeof(ProjectileID))) 
            {
                //Debug.Log("projectile check_ : " +projectile  + Global_CampaignData._projectileIDDataDic[projectile]._count);
                if (Global_CampaignData._projectileIDDataDic[id]._repeat <= 0)
                    continue;
                //총알 수를 큐에 넣어서 추가한다
                for (int i = 0; i < Global_CampaignData._projectileIDDataDic[id]._repeat; i++)
                    _currentShotAmmoDic[id].Enqueue(val);

                if (_shotCoroutineDic[id] == false)
                {
                    _shotCoroutineDic[id] = true;
                    StartCoroutine(ShotRoutine(id));
                }
                //산출량 이상한거 수정 해야함
                    //Debug.Log("Already Exist");
                //이미 나온게 아니라 이거 코루틴이 있을때 없을때 나누는게 낫겠다 어차피 중간에수정하는 코드도 없으니.
            }

        }

        //현재는 통합 형태지만 오브젝트 연사 정도도 이벤트로 조절 가능하게 만들 것이다.
        //그냥 발사하는 놈들임 조준 은 안한다? 일단 조준하도록 구현 해볼까. 야호 포탑이 움직여도 재밌겠구만
        float _temptDamage = 0;
        IEnumerator ShotRoutine(ProjectileID id) 
        {
            //탄환이 0이 아닐때 까지 반복
            while (true) 
            {
                //Shot
                //if(id == ProjectileID.StraightShot)
                //Debug.Log(Global_CampaignData._projectileIDDataDic[id]._count + "shot ss" + Global_CampaignData._projectileIDDataDic[id]._cooltime);
                _temptDamage = _currentShotAmmoDic[id].Dequeue();
                SetSpreadShotStyle(_temptDamage, id);

                if (_currentShotAmmoDic[id].Count > 0) 
                {
                    yield return new WaitForSeconds(Global_CampaignData._projectileIDDataDic[id]._cooltime);
                }
                else
                    break;
            }

            _shotCoroutineDic[id] = false;
            //Debug.Log("routine finished");
            yield return null;

        }
        //
        void SetSpreadShotStyle(float val, ProjectileID id) 
        {
            TargetTheEnemy();
            //지금은 그냥 instantiate를 하지만 나중에는 오브젝트 풀링이 가능하도록 만들것..
            int _spreadcount = Global_CampaignData._projectileIDDataDic[id]._count;


            while (_spreadcount > 0) 
            {
                for (int i = _targetList.Count - 1; i >= 0 && _spreadcount > 0; i--)
                {
                    GameObject _obj = ShootProjectile(id);
                    Projectile_Script _tempt = _obj.GetComponent<Projectile_Script>();
                    _tempt.SetInitialProjectileData(_targetedMobList[i], val, _projectileLifeTimeDic[id], 0.25f * (i - _spreadcount));
                    //Debug.Log("ss" + _spreadcount);
                    _spreadcount--;
                }
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
        GameObject ShootProjectile(ProjectileID id) 
        {
            GameObject _tempt;
            if (_deactivateProjectileDictionary[id].Count <= 0)
            {
                _tempt = Instantiate(_projectileDictionary[id], transform);
                _tempt.name = id + (_totalProjectileDictionary[id].Count ).ToString();
                _totalProjectileDictionary[id].Add(_tempt);
                Debug.Log("build new " + _deactivateProjectileDictionary[id].Count.ToString());
            }
            else 
            {
                _tempt = _deactivateProjectileDictionary[id][0];
                _deactivateProjectileDictionary[id].Remove(_tempt);
            }
            return _tempt;
        }
        public static void SetBackProjectile(GameObject projectile, ProjectileID id) 
        {
            _instance._deactivateProjectileDictionary[id].Add(projectile);
            projectile.transform.position = _instance.transform.position;
        }



    }

}
