using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PG.Battle
{
    public class JoystickPlayerMover : MonoBehaviour
    {
        public float speed;
        public FixedJoystick variableJoystick;
        [SerializeField]
        Transform _moveLU, _moveRD;
        Vector2 _moveLUvec, _moveRDvec;
        [SerializeField]
        Rigidbody2D _thisRB;

        private void Awake()
        {
            _moveLUvec = _moveLU.position;
            _moveRDvec = _moveRD.position;

        }
        public void FixedUpdate()
        {
            LineTracer.instance.SetDrawLineStart(transform.position);

            Vector3 _direction = Vector3.up * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
            _direction = _direction.normalized;
            _thisRB.velocity = _direction * speed;
            Vector3 _curTF = transform.position;
            _curTF.x = Mathf.Clamp(_curTF.x, _moveLUvec.x, _moveRDvec.x);
            _curTF.y = Mathf.Clamp(_curTF.y, _moveRDvec.y, _moveLUvec.y);
            transform.position = _curTF;
        }
    }

}