using System.Collections;
using UnityEngine;
using DG.Tweening;
using Mono.Collections.Generic;

namespace PG.Battle.FX
{

    [RequireComponent(typeof(TextMesh))]
    public class FloatingText : MonoBehaviour
    {
        public TextMesh textMesh;
        MeshRenderer _meshRenderer;
        public float LifeTime = 1;

        [SerializeField]
        Ease _fadeEase = Ease.InCirc;
        private void Awake()
        {
            textMesh = this.GetComponent<TextMesh>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.sortingLayerName = "BattleUpperUI";
            _meshRenderer.sortingOrder = 11;
        }


        public void SetText(string text, Color color)
        {
            textMesh.text = text;
            Vector3 _targetPos = new Vector2(Random.value, Random.value);
            _targetPos = transform.position + _targetPos.normalized;
            //transform.DOJump(_targetPos, 1f, 1, LifeTime);
            textMesh.color = color;
            StartCoroutine(DelayedFinish());
            //DOTween.ToAlpha(() => textMesh.color, x => textMesh.color = x, 0, LifeTime).
            //SetEase(_fadeEase).OnComplete(()=>DamageFXManager.FinishFX(gameObject));
        }

        IEnumerator DelayedFinish()
        {
            yield return new WaitForSeconds(LifeTime);
            DamageFXManager.FinishFX(gameObject);
        }

    }
}