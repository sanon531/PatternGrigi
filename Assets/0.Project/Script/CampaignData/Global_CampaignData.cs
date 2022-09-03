using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Battle;
using System.Linq;
namespace PG.Data
{
    public struct Data_CampaignOption
    {
        public Data_CampaignOption( CharacterID characterID, ArtifactID? artifactID, bool masterKeyUsed)
        {
            character = characterID;
            startArtifact = artifactID;
            this.masterKeyUsed = masterKeyUsed;
        }
        public CharacterID character { get; }
        public ArtifactID? startArtifact { get; }
        public bool masterKeyUsed { get; }
    }

    //인게임 내부에서 작동을 하는부분임.
    [System.Serializable]
    public static class Global_CampaignData
    {
        public static ArtifactIDArtifactDic _currentArtifactDictionary =
            new ArtifactIDArtifactDic();

        //public static Dictionary<ArtifactID, ArtifactData> _currentActivateDictionary =
        //new Dictionary<ArtifactID, ArtifactData>();
        public static Enemy_Script _currentEnemy;

        //적들의 데이터를 먼저 매치 하는 부분. 한번에 강한 공격 도 공격이지만 아무튼.
        //값은 언제든 수정이 가능.
        public static CharactorIDDataEntityDic _charactorAttackDic =new CharactorIDDataEntityDic();

        public static List<ArtifactID> _obtainableArtifactIDList = new List<ArtifactID>();

        #region//플레이 데이터 관련
        //거리에따른 배율임 
        public static DataEntity _lengthMagnData = new DataEntity(DataEntity.Type.LengthMag, 1);
        public static DataEntity _chargeGaugeData = new DataEntity(DataEntity.Type.ChargeGauge, 8);
        public static DataEntity _chargeEXPData = new DataEntity(DataEntity.Type.ChargeEXP, 8);
        //플레이어 사이즈
        public static DataEntity _playerSize = new DataEntity(DataEntity.Type.PlayerSize, 1);
        public static DataEntity _projectileSpeed = new DataEntity(DataEntity.Type.ProjectileSpeed, 5);
        public static DataEntity _projectileTargetNum = new DataEntity(DataEntity.Type.ProjectileCount, 1);

        public static List<float> _waveTimeList = new List<float>();
        public static List<WaveClass> _waveClassList = new List<WaveClass>();

        #endregion;

        #region



        #endregion

        #region//초기화 관련

        //완전히 게임이 종료 되었을 때 활용한다.데이터를 리셋하는경우에 활용한다.
        public static void ResetData() 
        {
            _currentArtifactDictionary.Clear();
        }

        //이곳에서 최초의 스테이지 데이터를 수정한다.
        public static void SetCampaginInitialize(CampaignData data) 
        {
            if(data._currentArtifactDictionary.Count !=0)
                _currentArtifactDictionary.CopyFrom(data._currentArtifactDictionary);
            //Debug.Log(data._charactorAttackDic.GetType());
            _charactorAttackDic.CopyFrom(data._charactorAttackDic);
            _obtainableArtifactIDList = data._obtainableArtifactIDList.ToList();

            _lengthMagnData = new DataEntity(data._lengthMagnData);
            //Debug.Log(data._lengthMagnData.FinalValue);
            _chargeGaugeData = new DataEntity(data._chargeGaugeData);
            _chargeEXPData = new DataEntity(data._chargeEXPData);
            _playerSize = new DataEntity(data._playerSize);
            _projectileSpeed = new DataEntity(data._projectileSpeed);
            _projectileTargetNum = new DataEntity(data._projectileTargetNum);

            _waveTimeList = new List<float>(data._waveDic.Keys);
            //켐페인데이터에 웨이브 시간 순서를 꼭 오름차순으로 입력 안해도 되도록 정렬+이에맞게 class리스트 만듦
            _waveTimeList.Sort();
            foreach (float key in _waveTimeList)
            {
                _waveClassList.Add(data._waveDic[key]);
                //Debug.Log(key);
            }
            //굳이 그럴필요없으면 이거 _waveClassList = new List<WaveClass>(data._waveDic.Values);
        }
        #endregion
    }

  


}