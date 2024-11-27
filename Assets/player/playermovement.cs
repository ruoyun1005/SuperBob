using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
   public float speed = 0.01f;

    void Start()
    {

    }

    void Update()
    {

            if (Input.GetKey("w")) { transform.Translate(0, 0, speed); }

            if (Input.GetKey("s")) { transform.Translate(0, 0, -speed); }


            if (Input.GetKey("a")) { transform.Translate(-speed, 0, 0); }

            if (Input.GetKey("d")) { transform.Translate(speed, 0, 0); }
    }

    

}
