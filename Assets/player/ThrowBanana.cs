using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThrowBanana : MonoBehaviour
{
    public int bananaCount = 0; // 玩家擁有的香蕉數量
    public GameObject bananaPrefab; // 香蕉的 Prefab
    public Transform handTransform; // 丟出香蕉的起始位置
    public float throwForce = 10f; // 丟香蕉的力量
    public TextMeshProUGUI bananaText; // UI 元件

    void Start()
    {
        if (handTransform == null)
        {
            Debug.LogError("Hand Transform is not assigned!");
        }
        UpdateBananaUI();
    }

    public void AddBananas(int amount)
    {
        bananaCount += amount;
        Debug.Log("Bananas added. Current count: " + bananaCount);
        UpdateBananaUI();

        if (handTransform.childCount == 0 && bananaPrefab != null)
        {
            // 實例化香蕉
            GameObject bananaInstance = Instantiate(bananaPrefab);
            bananaInstance.transform.SetParent(handTransform); // 設置為子物件
            bananaInstance.transform.position = handTransform.position; // 對齊世界位置
            bananaInstance.transform.rotation = handTransform.rotation; // 對齊世界旋轉
            bananaInstance.transform.localPosition = Vector3.zero; // 重置相對位置
            bananaInstance.transform.localRotation = Quaternion.identity; // 重置相對旋轉
            Debug.Log("Banana added to hand at position: " + bananaInstance.transform.position);
        }
        else
        {
            Debug.Log("Hand is not empty or bananaPrefab is null.");
        }
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
        if (handTransform.childCount == 0)
        {
            Debug.Log("Cannot throw banana: Hand is empty.");
            return;
        }

        Debug.Log("Throwing banana...");
        Transform bananaInHand = handTransform.GetChild(0);
        bananaInHand.SetParent(null);

        Rigidbody rb = bananaInHand.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(handTransform.forward * throwForce, ForceMode.Impulse);
        }

        bananaCount--;
        UpdateBananaUI();
        Debug.Log("Banana thrown! Remaining bananas: " + bananaCount);

        if (bananaCount > 0 && bananaPrefab != null)
        {
            GameObject newBanana = Instantiate(bananaPrefab, handTransform.position, handTransform.rotation);
            newBanana.transform.SetParent(handTransform);
            newBanana.transform.localPosition = Vector3.zero;
            newBanana.transform.localRotation = Quaternion.identity;
            Debug.Log("New banana added to hand at: " + newBanana.transform.position);
        }
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
