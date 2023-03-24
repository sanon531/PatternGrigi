using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PG.Battle
{
    public class ProjectileRestrictionSetter : MonoBehaviour
    {
        [Header("Get Target Pos")]
        [SerializeField]
        private Transform luTransform;
        [SerializeField]
        private Transform rdTransform;

        [Header("Set Target Pos")]
        [SerializeField]
        private Transform leftSide;

        [SerializeField]
        private Transform rightSide;

        [SerializeField]
        private Transform upperSide;

        [SerializeField]
        private Transform downSide;

        void Start()
        {

            float left = luTransform.position.x*0.8f;
            float up = luTransform.position.y;
            float right = rdTransform.position.x;
            float down = rdTransform.position.y*0.8f;
            leftSide.position = new Vector2(left , (up + down)/2);
            rightSide.position = new Vector2(right , (up + down)/2);
            upperSide.position = new Vector2(0 , up);
            downSide.position = new Vector2(0 , down);

        }
    }
}