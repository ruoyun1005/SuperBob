using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBanana : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform handTransform; // 玩家手部的 Transform
    public float throwForce = 10f; // 丟出的力量
    private GameObject heldBanana; // 手上握住的香蕉
    void Start()
    {
        if (Input.GetKeyDown(KeyCode.E) && heldBanana != null)
        {
            ThrowHeldBanana();
        }
    }
    public void PickUpBanana(GameObject banana)
    {
        Debug.Log("Picking up banana: " + banana.name);
        // 將香蕉設為手上的物件
        heldBanana = banana;
        heldBanana.transform.position = handTransform.position;
        heldBanana.transform.rotation = handTransform.rotation;
        heldBanana.transform.SetParent(handTransform);

        // 停止香蕉的物理效果
        Rigidbody rb = heldBanana.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    // Update is called once per frame
    private void ThrowHeldBanana()
    {
        if (heldBanana == null) return;

        // 將香蕉從手上移除
        heldBanana.transform.SetParent(null);

        // 添加物理效果，讓香蕉飛出
        Rigidbody rb = heldBanana.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(handTransform.forward * throwForce, ForceMode.Impulse);
        }

        Debug.Log("Banana thrown!");
        heldBanana = null; // 清除手上的香蕉
    }
}
