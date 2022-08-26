using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public Vector3 PositionMult;
    public Transform PositionDirection;
    private Vector3 positionTemp;

    void Start()
    {
        positionTemp = this.transform.position;
    }

    void FixedUpdate()
    {
        positionTemp += PositionDirection.position * Time.deltaTime;
        //PositionMult += PositionMult * Time.deltaTime;
        this.transform.position = Vector3.Lerp(this.transform.position, positionTemp, 0.5f);

    }
}
