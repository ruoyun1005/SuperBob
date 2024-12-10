using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 5f;
    public float rotationSpeed = 1f;
    private Rigidbody rb;
    private bool isGround = true;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void Update()
    {
        HandleJump();
    }

    void MovePlayer()
    {
        float moveX = 0;
        float moveZ = 0;

        if (Input.GetKey("w")) moveZ = 1;
        if (Input.GetKey("s")) moveZ = -1;
        if (Input.GetKey("a")) moveX = -1;
        if (Input.GetKey("d")) moveX = 1;

        // 計算移動方向
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (moveDirection.magnitude > 0)
        {
            // 計算新位置
            Vector3 newPosition = rb.position + moveDirection * speed * Time.fixedDeltaTime;

            // 使用 Rigidbody 移動
            rb.MovePosition(newPosition);

            // 讓玩家面向移動方向
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // 更新動畫參數
            animator.SetBool("isRuning", true);
        }
        else
        {
            animator.SetBool("isRuning", false);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;

            //animator.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
}
