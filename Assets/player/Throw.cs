using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    /*public int bananaCount = 5;
    public TextMeshProUGUI bananaText;*/
    public GameObject bananaPrefab;
    public float throwForce = 20f;
    
    private void Start()
    { 
        /*UpdateBananaUI();*/
    } 
    
    private void Update()
    { 
        // 投擲香蕉的按鍵檢測
        if (Input.GetKeyDown(KeyCode.E) /*&& bananaCount > 0*/)
        { 
            ThrowHeldBanana();
        } 
    } 
    
    private void ThrowHeldBanana()
    { 
        /*if (bananaCount <= 0) return;*/
        
        // 稍遠一點的出生位置，讓香蕉似乎是從前方拋出
        Vector3 spawnPosition = transform.position + transform.forward * 4f + Vector3.up * 4f;
        Quaternion spawnRotation = transform.rotation;

        GameObject bananaInstance = Instantiate(bananaPrefab, spawnPosition, spawnRotation);
        Rigidbody rb = bananaInstance.GetComponent<Rigidbody>();
        
        if (rb != null)
        { 
            rb.isKinematic = false;
            rb.useGravity = true; // 確保開啟重力
            
            // 移除角度轉換，直接水平向前
            float throwForce = 30f;// 加大拋出力道
            Vector3 throwDirection = transform.forward;
            rb.velocity = throwDirection * throwForce;
            float spinSpeed = 20f; // 可調整，越大旋轉越快
            rb.angularVelocity = bananaInstance.transform.up * spinSpeed;
        }
        
        /*bananaCount--;
        UpdateBananaUI();*/
    } 
    
    /*private void UpdateBananaUI()
    { 
        if (bananaText != null)
            bananaText.text = "Bananas: " + bananaCount;
    }*/
}
