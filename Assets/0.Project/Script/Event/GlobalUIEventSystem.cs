using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG.Event
{

    public static class GlobalUIEventSystem
    {

        public static bool _is암전 = false;

        public static void CallOn암전스위치() 
        {
            if (_is암전)
            {
                _is암전 = false;
                CallOff암전();
            }
            else
            {
                _is암전 = true;
                CallOn암전();
            }
        }

        public static event OnEvent _on암전;
        private static void CallOn암전() { _on암전?.Invoke(); }
        public static event OnEvent _off암전;
        private static void CallOff암전() { _off암전?.Invoke(); }



    }
}