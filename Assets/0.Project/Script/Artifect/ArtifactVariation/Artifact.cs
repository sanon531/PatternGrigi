using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using System;


namespace PG
{
    [Serializable]
    public class Artifact : Affector
    {

        #region//����
        [SerializeField]
        protected ArtifacProperty _properties;
        public ArtifacProperty Properties { get { return _properties; } }
        protected readonly ArtifactRarity _rarity;
        public ArtifactRarity Rarity { get { return _rarity; } }
        protected readonly ArtifactID thisID;
        public ArtifactID ThisID { get { return thisID; } }

        [SerializeField]
        protected int maxUpgrade = 5;
        public int MaxUpgrade { get => maxUpgrade; set => maxUpgrade = value; }

        [SerializeField]
        protected int _upgradeCount;
        public virtual int UpgradeCount { get { return _upgradeCount; } protected set 
            { 
                if(value <= maxUpgrade)
                _upgradeCount = value; 
            } 
        
        }
        private ArtifactFlag _flag;
        public ArtifactFlag Flag { get { return _flag; } }

        [SerializeField]
        string _devcomment;
        public string Devcomment { get { return _devcomment; } set { this._devcomment = value; } }

        [SerializeField]
        string _devcomment2;
        public string Devcomment2 { get { return _devcomment2; } set { this._devcomment2 = value; } }




        #endregion

        private Artifact() { }
        protected Artifact(ArtifactID artifactID)
        {
            thisID = artifactID;
            ArtifactData data = GlobalDataStorage.TotalArtifactTableDataDic[artifactID];
            _rarity = (ArtifactRarity)data.Rarity;
            _upgradeCount = data.UpgradeCount;

            _flag = ArtifactFlag.Inactive;
        }


        public virtual void OnGetArtifact()
        {
            //�� �Լ��� ������ ���� ȹ���Ҷ��� �ݵ� (�����Ѱ� �ε��Ҷ� �� �ȵ�)
            _upgradeCount++;
        }
        public void ActiveArtifact()
        {
            //if (_flag.HasFlag(ArtifactFlag.Active)) { return; }
            Enable();
        }
        public void DisposeArtifact()
        {
            //_flag |= ArtifactFlag.Inactive;
            for (int i = 0; i < _upgradeCount; i++)
                Disable();

            _upgradeCount = 0;
        }
        public virtual void AddCountOnArtifact()
        {
            _upgradeCount++;
        }



        public static Artifact Create(ArtifactID id)
        {
            Artifact result;
            try
            {
                string typeName = "Artifact_" + id;
                Debug.Log("Create Artifact : " + typeName);
                result = new Artifact(id);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw new System.Exception("���ǵ� ������ ã�� �� ����. ID : " + id);
            }
            return result;

        }
    }

}
