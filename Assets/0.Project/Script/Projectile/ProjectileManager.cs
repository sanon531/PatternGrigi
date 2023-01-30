using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using PG.Data;
using PG.Event;
using UnityEngine.Serialization;

namespace PG.Battle 
{
    //오브젝트 매니져는 이이외에도 그저
    public class ProjectileManager : MonoSingleton<ProjectileManager>
    {
        //나중에 풀링하기 위해서 만듬 지금은 풀할 필요없다.
        [SerializeField]
        private ProjectileIDObjectDic projectileDictionary = new ProjectileIDObjectDic();

        private Dictionary<ProjectileID, ProjectilePool<ProjectileScript>> _totalProjectileDictionary 
            = new Dictionary<ProjectileID, ProjectilePool<ProjectileScript>>();

        [SerializeField] private ProjectileIDintDic trackerIDintDic = new ProjectileIDintDic();
            
        [SerializeField]
        ProjectileIDFloatDic projectileLifeTimeDic = new ProjectileIDFloatDic()
        {
            { ProjectileID.NormalBullet,5f},
            { ProjectileID.LightningShot,0.5f},
            { ProjectileID.StraightShot,10f},
            { ProjectileID.TowerBullet,5f},
            { ProjectileID.CuttingKnife,1f},

        };

        
        //큐 플로트 값으로 저장한다음에하나하나씩 팝한다. 숫자는 정해진 대로 나오고.
        //만약 큐의 수가 일반 리스트 보다 작을 경우
        Dictionary<ProjectileID, Queue<float>> _currentShotAmmoDic 
            = new Dictionary<ProjectileID, Queue<float>>() { };
        //해당 셋이 현재 쏘는 상황인지 확인, 딜레이인지 확인.
        HashSet<ProjectileID> _keepShotSet = new HashSet<ProjectileID>(){};


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

            print(Global_CampaignData._projectileIDDataDic.Count());
            foreach (var data in Global_CampaignData._projectileIDDataDic)
            {
                _currentShotAmmoDic.Add(data.Key, new Queue<float>());
                projectileDictionary.Add(data.Key, Resources.Load<GameObject>("Projectile/" + data.Key));
                trackerIDintDic.Add(data.Key,0);
                _totalProjectileDictionary.Add(data.Key,
                    new ProjectilePool<ProjectileScript>
                    (
                        CreateProjectile,
                        OnGet,
                        OnRelease,
                        null,
                        true,
                        id :(int)data.Key,
                        10000
                    )
                );
                for(int i = 0 ; i<10 ;i++)
                    _totalProjectileDictionary[data.Key].FillStack();
                
            }
        }

        #region ObjectPoolPlace

        private ProjectileScript CreateProjectile(int id)
        {
            //print("create"+id);
            ProjectileID temptID = (ProjectileID)id;
            ProjectileScript project = Instantiate(projectileDictionary[temptID], transform).GetComponent<ProjectileScript>();
            project.SetInitialProjectileData(
                _totalProjectileDictionary[temptID],
                projectileLifeTimeDic[temptID]);
            
            project.gameObject.SetActive(false);
            return project;
        }

        private void OnGet(ProjectileScript projectileScript)
        {
            projectileScript.gameObject.SetActive(true);
            //print("pick"+nameof(projectileScript));
            Global_CampaignData._activatedProjectileList.Add(projectileScript.transform);
            trackerIDintDic[projectileScript.id]--;
            
        }
        private void OnRelease(ProjectileScript projectileScript)
        {
            projectileScript.gameObject.SetActive(false);
            //print("release"+nameof(projectileScript));
            Global_CampaignData._activatedProjectileList.Remove(projectileScript.transform);
            trackerIDintDic[projectileScript.id]++;
        }

        #endregion
        
        

        public static void AddProjectileDic(ProjectileID id)
        {
            _instance.projectileDictionary.Add(id, Resources.Load<GameObject>("Projectile/" + id));
        }


        //자동으로 
        List<int> _targetList = new List<int>();
        //지정된 몹 리스트.
        [SerializeField]
        List<MobScript> _targetedMobList = new List<MobScript>();

        //플레이어는 현재 공격할수있는 적에게 데미지를

        //일단 어찌 될지 모르니까 통합 하자.그게 유지보수에 훨씬 쉬울듯함
        // 탄창이 1, 한번에 쏘는 갯수로 나뉜다
        void SetProjectileToEnemy(float val) 
        {
            foreach (var pair in Global_CampaignData._projectileIDDataDic) 
            {
                
                 //Debug.Log("projectile check_ count: " +pair.Key + pair.Value._count+"projectile check_ repeat: "+pair.Value._repeat);
                
                if (pair.Value._repeat <= 0||
                    pair.Value._count<= 0)
                    continue;
                //총알 수를 큐에 넣어서 추가한다
                for (int i = 0; i < pair.Value._repeat; i++)
                    _currentShotAmmoDic[pair.Key].Enqueue(val);

                //간단히
                if (!_keepShotSet.Contains(pair.Key))
                {
                    _keepShotSet.Add(pair.Key);
                    StartCoroutine(ShotRoutine(pair.Key));
                }
            }

        }

        //발사 관련한 데이터가 있을 때 알아서 처리됨

        //현재 리턴 상태 측정하는 로그
        void LogCurrentDic()
        {
            
            
        }


        //현재는 통합 형태지만 오브젝트 연사 정도도 이벤트로 조절 가능하게 만들 것이다.
        //그냥 발사하는 놈들임 조준 은 안한다? 일단 조준하도록 구현 해볼까. 야호 포탑이 움직여도 재밌겠구만
        float _temptDamage = 0;

        IEnumerator ShotRoutine(ProjectileID id) 
        {
            //탄환이 0이 아닐때 까지 반복
            while (true) 
            {
                //yield return new WaitForEndOfFrame();
                //print("Pew Pew"+id);
                _temptDamage = _currentShotAmmoDic[id].Dequeue();
                SetSpreadShotStyle(_temptDamage, id);

                if (_currentShotAmmoDic[id].Count > 0) 
                {
                    yield return new WaitForSeconds(Global_CampaignData._projectileIDDataDic[id]._cooltime);
                }
                else
                    break;
            }

            //_shotCoroutineDic[id] = false;
            _keepShotSet.Remove(id);
            //Debug.Log("routine finished");
            yield return null;

        }
        //
        // ReSharper disable Unity.PerformanceAnalysis
        void SetSpreadShotStyle(float val, ProjectileID id) 
        {
            //지금은 그냥 instantiate를 하지만 나중에는 오브젝트 풀링이 가능하도록 만들것..
            TargetTheEnemy();
            int _spreadcount = Global_CampaignData._projectileIDDataDic[id]._count;

            int sqrtCeil = Mathf.CeilToInt(Mathf.Sqrt(_spreadcount));

            
            //발사에 관하여서 그냥 발사하는 시스템으로 만들어야한다.
            while (_spreadcount > 0) 
            {
                if (_targetedMobList.Count > 0)
                {
                    for (int i = 0; i <_targetedMobList.Count ; i++)
                    {
                        ProjectileScript _tempt = _totalProjectileDictionary[id].PickUp();
                        _tempt.SetFrequentProjectileData(_targetedMobList[i] , val, 
                            GetPosBySpread(_spreadcount,sqrtCeil)
                        );
                        _spreadcount--;
                    }
                }
                else
                {
                    ProjectileScript _tempt = _totalProjectileDictionary[id].PickUp();
                    _tempt.SetFrequentProjectileData(null, val, 
                        GetPosBySpread(_spreadcount,sqrtCeil)
                    );
                    //Debug.Log("ss" + GetPosBySpread(i,sqrtCeil));
                    _spreadcount--;
                }

            }


        }

        Vector2 GetPosBySpread(int thisCount, int sqrtCeil)
        {
            float x = (thisCount-1) % sqrtCeil  - (sqrtCeil-1)/2;
            float y = (thisCount-1) / sqrtCeil - (sqrtCeil-1)/2;
            //print("thisCount : "+thisCount + "sqrtCeil : "+sqrtCeil +" x : " +x+" y : "+y);
            x *= 1.5f;
            y *= 1.5f;
            
            return new Vector2(x, y);
        }


        [SerializeField]
        private List<MobScript> _temptenemyList;

        public void TargetTheEnemy()
        {
            _temptenemyList = MobGenerator.GetMobList();
            int maxTargetNum;
            maxTargetNum = 0;
            _targetList.Clear();
            _targetedMobList.Clear();
            if (_temptenemyList.Count <= 0)
                return ;
            
            //지금은 적의 스폰 순서 대로 대충 정하긴하지만. 
            //나중에는 몹제네레이터에서 몹들의 위치를 정해줌.
            //만약 리스트에 없으면 어케함.
            //적이 없을 경우 그냥.
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
            }

        }

     
    }

}
