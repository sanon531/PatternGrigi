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
    float moveSpeed = 10f;
    [SerializeField]

    // Start is called before the first frame update
    void Awake()
    {
        _thisRB = GetComponent<Rigidbody2D>();
    }




    // Update is called once per frame
    void Update()
    {
        //터치가능 영역만을 설정할수있도록 만든다.
        //절반아래일때만
        LineTracer.instance.SetDrawLineStart(transform.position);

        if (Input.touchCount > 0){
            Touch _touch = Input.GetTouch(0);
            _touchPosition = Camera.main.ScreenToWorldPoint(_touch.position);
            _touchPosition.z = 0;
            _direction = _touchPosition - transform.position;
            _direction = _direction.normalized;
            _thisRB.velocity = new Vector2(_direction.x, _direction.y) * moveSpeed;
            if (_touch.phase == TouchPhase.Ended)
                _thisRB.velocity = Vector2.zero;

            

        }
        else {
        
        }



    }


  

}
