using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceNegativeZ : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + Vector3.back, Vector3.up);
    }
}
