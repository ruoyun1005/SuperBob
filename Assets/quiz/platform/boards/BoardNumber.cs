using UnityEngine;
using TMPro;

public class BoardNumber : MonoBehaviour
{
    public int number;
    public TextMeshPro textMesh;
    private Rigidbody rb;
    private Quaternion initialRotation;
    private Vector3 initialPosition;
    private bool hasFallen = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        initialRotation = transform.rotation;
        initialPosition = transform.position;
    }

    public void SetNumber(int num) 
    {
        number = num;
        if (textMesh != null)
        {
            textMesh.text = num.ToString();
        }
        else
        {
            Debug.LogWarning("textMesh is not assigned on " + gameObject.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Banana"))
        {
            QuizManager qm = FindObjectOfType<QuizManager>();
            if (qm != null)
            {
                qm.OnNumberObtained(number);
            }
            else
            {
                Debug.LogWarning("No QuizManager found in the scene.");
            }
            if (rb != null)
            {
                rb.isKinematic = false; 
                // 可視情況添加一點斜力，讓它翻倒更明顯
                rb.AddForce((transform.right + Vector3.up) * 50f, ForceMode.Impulse);
            }

            hasFallen = true;
            StartCoroutine(StandUpAfterDelay(5f));
        }
    }

    void Update()
    {
        if (textMesh != null && Camera.main != null)
        {
            textMesh.transform.LookAt(Camera.main.transform);
            textMesh.transform.Rotate(0, 0, 0); 
        }
    }
    private System.Collections.IEnumerator StandUpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 恢復剛體為靜態模式(不受物理影響)
        rb.isKinematic = true;

        // 重設位置與角度回初始狀態
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // 如果需要，重置 hasFallen 狀態，讓板子可以再次被撞倒
        hasFallen = false; 
    }
}
