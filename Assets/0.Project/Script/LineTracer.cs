using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineTracer : MonoBehaviour
{
    public static LineTracer instance;
    // Start is called before the first frame update
    [SerializeField]
    LineRenderer _linerenderer;
    [SerializeField]
    Transform _lineStart, _lineEnd;
    [SerializeField]
    TextMeshPro _textForShowRatio;
    Vector2 _lastLineStartPos, _lastLineEndPos = new Vector2();


    private void Awake(){instance = this;}

    //여기 내부에서 현재 배율에대한정보를 그냥 패턴 매니져에서 담당함. 
    // 그리고 여기서ㅡㄴ 위치에 대한 정보만 담당하여서 배율을 처리하지않음으로 효율성을 늘린다
    public void SetDrawLineStart(Vector2 _startPosition)
    {
        _linerenderer.SetPosition(1, _startPosition);
        _lastLineStartPos = _startPosition;
        _textForShowRatio.transform.position = _lastLineEndPos + (_lastLineStartPos - _lastLineEndPos) /2 ;
        _textForShowRatio.SetText(Math.Round(GetLengthRatio(), 1).ToString());
    }
    public void SetDrawLineEnd(Vector2 _endPosition)
    {
        _linerenderer.SetPosition(0, _endPosition);
        _lastLineEndPos = _endPosition;
    }

    float GetLengthRatio()
    {
        float _xval = Mathf.Pow(_lastLineStartPos.x - _lastLineEndPos.x, 2);
        float _yval = Mathf.Pow(_lastLineStartPos.y - _lastLineEndPos.y, 2);

        return Mathf.Sqrt(_xval + _yval);
    }


    void ReSetDrawLine()
    {
        _linerenderer.SetPosition(0, new Vector3());
        _linerenderer.SetPosition(1, new Vector3());
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
