using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using PG.Event;
using PG.Data;
using UnityEngine.U2D;

namespace PG.Battle
{
    public class ArtifactManager : MonoSingleton<ArtifactManager>
    {
        #region VARIABLE

        public static ArtifactScriptableData s_artifactData;
        public static ArtifactMixtureData s_artifactMixData;

        [SerializeField] private ArtifactScriptableData artifactData;
        [SerializeField] private ArtifactMixtureData artifactMixData;


        [SerializeField] bool _isTestSet = true;
        [SerializeField] List<ArtifactID> _showerArtifectList = new List<ArtifactID>() { };

        private HashSet<ArtifactID> upgradableIDset;
        private HashSet<ArtifactID> completedIDset;
        private HashSet<ArtifactID> completeRequirementSet;

        #endregion

        /*유물 풀은 처음에 게임 시작하면 정리가 되며 일정 확률로 나오게되며
        만약 리스트에 없을 경우 상위 클래스의 오브젝트가 형성 된다.
        또 만약 풀이 형성 될 경우*/
        //획득 가능한 아티팩트들의 리스트 여기서 
        protected override void CallOnAwake()
        {
            LoadImageBeforePlaying();
            Global_BattleEventSystem._onBattleBegin += InitializeCurrentArtifact;
            Global_BattleEventSystem._onLevelUpShow += SetLevelUpOn;
            Global_BattleEventSystem._onLevelUpHide += SetLevelUpOff;
        }

        protected override void CallOnDestroy()
        {
            Global_BattleEventSystem._onBattleBegin -= InitializeCurrentArtifact;
            Global_BattleEventSystem._onLevelUpShow -= SetLevelUpOn;
            Global_BattleEventSystem._onLevelUpHide -= SetLevelUpOff;
        }

        //랜덤의 형성은 다음과 같아야한다. 
        //얻을 수 있는 업그레이드가 있을 경우

        #region ArtifactRelevant

        void SetLevelUpOn()
        {
            ArtifactSetRandomly();
            LevelUpPanelScript.LevelUpPannelOn();
        }

        void SetLevelUpOff()
        {
            LevelUpPanelScript.LevelUpPannelOff();
        }

        //Not only random increase percentage to get artifact gto already.
        void ArtifactSetRandomly()
        {
            _showerArtifectList.Clear();
            bool canGetNew = Global_CampaignData._currentArtifactDictionary.Count <
                             Global_CampaignData._totalMaxArtifactNumber;


            //만약 획득한 아이템이 있고 획득한 아이템이 업그레이드가 가능하다면 해당 요소 우선적으로 얻을수있게 만든다
            if (upgradableIDset.Count > 0)
            {
                if (canGetNew)
                {
                    var artifacts = Global_CampaignData._obtainableArtifactIDList.ToList();
                    var firstartifact = MyRandom.PickRandom(upgradableIDset);
                    artifacts.Remove(firstartifact);
                    _showerArtifectList.Add(firstartifact);
                    if(artifacts.Count>0)
                        _showerArtifectList.Add(MyRandom.PickRandom(artifacts));
                }
                else
                {
                    _showerArtifectList = MyRandom.PickRandoms(upgradableIDset, upgradableIDset.Count);
                }
            }
            //만약 획득한 아이템이 없을 경우 무작위로 선택해서 준다.
            else if (canGetNew)
            {
                _showerArtifectList = MyRandom.PickRandoms(Global_CampaignData._obtainableArtifactIDList, 2);
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
            if (artifactData is null || artifactMixData is null)
                throw new Exception(" ArtifactData isn't set");
            s_artifactData = artifactData;
            s_artifactMixData = artifactMixData;
            upgradableIDset = new HashSet<ArtifactID>();
            completedIDset = new HashSet<ArtifactID>();
            completeRequirementSet = new HashSet<ArtifactID>();
            foreach (var pair in artifactMixData.mixDic)
            {
                foreach (var id in pair.Value)
                {
                    completeRequirementSet.Add(id);
                }
            }


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
                _instance.upgradableIDset.Add(id);
                Global_CampaignData._currentArtifactDictionary.Add(id,
                    Artifact.Create(id, ArtifactManager.s_artifactData.idArtifactDataDic[id]));
                Global_CampaignData._currentArtifactDictionary[id].OnGetArtifact();
                //캔버스의 분리를 위해 남겨둠.
                //여기부분 수정해서 성능을 향상 시켜 보자.
                ArtifactListShower.SetNewCaseOnList(id);
                //또한 지금은 선택을 하고나면 기존의 데이터 셋에서 지워야함.
            }
            else
            {
                Global_CampaignData._currentArtifactDictionary[id].AddCountOnArtifact();
                //캔버스의 분이를 위해 남겨둠.
                ArtifactListShower.SetNumberOnCase(id,
                    Global_CampaignData._currentArtifactDictionary[id].ArtifactLevel);
            }
        }


        public static void RemoveArtifactOnPlayer(ArtifactID id)
        {
            while (Global_CampaignData._obtainableArtifactIDList.Contains(id))
            {
                Global_CampaignData._obtainableArtifactIDList.Remove(id);
            }

            _instance.completedIDset.Add(id);
            _instance.upgradableIDset.Remove(id);
            _instance.SearchArtifactMixtureAndSet(id);
        }

        private bool _addable = true;

        private void SearchArtifactMixtureAndSet(ArtifactID id)
        {
            if (completeRequirementSet.Contains(id))
            {
                foreach (var pair in artifactMixData.mixDic)
                {
                    _addable = true;
                    foreach (var pairId in pair.Value)
                    {
                        if (!completedIDset.Contains(pairId))
                            _addable = false;
                    }

                    if (_addable)
                    {
                        Global_CampaignData._obtainableArtifactIDList.Add(pair.Key);
                    }
                }
            }
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
            foreach (KeyValuePair<ArtifactID, Artifact> keyValuePair in Global_CampaignData._currentArtifactDictionary)
            {
                keyValuePair.Value.DisposeArtifact();
            }

            Global_CampaignData._currentArtifactDictionary.Clear();
        }

        #endregion


        #region ArtifactImage Load

        private SpriteAtlas _artifactAtlas;

        void LoadImageBeforePlaying()
        {
            _artifactAtlas = Resources.Load<SpriteAtlas>("Artifact/Artifact_Atlas");

            if (_artifactAtlas is null)
                throw new DataException("No Atlas is Exist");
        }

        public static Sprite GetSpriteFromImage(ArtifactID id)
        {
            Sprite _tempt = null;
            _tempt = _instance._artifactAtlas.GetSprite(id.ToString());
            if (_tempt is null)
                throw new DataException("No Sprite in Atlas is Exist" + id.ToString());

            return _tempt;
        }

        #endregion
    }
}