using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTracer : MonoBehaviour
{
    public static LineTracer instance;
    // Start is called before the first frame update
    [SerializeField]
    LineRenderer _linerenderer;
    [SerializeField]
    Transform _lineStart, _lineEnd;

    private void Awake(){instance = this;}

    public void SetDrawLineStart(Vector2 _startPosition)
    {
        _linerenderer.SetPosition(1, _startPosition);
    }
    public void SetDrawLineEnd(Vector2 _endPosition)
    {
        _linerenderer.SetPosition(0, _endPosition);
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
