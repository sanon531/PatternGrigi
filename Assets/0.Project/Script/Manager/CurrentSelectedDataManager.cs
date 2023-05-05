using PG.Data;
using UnityEngine;

namespace PG
{
    public class CurrentSelectedDataManager : MonoSingleton<CurrentSelectedDataManager>
    {
        public static StartCondition CurrentCondition = StartCondition.None;
        public StartConditionCampaignDic startConditionCampaignDictionary = new StartConditionCampaignDic();
        public static void SetCurrentCondition(StartCondition val)
        {
            CurrentCondition = val;
        }

        public static CampaignSelectingData GetCurrentCondition()
        {
            return _instance.startConditionCampaignDictionary[CurrentCondition];
        }

    }
}
