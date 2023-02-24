using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using System;
using System.Reflection;
using UnityEngine.Serialization;


namespace PG
{
    [Serializable]
    public class Artifact : Affector
    {
        #region //변수

        [SerializeField] protected ArtifacProperty _properties;

        public ArtifacProperty Properties
        {
            get { return _properties; }
        }

        protected ArtifactRarity _rarity;

        public ArtifactRarity Rarity
        {
            get { return _rarity; }
            set { _rarity = value; }
        }

        protected readonly ArtifactID thisID;
        public ArtifactID ThisID
        {
            get { return thisID; }
        }

       
        [FormerlySerializedAs("maxUpgrade")] [SerializeField] protected int maxLevel;

        public int MaxLevel
        {
            get => maxLevel;
            set => maxLevel = value;
        }

        [FormerlySerializedAs("_upgradeCount")] 
        [SerializeField] protected int _artifactLevel;

        public virtual int ArtifactLevel
        {
            get { return _artifactLevel; }
            set
            {
                if (value <= maxLevel)
                    _artifactLevel = value;
                else
                    CompleteArtifact();
            }
        }

        private ArtifactFlag _flag;

        public ArtifactFlag Flag
        {
            get { return _flag; }
        }

        [SerializeField] string _devcomment;

        public string Devcomment
        {
            get { return _devcomment; }
            set { this._devcomment = value; }
        }

        [SerializeField] string _devcomment2;

        public string Devcomment2
        {
            get { return _devcomment2; }
            set { this._devcomment2 = value; }
        }

        
        
        [FormerlySerializedAs("UpgradeValueList")] [SerializeField]
        protected List<float> ArfifactLevelValueList;
        [FormerlySerializedAs("UpgradeValueList")] [SerializeField]
        protected List<float> ArfifactLevelValueList2;

        #endregion

        public Artifact(ArtifactID artifactID)
        {
            thisID = artifactID;
        }
        
        

        public void SetData(ArtifactData data)
        {
            Rarity = (ArtifactRarity)data.Rarity;
            ArtifactLevel = data.ArtifactLevel;
            MaxLevel = data.MaxLevel;
            _flag = ArtifactFlag.Inactive;
            ArfifactLevelValueList = data.ArfifactLevelValueList;
            ArfifactLevelValueList2 = data.ArfifactLevelValueList2;
        }


        public virtual void OnGetArtifact()
        {
            //이 함수는 유물을 직접 획득할때만 콜됨 (저장한거 로드할땐 콜 안됨)
            ArtifactLevel++;
        }

        public void ActiveArtifact()
        {
            //if (_flag.HasFlag(ArtifactFlag.Active)) { return; }
            Enable();
        }

        public void DisposeArtifact()
        {
            //_flag |= ArtifactFlag.Inactive;
            for (int i = 0; i < _artifactLevel; i++)
                Disable();

            _artifactLevel = 0;
        }

        public virtual void AddCountOnArtifact()
        {
            ArtifactLevel++;
        }

        protected bool alreadyCompleted = false;

        public virtual void CompleteArtifact()
        {
            if (alreadyCompleted)
                return;
            alreadyCompleted = true;
            Battle.ArtifactManager.RemoveArtifactOnPlayer(thisID);
        }


        public static Artifact Create(ArtifactID id,ArtifactData data)
        {
            Artifact temp;
            var type = Type.GetType("PG.Artifact_" + id.ToString());
            if (type != null)
            {
                temp = (Artifact)Activator.CreateInstance(type);
                temp.SetData(data);
                return temp;
            }

            type = Assembly.GetExecutingAssembly().GetType("PG.Artifact_" + id.ToString());
            if (type != null)
            {
                temp = (Artifact)Activator.CreateInstance(type);
                temp.SetData(data);
                return temp;
            }

            throw new ArgumentException("No Artifact" + "PG.Artifact_" + id.ToString());
            return null;
        }
        
        
        
    }
}