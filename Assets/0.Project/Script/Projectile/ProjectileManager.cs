using System.Collections;
using System.Collections.Generic;
using System;
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
        ProjectileIDObjectListDic _totalProjectileDictionary = new ProjectileIDObjectListDic() {};
        [SerializeField]
        ProjectileIDObjectListDic _deactivateProjectileDictionary = new ProjectileIDObjectListDic() {};

        [SerializeField]
        List<MobScript> _temptenemyList = new List<MobScript>();

        //ť �÷�Ʈ ������ �����Ѵ������ϳ��ϳ��� ���Ѵ�. ���ڴ� ������ ��� ������.
        //���� ť�� ���� �Ϲ� ����Ʈ ���� ���� ���
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

        // ���ҽ����� �ε��Ͽ� �ΰ��� ��ųʸ��� �����Ѵ�.
        int _totalprojectileNum = 0;
        void InitiallzeProjectileDic() 
        {
            //���� �� ���� �Ŵ������Լ� ������ �����Ϳ� ���� ���� �������� ��.
            //������ �����Ϳ� ���� ���̸� ������
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


        //�ڵ����� 
        List<int> _targetList = new List<int>();
        //������ �� ����Ʈ.
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

        //�÷��̾�� ���� �����Ҽ��ִ� ������ ��������

        //�ϴ� ���� ���� �𸣴ϱ� ���� ����.�װ� ���������� �ξ� �������
        // źâ�� 1, �ѹ��� ��� ������ ������
        void SetProjectileToEnemy(float val) 
        {
            foreach (ProjectileID id in Enum.GetValues(typeof(ProjectileID))) 
            {
                //Debug.Log("projectile check_ : " +projectile  + Global_CampaignData._projectileIDDataDic[projectile]._count);
                if (Global_CampaignData._projectileIDDataDic[id]._repeat <= 0)
                    continue;
                //�Ѿ� ���� ť�� �־ �߰��Ѵ�
                for (int i = 0; i < Global_CampaignData._projectileIDDataDic[id]._repeat; i++)
                    _currentShotAmmoDic[id].Enqueue(val);

                if (_shotCoroutineDic[id] == false)
                {
                    _shotCoroutineDic[id] = true;
                    StartCoroutine(ShotRoutine(id));
                }
                //���ⷮ �̻��Ѱ� ���� �ؾ���
                    //Debug.Log("Already Exist");
                //�̹� ���°� �ƴ϶� �̰� �ڷ�ƾ�� ������ ������ �����°� ���ڴ� ������ �߰��������ϴ� �ڵ嵵 ������.
            }

        }

        //����� ���� �������� ������Ʈ ���� ������ �̺�Ʈ�� ���� �����ϰ� ���� ���̴�.
        //�׳� �߻��ϴ� ����� ���� �� ���Ѵ�? �ϴ� �����ϵ��� ���� �غ���. ��ȣ ��ž�� �������� ��հڱ���
        float _temptDamage = 0;
        IEnumerator ShotRoutine(ProjectileID id) 
        {
            //źȯ�� 0�� �ƴҶ� ���� �ݺ�
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
            //������ �׳� instantiate�� ������ ���߿��� ������Ʈ Ǯ���� �����ϵ��� �����..
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
