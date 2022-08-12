using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;

namespace PG.Battle
{
    public class TouchFollowScript : MonoBehaviour, ISetNontotalPause
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
            Global_BattleEventSystem._onNonTotalPause += SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause += SetNonTotalPauseOff;
        }
        private void OnDestroy()
        {
            Global_BattleEventSystem._onNonTotalPause -= SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause -= SetNonTotalPauseOff;
        }





        // Update is called once per frame
        void Update()
        {
            //��ġ���� �������� �����Ҽ��ֵ��� �����.
            //���ݾƷ��϶���
            LineTracer.instance.SetDrawLineStart(transform.position);
            if (Input.GetMouseButtonDown(0)) 
                _isClicked = true;
            else if(Input.GetMouseButtonUp(0))
                _isClicked = false;


            if (_isClicked)
                CallClickProcess();

            if (Input.touchCount > 0)
            {
                Touch _touch = Input.GetTouch(0);
                _touchPosition = Camera.main.ScreenToWorldPoint(_touch.position);
                SetPlayerPos(_touchPosition);

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
            _touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetPlayerPos(_touchPosition);


        }



        bool _isLevelUpPaused = false;

        //��ġ ���Ѽ��� �۵� ���Ѽ� �ΰ����� ������.
        void SetPlayerPos(Vector3 targetPos)
        {
            //���� ���� ĳ���Ͱ� ���� ������ �Ѿ��� ����ִ°� �ؾ��Ѵ�.

            if (_isLevelUpPaused)
                return;


            if (PositionCheck(targetPos))
            {

                targetPos.z = 0;
                _direction = targetPos - transform.position;
                if (_direction.magnitude < 0.3f)
                {
                    transform.position = targetPos;
                    _thisRB.velocity = new Vector2();
                }
                else 
                {
                    _direction = _direction.normalized;
                    _thisRB.velocity = new Vector2(_direction.x, _direction.y) * _moveSpeed;
                }

            }
            else
                _thisRB.velocity = Vector2.zero;

            Vector3 _curTF = transform.position;
            _curTF.x = Mathf.Clamp(_curTF.x, _moveLUvec.x, _moveRDvec.x);
            _curTF.y = Mathf.Clamp(_curTF.y, _moveRDvec.y, _moveLUvec.y);
            transform.position = _curTF;
        }

        bool PositionCheck(Vector2 targetPos) 
        {

            return 
                _touchLUvec.x < targetPos.x && 
                _moveRDvec.x > targetPos.x && 
                _touchLUvec.y > targetPos.y && 
                _moveRDvec.y < targetPos.y;


        }


        public void SetNonTotalPauseOn() { _isLevelUpPaused = true; }
        public void SetNonTotalPauseOff() { _isLevelUpPaused = false; }


    }
}