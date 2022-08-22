using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
using PG.Data;

namespace PG.Battle
{
    public class ArtifactManager : MonoSingleton<ArtifactManager>
    {

        //유물 풀은 처음에 게임 시작하면 정리가 되며 일정 확률로 나오게되며
        //만약 리스트에 없을 경우 상위 클래스의 오브젝트가 형성 된다.
        //또 만약 풀이 형성 될 경우
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


        //현재 가지고 있는 아티팩트를 전부 재 확인후 적용함,.
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
                //여기는 아예 전달 받은걸로 만든다.
                //GlobalDataStorage.TotalArtifactTableDataDic;
            }
        }
        //유물 획득하는
        public static void AddArtifactToPlayer_tempUse(ArtifactID id)
        {
            //일단 유물은 딕셔너리에서 1개씩 얻을 수 있으며 여러번 넣어지면 그냥 숫자가 상승한다고 하자. 
            //미래에는 그냥 간단하게
            if (Global_CampaignData._currentArtifactDictionary.ContainsKey(id) == false)
            {
                Debug.Log("Upgrade" + id.ToString());
                _instance._temptTestArtifectList.Add(id);
                Global_CampaignData._currentArtifactDictionary.Add(id, GlobalDataStorage.TotalArtifactClassDic[id]);
                Global_CampaignData._currentArtifactDictionary[id].OnGetArtifact();
                //캔버스의 분이를 위해 남겨둠.
                ArtifactListShower.SetNewCaseOnList(id);
            }
            else 
            {
                Global_CampaignData._currentArtifactDictionary[id].AddCountOnArtifact();
                //캔버스의 분이를 위해 남겨둠.
                ArtifactListShower.SetNumberOnCase(id, Global_CampaignData._currentArtifactDictionary[id].Value);
            }
        }



    }
}
