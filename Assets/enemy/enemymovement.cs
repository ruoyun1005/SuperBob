using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //player
    public Transform player;

    //zombie 
    public float moveSpeed = 3f;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.0f; // 攻擊冷卻時間
    
    //zombie move
    public float rotationSpeed = 5f; // 旋轉速度
    public float stopDistance = 1.5f; // 停止距離
    
    //zombie parameter
    
    private bool isWalking = false;

    //setting
    private Animator animator; // 動畫控制器
    private Rigidbody rb;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("Player object not found in the scene. Make sure the player has the 'Player' tag.");
            }
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is missing!");
            return;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= chaseRange)
        {
            if (distanceToPlayer > attackRange)
            {
                //follow player
                MoveTowardsPlayer();
                Debug.Log($"FIND PLAYER!!!");
                //isWalking = true;
                //animator.SetBool("isWalking", true);
                
            }
            else
            {
                //attacking player
                AttackPlayer();
                Debug.Log($"ATTACKING PLAYER");
                isWalking = false;
                //animator.SetBool("isWalking", false);
                animator.SetTrigger("Attacking");
            }
        }
        else
        {
            //ilde
            //isWalking = false;
            animator.SetBool("isWalking", false);
            
        }

    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        
    }
    void AttackPlayer()
    {

    }

}
