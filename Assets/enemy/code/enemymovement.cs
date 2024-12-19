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
    //public float attackRange = 2f;
    //public float attackCooldown = 1.0f; // 攻擊冷卻時間
    
    //zombie move
    public float rotationSpeed = 5f; // 旋轉速度
    public float stopDistance = 1.5f; // 停止距離
    public float dizzyDuration = 3.0f; // 暈眩持續時間
    public float deadDuration = 10.0f;
    
    //zombie parameter
    //private bool isWalking = false;
    private bool isDizzy = false;
    private bool isDead = false;

    //setting
    private Animator animator; // 動畫控制器
    private Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("Player object not found in the scene. Make sure the player has the 'Player' tag.");
            }
        
        
    }
    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is missing!");
            return;
        }

        if (isDizzy || isDead)
        { 
            // 如果敵人處於暈眩狀態，不做任何移動
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= chaseRange)
        {
            //if (distanceToPlayer > attackRange)
            //{

            //follow player
            MoveTowardsPlayer();
            //Debug.Log($"FIND PLAYER!!!");
            //isWalking = true;
            animator.SetBool("isWalking", true);
                
            //}
            //else
            //{
                // 還沒有加攻擊
                //attacking player
                /*AttackPlayer();
                Debug.Log($"ATTACKING PLAYER");
                isWalking = false;
                animator.SetBool("isWalking", false);
                animator.SetTrigger("Attacking");*/
                
            //}
        }
        else
        {
            //ilde
            //isWalking = false;
            animator.SetBool("isWalking", false);
            
        }

         // 如果敵人正在播放死亡動畫，並且動畫已經播放完畢
        if (isDead && animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // 確保死亡動畫播放完畢後銷毀物件
            Destroy(gameObject);
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

    public bool IsDizzy()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("dizzy");
    }
    
    public void Stun()
    {
        if (IsDizzy()) return;
        isDizzy = true;
        animator.SetBool("isWalking", false);
        animator.SetTrigger("isDizzy"); // 播放暈眩動畫
        StartCoroutine(DizzyCoroutine());
    } 
    
    IEnumerator DizzyCoroutine()
    { 
        yield return new WaitForSeconds(dizzyDuration);
        
        isDizzy = false;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= chaseRange)
        {
            //isWalking = true;
            animator.SetBool("isWalking", true);
        }
        else 
        { 
            //isWalking = false;
            animator.SetBool("isWalking", false);
        } 
    }

    public void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    { 
        yield return new WaitForSeconds(deadDuration);
        
        Destroy(gameObject);
    }
}
