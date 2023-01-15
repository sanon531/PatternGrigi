using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PG.Event;
using PG.Data;

namespace PG.Battle
{
    public class ArtifactManager : MonoSingleton<ArtifactManager>
    {

        //���� Ǯ�� ó���� ���� �����ϸ� ������ �Ǹ� ���� Ȯ���� �����ԵǸ�
        //���� ����Ʈ�� ���� ��� ���� Ŭ������ ������Ʈ�� ���� �ȴ�.
        //�� ���� Ǯ�� ���� �� ���
        private static List<ArtifactID> _commonArtifactPool = new List<ArtifactID>();
        public static List<ArtifactID> commonArtifactPool { get { return new List<ArtifactID>(_commonArtifactPool); } }
        private static List<ArtifactID> _uncommonArtifactPool = new List<ArtifactID>();
        public static List<ArtifactID> uncommonArtifactPool { get { return new List<ArtifactID>(_uncommonArtifactPool); } }
        private static List<ArtifactID> _rareArtifactPool = new List<ArtifactID>();
        public static List<ArtifactID> rareArtifactPool { get { return new List<ArtifactID>(_rareArtifactPool); } }
        private static List<ArtifactID> _uniqueArtifactPool = new List<ArtifactID>();
        public static List<ArtifactID> uniqueArtifactPool { get { return new List<ArtifactID>(_uniqueArtifactPool); } }
        
        public static ArtifactScriptableData s_artifactData;
        [SerializeField]
        private ArtifactScriptableData artifactData;

        
        [SerializeField]
        bool _isTestSet = true;
        [SerializeField]
        List<ArtifactID> _showerArtifectList = new List<ArtifactID>() { };

        private HashSet<ArtifactID> completedIDset;


        //ȹ�� ������ ��Ƽ��Ʈ���� ����Ʈ ���⼭ 
        protected override void CallOnAwake()
        {
            Global_BattleEventSystem._onBattleBegin += InitializeCurrentArtifact;
            Global_BattleEventSystem._onLevelUpShow += SetLevelUpOn;
            Global_BattleEventSystem._onLevelUpHide += SetLevelUpOff;
            s_artifactData = artifactData;
        }
        protected override void CallOnDestroy()
        {
            Global_BattleEventSystem._onBattleBegin -= InitializeCurrentArtifact;
            Global_BattleEventSystem._onLevelUpShow -= SetLevelUpOn;
            Global_BattleEventSystem._onLevelUpHide -= SetLevelUpOff;

        }

        //������ ������ ������ ���ƾ��Ѵ�. 
        //���� �� �ִ� ���׷��̵尡 ���� ���

        void SetLevelUpOn() 
        {
            ArtifactSetRandomly();
            LevelUpPanelScript.LevelUpPannelOn();
        }
        void SetLevelUpOff() 
        {
            LevelUpPanelScript.LevelUpPannelOff();
        }

        //������ ����������. �ϴ� �������� ������ 
        void ArtifactSetRandomly() 
        {
            _showerArtifectList.Clear();
            
            if(Global_CampaignData._obtainableArtifactIDList.Count>4)
                _showerArtifectList = MyRandom.PickRandoms(Global_CampaignData._obtainableArtifactIDList,4);
            else
            {
                foreach (var id in Global_CampaignData._obtainableArtifactIDList)
                {
                    _showerArtifectList.Add(id);
                }
            }

            if (_showerArtifectList.Count == 0)
                _showerArtifectList.Add(ArtifactID.Default_HealthUp);

            LevelUpPanelScript.SetRandomItemOnPannel(_showerArtifectList);
        }



        //���� ������ �ִ� ��Ƽ��Ʈ�� ���� �� Ȯ���� ������,.
        void InitializeCurrentArtifact()
        {
            //������ �����͸� ���� ����� ����.
            DisableAllArtifact();
            if (Global_CampaignData._startArtifactList.Count != 0) 
            {
                foreach (ArtifactID id in Global_CampaignData._startArtifactList) 
                {
                    AddArtifactToPlayer_tempUse(id);
                }
            }
        }
        //���� ȹ���ϴ�
        public static void AddArtifactToPlayer_tempUse(ArtifactID id)
        {
            //�ϴ� ������ ��ųʸ����� 1���� ���� �� ������ ������ �־����� �׳� ���ڰ� ����Ѵٰ� ����. 
            //�׸��� �ش��ϴ� �������� ����� ��� 
            if (Global_CampaignData._currentArtifactDictionary.ContainsKey(id) == false)
            {
                //Debug.Log("Upgrade" + id.ToString());
                _instance._showerArtifectList.Add(id);
                Global_CampaignData._currentArtifactDictionary.Add(id, 
                    Artifact.Create(id,ArtifactManager.s_artifactData.idArtifactDataDic[id]));
                Global_CampaignData._currentArtifactDictionary[id].OnGetArtifact();
                //ĵ������ �и��� ���� ���ܵ�.
                ArtifactListShower.SetNewCaseOnList(id);
                //���� ������ ������ �ϰ��� ������ ������ �¿��� ��������.
            }
            else 
            {
                Global_CampaignData._currentArtifactDictionary[id].AddCountOnArtifact();
                //ĵ������ ���̸� ���� ���ܵ�.
                ArtifactListShower.SetNumberOnCase(id, Global_CampaignData._currentArtifactDictionary[id].UpgradeCount);
            }
        }


        public static void RemoveArtifactOnPlayer(ArtifactID id)
        {
            Global_CampaignData._obtainableArtifactIDList.Remove(id);
            _instance.SearchArtifactMixtureAndSet(id);
        }

        private void SearchArtifactMixtureAndSet(ArtifactID id)
        {
        }

        private void DebugArtifactlist()
        {
            string str = "";
            foreach (var iD in Global_CampaignData._obtainableArtifactIDList)
            {
                str += iD;
                str += "\n";
            }

            Debug.Log(str);

        }


        //������ �ʱ�ȭ �Ǹ� ������ ��� ��Ƽ��Ʈ ������ �����͵��� �����Ѵ�.
        void DisableAllArtifact() 
        {
            foreach (KeyValuePair<ArtifactID, Artifact> keyValuePair in  Global_CampaignData._currentArtifactDictionary) 
            {
                keyValuePair.Value.DisposeArtifact();
            }
            Global_CampaignData._currentArtifactDictionary.Clear();
        }

    }
}
