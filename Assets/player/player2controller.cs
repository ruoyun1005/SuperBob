using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player2controller : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float stompForce = 10.0f;
    [SerializeField] private float stunRadius = 5.0f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Animator animator;
    private bool isGrounded;
    private bool isSlipping = false;
    private bool isStomping = false;
    //private bool isOnJumpableSurface = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //isSlipping = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    { 
        //if (!isSliding)
        //{
        moveInput = context.ReadValue<Vector2>();
        //}
    }
    
    public void OnJump(InputAction.CallbackContext context)
    { 
         // 僅當角色在可跳躍的表面時才允許跳躍
        if (context.started && isGrounded && !isSlipping && !isStomping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        } 
    }

    public void OnStomp(InputAction.CallbackContext context)
    {
        if (context.started && !isGrounded && !isStomping)
        {
            TriggerStomp();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSlipping || isStomping)
        {
            return;
        }
        
        // 獲取移動輸入 
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // 如果有移動輸入，更新角色朝向
        if (move != Vector3.zero)
        { 
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
        
        // 移動角色 
        rb.velocity = new Vector3(move.x * playerSpeed, rb.velocity.y, move.z * playerSpeed);

        // 更新 Animator 的 isRunning 參數
        animator.SetBool("isRunning", move.magnitude > 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 結束踩踏（僅在正在踩踏時執行）
        if (isStomping)
        {
            EndStomp();

            // 檢測是否碰到敵人
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // 檢查敵人是否有 "EnemyHead" 物件
                Collider[] colliders = collision.gameObject.GetComponentsInChildren<Collider>();
                foreach (Collider col in colliders)
                {
                    // 如果是敵人的頭部觸發器
                    if (col.CompareTag("EnemyHead"))
                    {
                        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                        if (enemy != null)
                        {
                            enemy.Stun(); // 讓敵人進入暈眩狀態
                        }
                        return;
                    }
                }
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            //isOnJumpableSurface = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    { 
        // 如果離開地板或運算符號板，設置 isOnJumpableSurface 為 false
        //if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("OperatorPlatform") || collision.gameObject.CompareTag("EnemyHead"))
        //{
            //isOnJumpableSurface = false;
        //}

        // 如果離開地板，設置 isGrounded 為 false
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isStomping)
        {
            EndStomp();

            if (other.CompareTag("OperatorPlatform"))
            {
                //isOnJumpableSurface = true;

                // 如果是運算符號板，通知 QuizManager
                if (other.CompareTag("OperatorPlatform"))
                {
                    OperatorPlatform platform = other.GetComponent<OperatorPlatform>();
                    if (platform != null)
                    {
                        platform.NotifyQuizManager();
                    }
                    return;
                }
            }
        }

        if (other.CompareTag("Banana"))
        { 
            Banana banana = other.GetComponent<Banana>();
            if (banana != null && banana.IsOnGround())
            { 
                TriggerSlip(); // 觸發滑倒動畫
            } 
        }
    }

    private void TriggerStomp()
    {
        isStomping = true;
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.down * stompForce, ForceMode.Impulse);
        animator.SetTrigger("isStomping");
    }
    
    private void EndStomp()
    {
        // 啟動協程來延遲結束踩踏
        StartCoroutine(DelayedEndStomp());
    }

    private IEnumerator DelayedEndStomp()
    {
        yield return new WaitForSeconds(0.1f); // 延遲
    
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("stomp"))
        {
            animator.ResetTrigger("isStomping");
            animator.Play("idle");
            isStomping = false; // 確保可以繼續移動
            StunNearbyBob();
        }
    }

    
    private void StunNearbyBob()
    {
        // 在角色位置附近檢測碰撞體
        Collider[] colliders = Physics.OverlapSphere(transform.position, stunRadius);

        foreach (Collider collider in colliders)
        {
            // 如果檢測到的對象有標籤 "Bob"
            if (collider.CompareTag("Player"))
            {
                player1controller bobController = collider.GetComponent<player1controller>();
                if (bobController != null)
                {
                    bobController.Stun();
                }
            }
        }
    }
    
    public void TriggerSlip()
    { 
        isSlipping = true;
        animator.SetTrigger("isSlipping");
    }

    public void ResetSlip()
    {
        isSlipping = false; // 恢復移動
    }
}
