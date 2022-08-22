using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(TextMesh))]
public class FloatingText : MonoBehaviour
{
    public TextMesh textMesh;
    public float LifeTime = 1;

    private float timeTemp = 0;
    [SerializeField]
    Ease _fadeEase = Ease.InCirc;
    private void Awake()
    {
        textMesh = this.GetComponent<TextMesh>();
    }

    void Start()
    {
        timeTemp = Time.time;
    }

    public void SetText(string text)
    {
        if (textMesh)
            textMesh.text = text;
    }
    public void SetColor(Color color)
    {
        if (textMesh)
            textMesh.color= color;

        DOTween.ToAlpha(() => textMesh.color, x => textMesh.color =x, 0, 1.25f).SetEase(_fadeEase);
    }

}
