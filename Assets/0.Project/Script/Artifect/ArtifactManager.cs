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
        void Start()
        {
            Global_BattleEventSystem._onBattleBegin += InitializeCurrentArtifact;
        }


        //현재 가지고 있는 아티팩트를 전부 재 확인후 적용함,.
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
