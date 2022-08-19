using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using System;


namespace PG 
{
    public class Artifact : Affector
    {
        public enum Property
        {
            None = 0,
            CountValue = 1,
        }

        public enum Flag
        {
            Active = 0,
            Inactive = 1,
        }

        private Artifact() { }
        protected Artifact(ArtifactID artifactID)
        {
            _artifactID = artifactID;
            ArtifactTableData data = GlobalDataStorage.TotalArtifactTableDataDic[artifactID]; 
            _rarity = (Rarity)data.Rarity;
            _value = data.Value;
            _flag = Flag.Active;
        }


        protected Property _properties;
        public Property properties { get { return _properties; } }
        protected readonly Rarity _rarity;
        public Rarity rarity { get { return _rarity; } }
        protected readonly ArtifactID _artifactID;
        public ArtifactID artifactID { get { return _artifactID; } }
        protected int _value;
        public virtual int Value
        {
            get { return _value; }
            protected set { _value = value; }
        }
        private Flag _flag;
        public Flag flag { get { return _flag; } }

        public virtual void OnGetArtifact() {
            //이 함수는 유물을 직접 획득할때만 콜됨 (저장한거 로드할땐 콜 안됨)
        }
        public void ActiveArtifact() {
            if (_flag.HasFlag(Flag.Active)) { return; }
            Enable();
        }
        public void DisposeArtifact() {
            _flag |= Flag.Inactive;
            Disable();
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
