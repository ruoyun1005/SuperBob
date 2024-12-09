using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatBanana : MonoBehaviour
{
    // Start is called before the first frame update
    public float floatSpeed = 1f; // 懸浮速度
    public float floatAmplitude = 0.5f; // 懸浮振幅
    public float rotationSpeed = 50f; // 旋轉速度
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // 持續旋轉效果
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
