using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReaction : MonoBehaviour
{
    private Animator animator;
    private bool isHitted = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void TakeDamage()
    {
        if (animator != null)
        {
            animator.SetBool("isHitted", true);
        }
        else
        {
            Debug.LogError("Animator component not found on enemy!");
        }
    }
}
