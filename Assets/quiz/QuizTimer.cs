using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuizTimer : MonoBehaviour
{
    public float totalTime = 10f; // 每題10秒倒數
    private float currentTime;
    public TextMeshProUGUI timerText; 
    private bool isTimerRunning = false;

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isTimerRunning = false;
                TimeUp();
            }

            UpdateTimerDisplay();
        }
    }

    public void StartTimer()
    {
        currentTime = totalTime;
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    void UpdateTimerDisplay()
    {
       int totalSeconds = Mathf.FloorToInt(currentTime);

        // 計算分和秒
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        // 使用string.Format格式化成 "mm:ss"
        // {0:00} 代表第一個參數以兩位數顯示，不足補0
        // {1:00} 代表第二個參數同理
        string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerText.text = timeText;
    }

    void TimeUp()
    {
        Debug.Log("Time is up!");
        // 時間到自動判定結果
        FindObjectOfType<QuizManager>().TimeOutAction();
    }
}