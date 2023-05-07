using System.Collections;
using System.Collections.Generic;
using PG.Data;
using UnityEngine;
using PG.Event;

namespace PG.Battle
{
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

        [SerializeField]
        bool _isCamOrthoGraph = false;

        [SerializeField]
        Camera mainCam;

        void Awake()
        {
            _thisRB = GetComponent<Rigidbody2D>();
            _moveLUvec = _moveLU.position;
            _moveRDvec = _moveRD.position;
            _touchLUvec = _touchLU.position;
            _isCamOrthoGraph  = mainCam.orthographic;
        }
        private void OnDestroy()
        {
        }





        // Update is called once per frame
        void Update()
        {

            if (Global_CampaignData._gameOver || Global_CampaignData._gameCleared)
                return;
            LineTracer._instance.SetDrawLineStart(transform.position);
            //클릭 담당 부분.
            if (Input.GetMouseButtonDown(0)) 
                _isClicked = true;
            else if(Input.GetMouseButtonUp(0))
                _isClicked = false;

            if (_isClicked)
                CallClickProcess();
            else
                _thisRB.velocity = Vector2.zero;



            //터치 담당 부분.
            if (Input.touchCount > 0)
            {
                Touch _touch = Input.GetTouch(0);
                if (_isCamOrthoGraph)
                {
                    _touchPosition = mainCam.ScreenToWorldPoint(_touch.position);
                    SetPlayerPos(_touchPosition);
                }
                else 
                {
                    Ray ray = mainCam.ScreenPointToRay(_touch.position); 
                    if(Physics.Raycast(ray,out RaycastHit rayhit))
                        _touchPosition = rayhit.point;

                    SetPlayerPos(_touchPosition);
                }


                //ShowDebugtextScript.SetDebug(_touchPosition + " -> " + transform.position);
                //ShowDebugtextScript.SetDebug2(_touchLUvec +","+ _moveRDvec + " , " + PositionCheck(_touchPosition));

                if (_touch.phase == TouchPhase.Ended)
                    _thisRB.velocity = Vector2.zero;
            }
            else
            {

            }
        }

        bool _isClicked = false;

        void CallClickProcess() 
        {
            if (_isCamOrthoGraph)
            {
                _touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SetPlayerPos(_touchPosition);
            }
            else
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit rayhit))
                    _touchPosition = rayhit.point;

                SetPlayerPos(_touchPosition);
            }

        }

        

        bool _isLevelUpPaused = false;

[SerializeField]
        private float _teleportLength = 0.5f;

        public void SetSpeedAndTeleport(float speed,float teleport)
        {
            _moveSpeed = speed;
            _teleportLength = teleport;

        }
        //터치 상한선과 작동 상한선 두가지로 나누자.
        void SetPlayerPos(Vector3 targetPos)
        {
            //먼저 현재 캐릭터가 제한 범위를 넘어갈경우 잡아주는걸 해야한다.

            if (_isLevelUpPaused)
                return;

            _thisRB.velocity = Vector2.zero;
            targetPos.z = 0;

            _direction = targetPos - transform.position;

/*            if (PositionCheckX(targetPos))
            {
                if (_direction.magnitude < _teleportLength)
                {
                    transform.position = targetPos;
                }
                else
                {
                    _direction.x = _direction.normalized.x;
                }
            } 

            if (PositionCheckY(targetPos)) 
            {
                if (_direction.magnitude < _teleportLength)
                {
                    transform.position = targetPos;
                }
                else
                {
                    _direction.y = _direction.normalized.y;
                }
            }*/
            transform.Translate(_direction * (Time.deltaTime * _moveSpeed));
            Vector3 _curTF = transform.position;
            _curTF.x = Mathf.Clamp(_curTF.x, _moveLUvec.x, _moveRDvec.x);
            _curTF.y = Mathf.Clamp(_curTF.y, _moveRDvec.y, _moveLUvec.y);
            transform.position = _curTF;
        }

        bool PositionCheckX(Vector2 targetPos) 
        {

            return 
                _touchLUvec.x < targetPos.x && 
                _moveRDvec.x > targetPos.x;
        }
        bool PositionCheckY(Vector2 targetPos) 
        {
            return 
                _touchLUvec.y > targetPos.y && 
                _moveRDvec.y < targetPos.y;
        }




    }
}