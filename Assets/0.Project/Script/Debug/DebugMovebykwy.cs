using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMovebykwy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform _moveLU, _moveRD;

    Vector2 _moveLUvec, _moveRDvec;


    void Start()
    {
        _moveLUvec = _moveLU.position;
        _moveRDvec = _moveRD.position;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 _curTF = transform.position;
        _curTF.x += Input.GetAxis("Horizontal") * Time.deltaTime * 20;
        _curTF.y += Input.GetAxis("Vertical") * Time.deltaTime * 20;
        _curTF.x = Mathf.Clamp(_curTF.x, _moveLUvec.x, _moveRDvec.x);
        _curTF.y = Mathf.Clamp(_curTF.y, _moveRDvec.y, _moveLUvec.y);
        transform.localPosition = _curTF;
    }
}
