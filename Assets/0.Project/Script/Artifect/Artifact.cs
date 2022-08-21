using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using System;


namespace PG 
{
    public class Artifact : Affector
    {

        #region//변수

        protected ArtifacProperty _properties;
        public ArtifacProperty Properties { get { return _properties; } }
        protected readonly ArtifactRarity _rarity;
        public ArtifactRarity Rarity { get { return _rarity; } }
        protected readonly ArtifactID _artifactID;
        public ArtifactID _ArtifactID { get { return _artifactID; } }
        protected int _value;
        public virtual int Value{get { return _value; }protected set { _value = value; }}
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
            _artifactID = artifactID;
            ArtifactData data = GlobalDataStorage.TotalArtifactTableDataDic[artifactID];
            _rarity = (ArtifactRarity)data.Rarity;
            _value = data.Value;

            _flag = ArtifactFlag.Inactive;
        }


        public virtual void OnGetArtifact() {
            //이 함수는 유물을 직접 획득할때만 콜됨 (저장한거 로드할땐 콜 안됨)
        }
        public void ActiveArtifact() {
            //if (_flag.HasFlag(ArtifactFlag.Active)) { return; }
            Enable();
        }
        public void DisposeArtifact() {
            //_flag |= ArtifactFlag.Inactive;
            Disable();
        }
        public virtual void AddCountOnArtifact() 
        {
        
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
                throw new System.Exception("정의된 유물을 찾을 수 없음. ID : " + id);
            }
            return result;

        }
    }

}
