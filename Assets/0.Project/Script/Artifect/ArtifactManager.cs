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

        /*���� Ǯ�� ó���� ���� �����ϸ� ������ �Ǹ� ���� Ȯ���� �����ԵǸ�
        ���� ����Ʈ�� ���� ��� ���� Ŭ������ ������Ʈ�� ���� �ȴ�.
        �� ���� Ǯ�� ���� �� ���*/
        //ȹ�� ������ ��Ƽ��Ʈ���� ����Ʈ ���⼭ 
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

        //������ ������ ������ ���ƾ��Ѵ�. 
        //���� �� �ִ� ���׷��̵尡 ���� ���

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


            //���� ȹ���� �������� �ְ� ȹ���� �������� ���׷��̵尡 �����ϴٸ� �ش� ��� �켱������ �������ְ� �����
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
            //���� ȹ���� �������� ���� ��� �������� �����ؼ� �ش�.
            else if (canGetNew)
            {
                _showerArtifectList = MyRandom.PickRandoms(Global_CampaignData._obtainableArtifactIDList, 2);
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


        //���� ȹ���ϴ�
        public static void AddArtifactToPlayer_tempUse(ArtifactID id)
        {
            //�ϴ� ������ ��ųʸ����� 1���� ���� �� ������ ������ �־����� �׳� ���ڰ� ����Ѵٰ� ����. 
            //�׸��� �ش��ϴ� �������� ����� ��� 
            if (Global_CampaignData._currentArtifactDictionary.ContainsKey(id) == false)
            {
                //Debug.Log("Upgrade" + id.ToString());
                _instance._showerArtifectList.Add(id);
                _instance.upgradableIDset.Add(id);
                Global_CampaignData._currentArtifactDictionary.Add(id,
                    Artifact.Create(id, ArtifactManager.s_artifactData.idArtifactDataDic[id]));
                Global_CampaignData._currentArtifactDictionary[id].OnGetArtifact();
                //ĵ������ �и��� ���� ���ܵ�.
                //����κ� �����ؼ� ������ ��� ���� ����.
                ArtifactListShower.SetNewCaseOnList(id);
                //���� ������ ������ �ϰ��� ������ ������ �¿��� ��������.
            }
            else
            {
                Global_CampaignData._currentArtifactDictionary[id].AddCountOnArtifact();
                //ĵ������ ���̸� ���� ���ܵ�.
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


        //������ �ʱ�ȭ �Ǹ� ������ ��� ��Ƽ��Ʈ ������ �����͵��� �����Ѵ�.
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