using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    private Rigidbody rb;
    private bool isOnGround = false;
    private Collider bananaCollider;
    
    private void Start()
    { 
        rb = GetComponent<Rigidbody>();
        bananaCollider = GetComponent<Collider>();
    }
    
    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if(enemy.IsDizzy())
                {
                    enemy.Die(); // 讓敵人進入暈眩狀態
                }
            }
        }
        
        // 如果碰到地面，停止運動
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // 禁用物理運動
            isOnGround = true;
            bananaCollider.isTrigger = true;
        }
    }

    public bool IsOnGround() 
    { 
        return isOnGround;
    }
}
