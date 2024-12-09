using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyswaper : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemyPrefab; 
    public int Count = 10;
    public Vector3 spawnAreaCenter; // 生成區域的中心
    public Vector3 spawnAreaSize; // 生成區域的大小
    void Start()
    {
        SpawnEnemies();
    }

    // Update is called once per frame
    void SpawnEnemies()
    {
        for (int i = 0; i < Count; i++ )
        {
            Vector3 spawnPosition = GetRandomPosition();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
    Vector3 GetRandomPosition()
    {
        // 計算隨機位置
        float x = Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2);
        float y = spawnAreaCenter.y; // 假設所有殭屍生成在同一高度
        float z = Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2);

        return new Vector3(x, y, z);
    }
    void OnDrawGizmosSelected()
    {
        // 在編輯器中可視化生成區域
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
    }

}
