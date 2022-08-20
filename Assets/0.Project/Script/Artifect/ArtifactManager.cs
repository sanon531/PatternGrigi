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
        void Start()
        {
            Global_BattleEventSystem._onBattleBegin += InitializeCurrentArtifact;
        }


        //���� ������ �ִ� ��Ƽ��Ʈ�� ���� �� Ȯ���� ������,.
        void InitializeCurrentArtifact() 
        {
            if(_isTestSet)
                foreach (ArtifactID id in _temptTestArtifectList) 
                {
                    GlobalDataStorage.TotalArtifactClassDic[id].ActiveArtifact();
                    //Debug.Log(id.ToString());
                }
        }
        public static void AddArtifactToPlayer_tempUse(ArtifactID id)
        {
            if (_instance._isTestSet) 
            {
                _instance._temptTestArtifectList.Add(id);
                GlobalDataStorage.TotalArtifactClassDic[id].ActiveArtifact();
            }




        }





    }
}
