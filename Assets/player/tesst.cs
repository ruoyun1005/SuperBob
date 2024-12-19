using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using TMPro;

public class Test : MonoBehaviour
{
    public int bananaCount = 5; // 玩家擁有的香蕉數量
    public GameObject bananaPrefab; // 香蕉的 Prefab
    public float throwForce = 10f; // 丟香蕉的力量
    public TextMeshProUGUI bananaText; // UI 元件

    void Start()
    {
        UpdateBananaUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && bananaCount > 0)
        {
            ThrowHeldBanana();
        }
    }

    private void ThrowHeldBanana()
    {
        if (bananaCount <= 0)
        {
            Debug.Log("Cannot throw banana: No bananas available.");
            return;
        }

        Debug.Log("Throwing banana...");

        // 在玩家正前方生成香蕉
        Vector3 spawnPosition = transform.position + transform.forward * 1.0f + Vector3.up * 4.5f; // 玩家正前方偏上
        Quaternion spawnRotation = transform.rotation; // 與玩家方向一致

        // 實例化香蕉
        GameObject bananaInstance = Instantiate(bananaPrefab, spawnPosition, spawnRotation);

        // 給香蕉添加物理推力
        Rigidbody rb = bananaInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse); // 沿著玩家的前方施加推力
        }

        bananaCount--;
        UpdateBananaUI();
        Debug.Log("Banana thrown! Remaining bananas: " + bananaCount);
    }

    private void UpdateBananaUI()
    {
        if (bananaText != null)
        {
            bananaText.text = "Bananas: " + bananaCount;
        }
        else
        {
            Debug.LogError("BananaText UI is not assigned!");
        }
    }
}

