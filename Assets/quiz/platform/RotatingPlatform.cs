using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public int count = 10;       // 板子數量
    public float a = 8f;         // 橢圓半長軸 (x方向)
    public float b = 4f;         // 橢圓半短軸 (z方向)
    public float rotationSpeed = 1.0f; // 板子繞橢圓的移動速度 (弧度/秒)
    public GameObject boardPrefab;     // 板子的預製物 (上面有BoardNumber腳本與TextMesh設定)

    private List<float> angles;  // 儲存每個板子對應的角度
    private List<GameObject> boards;

    void Start()
    {
        angles = new List<float>();
        boards = new List<GameObject>();

        // 等距分佈10個板子的初始角度
        for (int i = 0; i < count; i++)
        {
            float theta = 2f * Mathf.PI * i / count; 
            angles.Add(theta);

            // 根據初始角度計算初始位置
            float x = a * Mathf.Cos(theta);
            float z = b * Mathf.Sin(theta);

            // 建立板子實體
            GameObject board = Instantiate(boardPrefab, new Vector3(x, 4, z), Quaternion.identity);
            board.name = "Board_" + i;
            boards.Add(board);

            // 在這裡取得 BoardNumber 元件並設定數字
            BoardNumber bn = board.GetComponent<BoardNumber>();
            if (bn != null)
            {
                bn.SetNumber(i);
            }

            // 如有需要，設定方向朝中心
            // Vector3 dirToCenter = (Vector3.zero - board.transform.position).normalized;
            // board.transform.forward = dirToCenter; 
        }
    }

    void Update()
    {
        // 每幀更新每個板子的位置，使其沿著橢圓軌道移動
        for (int i = 0; i < count; i++)
        {
            // 增加角度
            angles[i] += rotationSpeed * Time.deltaTime;

            float x = a * Mathf.Cos(angles[i]);
            float z = b * Mathf.Sin(angles[i]);

            boards[i].transform.position = new Vector3(x, 4, z);

            // 可加入朝向中心的程式碼
            // var dirToCenter = (Vector3.zero - boards[i].transform.position).normalized;
            // boards[i].transform.forward = dirToCenter;
        }
    }
}
