using UnityEngine;
using DG.Tweening;
namespace PG.Battle.FX
{

    [RequireComponent(typeof(TextMesh))]
    public class FloatingText : PoolableObject
    {
        public TextMesh textMesh;
        public float LifeTime = 1;

        [SerializeField]
        Ease _fadeEase = Ease.InCirc;
        private void Awake()
        {
            textMesh = this.GetComponent<TextMesh>();
        }


        public void SetText(string text, Color color)
        {
            if (textMesh)
                textMesh.text = text;
            Vector3 _targetPos = new Vector2(Random.value, Random.value);
            _targetPos = transform.position + _targetPos.normalized;
            transform.DOJump(_targetPos, 1f, 1, LifeTime);
            if (textMesh)
                textMesh.color = color;
            DOTween.ToAlpha(() => textMesh.color, x => textMesh.color = x, 0, LifeTime).
                SetEase(_fadeEase).OnComplete(()=>DamageFXManager.FinishFX(gameObject));

        }


    }
}