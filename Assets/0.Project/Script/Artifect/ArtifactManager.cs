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
        
        public static ArtifactScriptableData s_artifactData;
        [SerializeField]
        private ArtifactScriptableData artifactData;

        
        [SerializeField]
        bool _isTestSet = true;
        [SerializeField]
        List<ArtifactID> _showerArtifectList = new List<ArtifactID>() { };

        private HashSet<ArtifactID> completedIDset;


        //획득 가능한 아티팩트들의 리스트 여기서 
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

        //랜덤의 형성은 다음과 같아야한다. 
        //얻을 수 있는 업그레이드가 있을 경우

        void SetLevelUpOn() 
        {
            ArtifactSetRandomly();
            LevelUpPanelScript.LevelUpPannelOn();
        }
        void SetLevelUpOff() 
        {
            LevelUpPanelScript.LevelUpPannelOff();
        }

        //지금은 랜덤이지만. 일단 다음에는 일종의 
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



        //현재 가지고 있는 아티팩트를 전부 재 확인후 적용함,.
        void InitializeCurrentArtifact()
        {
            //전판의 데이터를 전부 지우고 넣음.
            DisableAllArtifact();
            if (Global_CampaignData._startArtifactList.Count != 0) 
            {
                foreach (ArtifactID id in Global_CampaignData._startArtifactList) 
                {
                    AddArtifactToPlayer_tempUse(id);
                }
            }
        }
        //유물 획득하는
        public static void AddArtifactToPlayer_tempUse(ArtifactID id)
        {
            //일단 유물은 딕셔너리에서 1개씩 얻을 수 있으며 여러번 넣어지면 그냥 숫자가 상승한다고 하자. 
            //그리고 해당하는 아이템을 얻었을 경우 
            if (Global_CampaignData._currentArtifactDictionary.ContainsKey(id) == false)
            {
                //Debug.Log("Upgrade" + id.ToString());
                _instance._showerArtifectList.Add(id);
                Global_CampaignData._currentArtifactDictionary.Add(id, 
                    Artifact.Create(id,ArtifactManager.s_artifactData.idArtifactDataDic[id]));
                Global_CampaignData._currentArtifactDictionary[id].OnGetArtifact();
                //캔버스의 분리를 위해 남겨둠.
                ArtifactListShower.SetNewCaseOnList(id);
                //또한 지금은 선택을 하고나면 기존의 데이터 셋에서 지워야함.
            }
            else 
            {
                Global_CampaignData._currentArtifactDictionary[id].AddCountOnArtifact();
                //캔버스의 분이를 위해 남겨둠.
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


        //게임이 초기화 되면 기존의 모든 아티팩트 내부의 데이터들을 제거한다.
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
