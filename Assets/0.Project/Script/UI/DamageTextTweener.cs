using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PG.Battle 
{
    public class DamageTextTweener : MonoBehaviour
    {
        private Vector3 _positionTemp;
        public Transform _positionDirection;

        Tweener tweener;
        public void BeginTextTweeener(Transform targetTransform,Transform middle,float time) 
        {
            _positionDirection = targetTransform;
            tweener = transform.DOMove(middle.position, time);
            tweener.onComplete += AfterReach;
            transform.DOScale(new Vector3(2, 2, 2), time);
        }



        void AfterReach() 
        {
            transform.DOMove(_positionDirection.position, 0.1f);
        }
    }

}
