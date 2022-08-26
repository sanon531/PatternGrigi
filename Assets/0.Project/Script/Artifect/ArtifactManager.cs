using System.Collections;
using System.Collections.Generic;
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

        [SerializeField]
        bool _isTestSet = true;
        [SerializeField]
        List<ArtifactID> _temptTestArtifectList = new List<ArtifactID>() { };
        protected override void CallOnAwake()
        {
            Global_BattleEventSystem._onBattleBegin += InitializeCurrentArtifact;
        }
        protected override void CallOnDestroy()
        {
            Global_BattleEventSystem._onBattleBegin -= InitializeCurrentArtifact;
        }

        //���� ������ �ִ� ��Ƽ��Ʈ�� ���� �� Ȯ���� ������,.
        void InitializeCurrentArtifact()
        {
            if (_isTestSet)
                foreach (ArtifactID id in _temptTestArtifectList)
                {
                    Global_CampaignData._currentArtifactDictionary.Add(id, GlobalDataStorage.TotalArtifactClassDic[id]);
                    Global_CampaignData._currentArtifactDictionary[id].ActiveArtifact();
                }
            else
            {
                //����� �ƿ� ���� �����ɷ� �����.
                //GlobalDataStorage.TotalArtifactTableDataDic;
            }
        }
        //���� ȹ���ϴ�
        public static void AddArtifactToPlayer_tempUse(ArtifactID id)
        {
            //�ϴ� ������ ��ųʸ����� 1���� ���� �� ������ ������ �־����� �׳� ���ڰ� ����Ѵٰ� ����. 
            //�̷����� �׳� �����ϰ�
            if (Global_CampaignData._currentArtifactDictionary.ContainsKey(id) == false)
            {
                Debug.Log("Upgrade" + id.ToString());
                _instance._temptTestArtifectList.Add(id);
                Global_CampaignData._currentArtifactDictionary.Add(id, GlobalDataStorage.TotalArtifactClassDic[id]);
                Global_CampaignData._currentArtifactDictionary[id].OnGetArtifact();
                //ĵ������ ���̸� ���� ���ܵ�.
                ArtifactListShower.SetNewCaseOnList(id);
            }
            else 
            {
                Global_CampaignData._currentArtifactDictionary[id].AddCountOnArtifact();
                //ĵ������ ���̸� ���� ���ܵ�.
                ArtifactListShower.SetNumberOnCase(id, Global_CampaignData._currentArtifactDictionary[id].Value);
            }
        }



    }
}
