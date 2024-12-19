using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player1controller : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 5.0f;
    //[SerializeField] private float knockbackForce = 1.0f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Animator animator;

    public GameObject bananaPrefab;
    public float throwForce = 20f;
    public float throwDelay = 0.5f;
    public float throwAngle = 45f;
    public float dizzyDuration = 3.0f;
    private bool isDizzy = false;
    
    private void Start()
    { 
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    } 
    
    public void OnMove(InputAction.CallbackContext context)
    { 
        moveInput = context.ReadValue<Vector2>();
    } 
    
    public void OnThrow(InputAction.CallbackContext context)
    { 
        if (context.started && !isDizzy) 
        { 
            animator.SetBool("isThrowing", true);
            StartCoroutine(ThrowWithDelay()); // 延遲投擲香蕉
        } 
    }

    private IEnumerator ThrowWithDelay()
    {
        yield return new WaitForSeconds(throwDelay); // 等待指定時間
        ThrowBanana();
    }
    
    private void FixedUpdate() 
    { 
        if (isDizzy) return;
        
        // 獲取移動輸入 
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // 如果有移動輸入，更新角色朝向
        if (move != Vector3.zero)
        { 
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
        
        // 移動角色 
        rb.velocity = move * playerSpeed;

        // 更新 Animator 的 isRunning 參數
        animator.SetBool("isRunning", move.magnitude > 0);

        // 如果投擲動畫播放結束，重置 IsThrowing 參數
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("throw") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) 
        { 
            animator.SetBool("isThrowing", false);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
            //Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;
            //knockbackDirection.y = 0;
            //rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse); 
        //}
    //}

    private void ThrowBanana()
    { 
        // 稍遠一點的出生位置，讓香蕉似乎是從前方拋出
        Vector3 spawnPosition = transform.position + transform.forward * 0.5f + Vector3.up * 0.25f + transform.right * 0.25f;
        Quaternion spawnRotation = transform.rotation;

        GameObject bananaInstance = Instantiate(bananaPrefab, spawnPosition, spawnRotation);
        Rigidbody rb = bananaInstance.GetComponent<Rigidbody>();
        
        if (rb != null)
        { 
            rb.isKinematic = false;
            rb.useGravity = true; // 確保開啟重力
            
            float angle = throwAngle * Mathf.Deg2Rad; // 將角度轉換為弧度
            Vector3 throwDirection = (transform.forward * Mathf.Cos(angle)) + (transform.up * Mathf.Sin(angle));
            rb.velocity = throwDirection * throwForce;

            float spinSpeed = 20f; // 可調整，越大旋轉越快
            rb.angularVelocity = bananaInstance.transform.up * spinSpeed;
        }
    }

    private bool IsDizzy()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("dizzy");
    }
    
    public void Stun()
    {
        if (IsDizzy()) return;
        isDizzy = true;
        animator.SetTrigger("isDizzy"); // 播放暈眩動畫
        StartCoroutine(DizzyCoroutine());
    }

    IEnumerator DizzyCoroutine()
    { 
        yield return new WaitForSeconds(dizzyDuration);
        
        isDizzy = false;
    }
}
