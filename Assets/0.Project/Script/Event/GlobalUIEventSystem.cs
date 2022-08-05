using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG.Event
{

    public static class GlobalUIEventSystem
    {

        public static bool _isTotalFade = false;

        public static void CallTotalFade() 
        {
            if (_isTotalFade)
            {
                _isTotalFade = false;
                CallOnFadeIn();
            }
            else
            {
                _isTotalFade = true;
                CallOnFadeOut();
            }
        }

        public static event OnEvent _onFadeOut;
        private static void CallOnFadeOut() { _onFadeOut?.Invoke(); }
        public static event OnEvent _onFadeIn;
        private static void CallOnFadeIn() { _onFadeIn?.Invoke(); }

        public static bool _isCharge = false;
        public static void CallChargeEvent()
        {
            if (_isCharge)
            {
                _isCharge = false;
                CallOffChargeStart();
            }
            else
            {
                _isCharge = true;
                CallOnChargeStart();
            }
        }

        public static event OnEvent _onCallChargeStart;
        private static void CallOnChargeStart() { _onCallChargeStart?.Invoke(); }
        public static event OnEvent _offChargeStart;
        private static void CallOffChargeStart() { _offChargeStart?.Invoke(); }



    }
}