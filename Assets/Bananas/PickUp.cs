using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Start is called before the first frame update
    public string playerTag = "Player"; // 玩家物件的標籤
    public Transform handTransform; // 玩家手部的 Transform
    private bool isPickedUp = false; // 是否已經被撿起
    public GameObject pickupEffect; // 可選：撿取效果
    void OnTriggerEnter(Collider other)
    {
        if (!isPickedUp && other.CompareTag("Player"))
        {
            Debug.Log("Trigger Entered: " + other.name);
            // 找到玩家的丟香蕉腳本
            ThrowBanana throwScript = other.GetComponent<ThrowBanana>();
            if (throwScript != null)
            {
                // 設置香蕉為手上的物件
                
                //throwScript.PickUpBanana(gameObject);
                isPickedUp = true;

                Debug.Log("Banana picked up by player!");
            }
        }
    }

}
