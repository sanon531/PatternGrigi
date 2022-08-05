using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG.Event
{

    public static class GlobalUIEventSystem
    {

        public static bool _is���� = false;

        public static void CallOn��������ġ() 
        {
            if (_is����)
            {
                _is���� = false;
                CallOff����();
            }
            else
            {
                _is���� = true;
                CallOn����();
            }
        }

        public static event OnEvent _on����;
        private static void CallOn����() { _on����?.Invoke(); }
        public static event OnEvent _off����;
        private static void CallOff����() { _off����?.Invoke(); }



    }
}