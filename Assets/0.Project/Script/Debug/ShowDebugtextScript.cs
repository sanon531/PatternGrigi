using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;

namespace PG 
{
    public class ShowDebugtextScript : MonoSingleton<ShowDebugtextScript >
    {
        // Start is called before the first frame update
        [SerializeField]
        Text _debugshower;
        [SerializeField]
        Text _debugshower2;



        [SerializeField] private RectTransform thisImage;
        private bool isHide = false;
        private void Update()
        {
            //Debug.Log("P");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Global_BattleEventSystem.CallTotalPauseSwitch();
                Debug.Log("Paused button");
                if (isHide)
                {
                    isHide = false;
                    thisImage.localPosition = new Vector2();
                }
                else
                {                    
                    isHide = true;
                    thisImage.localPosition = new Vector2(-1000,0);
                    
                }


            }
            if (Input.GetKeyDown(KeyCode.Y))
                Global_BattleEventSystem.CallOnGameOver();
        }


        public static void SetDebug(string _str)
        {
            _instance._debugshower.text = "_";
            _instance._debugshower.text = _str;
        }
        public static void SetDebug2(string _str)
        {
            _instance._debugshower2.text = "Total_";
            _instance._debugshower2.text += _str;
        }

        private int accumulateDamage = 0;
        public static void ShowCurrentAccumulateDamage(int damage)
        {
            _instance.accumulateDamage += damage;
            SetDebug2(_instance.accumulateDamage.ToString());
        }



    }

}

