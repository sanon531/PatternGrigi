using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TouchFollowScript : MonoBehaviour
{

    [SerializeField]
    Vector3 _touchPosition;
    [SerializeField]
    Rigidbody2D _thisRB;
    Vector3 _direction;
    [SerializeField]
    float _moveSpeed = 10f;

    // Start is called before the first frame update


    [SerializeField]
    Transform _moveLU, _moveRD, _touchLU;

    Vector2 _moveLUvec, _moveRDvec, _touchLUvec;
    void Awake()
    {
        _thisRB = GetComponent<Rigidbody2D>();
        _moveLUvec = _moveLU.position;
        _moveRDvec = _moveRD.position;
        _touchLUvec = _touchLU.position;
    }






    // Update is called once per frame
    void Update()
    {
        //터치가능 영역만을 설정할수있도록 만든다.
        //절반아래일때만
        LineTracer.instance.SetDrawLineStart(transform.position);

        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            _touchPosition = Camera.main.ScreenToWorldPoint(_touch.position);
            SetPlayerPos(_touchPosition);

            if (_touch.phase == TouchPhase.Ended)
                _thisRB.velocity = Vector2.zero;
        }
        else
        {

        }



    }

    //터치 상한선과 작동 상한선 두가지로 나누자.
    void SetPlayerPos(Vector3 _targetPos)
    {
        //먼저 현재 캐릭터가 제한 범위를 넘어갈경우 잡아주는걸 해야한다.
        
        if (_touchLUvec.x < _targetPos.x && _moveRDvec.x > _targetPos.x &&
        _touchLUvec.y > _targetPos.y && _moveRDvec.y < _targetPos.y)
        {
            _targetPos.z = 0;
            _direction = _targetPos - transform.position;
            _direction = _direction.normalized;
            _thisRB.velocity = new Vector2(_direction.x, _direction.y) * _moveSpeed;
        }
        else
            _thisRB.velocity = Vector2.zero;

        Vector3 _curTF = transform.position;
        _curTF.x = Mathf.Clamp(_curTF.x, _moveLUvec.x, _moveRDvec.x);
        _curTF.y = Mathf.Clamp(_curTF.y, _moveRDvec.y, _moveLUvec.y);
        transform.position = _curTF;
    }



}
