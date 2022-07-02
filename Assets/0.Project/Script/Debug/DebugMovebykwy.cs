using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMovebykwy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    Vector3 Vec;
    // Update is called once per frame
    void Update()
    {
        Vec = transform.localPosition;
        Vec.x += Input.GetAxis("Horizontal") * Time.deltaTime * 20;
        Vec.y += Input.GetAxis("Vertical") * Time.deltaTime * 20;
        transform.localPosition = Vec;
    }
}
