using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bananaaa : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        EnemyReaction enemy  = collision.gameObject.GetComponent<EnemyReaction>();
        if (enemy != null)
        {
            // 如果目標是敵人，造成傷害
            enemy.TakeDamage();
        }

        // 無論是否命中敵人，子彈都會被銷毀
        //Destroy(gameObject);
    }
    
}

