using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaStation : MonoBehaviour
{   
    public int supplyAmount = 5; // 每次補給的香蕉數量
    public string playerTag = "Player"; // 玩家標籤
    public GameObject supplyEffect; // 可選：補給效果

    void OnTriggerEnter(Collider other)
    {
        // 檢測玩家是否進入補給站
        if (other.CompareTag(playerTag))
        {
            ThrowBanana inventory = other.GetComponent<ThrowBanana>();
            if (inventory != null)
            {
                inventory.AddBananas(supplyAmount); // 增加香蕉數量

                // 顯示補給效果（可選）
                if (supplyEffect != null)
                {
                    Instantiate(supplyEffect, transform.position, Quaternion.identity);
                }

                Debug.Log("Player supplied with " + supplyAmount + " bananas!");
            }
        }
    }
}
