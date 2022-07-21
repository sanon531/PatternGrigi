using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG.Battle
{
    public class Obstacle : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public float _damageDeal = 8f;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                Player_Script.Damage(_damageDeal);
        }

    }
}