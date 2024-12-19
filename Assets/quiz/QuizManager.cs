using UnityEngine;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public QuizGenerator quizGenerator;
    public TextMeshPro textMeshProObject;
    public GameObject resultPanel;
    public GameObject donePanel;
    public TextMeshProUGUI resultText;
    public QuizTimer quizTimer;
    public Animator npcAnimator;

    public enum GameState { Idle, AwaitingAnswer, ShowingResult }
    private GameState state;

    private int questionIndex = 0;
    private int totalQuestions = 10;
    private bool awaitingAnswer = false;

    private char selectedOperator;
    private int chosenNumber = -1; // 只需要一個數字
    // 不需要 secondNumber
    char asciiPlus = (char)0x2B;

    void Start()
    {
        
        quizGenerator.currentDifficulty = (questionIndex < 5) ? QuizGenerator.Difficulty.Simple : QuizGenerator.Difficulty.Hard;
        NextQuestion();
    }

    void NextQuestion()
    {
        if (questionIndex >= totalQuestions)
        {
            Debug.Log("All questions done.");
            state = GameState.Idle;
            donePanel.SetActive(true);
            return;
        }

        quizGenerator.currentDifficulty = (questionIndex < 5) ? QuizGenerator.Difficulty.Simple : QuizGenerator.Difficulty.Hard;
        quizGenerator.GenerateQuestion();

        // 顯示題目 X ? ? = Z
        // 第一個 ? 會被SetOperator替換成運算子
        // 第二個 ? 會被OnNumberObtained替換成玩家選的數字
        textMeshProObject.text = quizGenerator.X + " ? ? = " + quizGenerator.Z;

        awaitingAnswer = true;
        chosenNumber = -1;
        selectedOperator = '\0';

        questionIndex++;
        state = GameState.AwaitingAnswer;
        
        quizTimer.StartTimer();
    }

    public void SetOperator(char op)
    {
        if (!awaitingAnswer) return;

        char displayOp = op;
        if (op == '*') displayOp = '×'; // 或 '\u00D7'
        if (op == '/') displayOp = '÷'; // 或 '\u00F7'

        selectedOperator = op; // 內部邏輯仍用原始符號判斷
        Debug.Log("Operator selected: " + displayOp);

        string currentText = textMeshProObject.text;
        int questionMarkIndex = currentText.IndexOf('?');
        if (questionMarkIndex != -1)
        {
            textMeshProObject.text = currentText.Substring(0, questionMarkIndex)
                                    + displayOp
                                    + currentText.Substring(questionMarkIndex + 1);
        }
    }


    public void OnNumberObtained(int num)
    {
        if (!awaitingAnswer) return;
        chosenNumber = num;
        Debug.Log("Number chosen: " + num);

        // 將第二個?替換成該數字
        string currentText = textMeshProObject.text;
        int questionMarkIndex = currentText.IndexOf('?');
        if (questionMarkIndex != -1)
        {
            textMeshProObject.text = currentText.Substring(0, questionMarkIndex)
                                     + num.ToString()
                                     + currentText.Substring(questionMarkIndex + 1);
        }
        Debug.Log("Final text: " + textMeshProObject.text);


        CheckAnswer();
    }

    void CheckAnswer()
    {
        quizTimer.StopTimer();
        int computed = int.MinValue;

        // 現在只計算 X op chosenNumber = Z
        switch (selectedOperator)
        {
            case '+': computed = quizGenerator.X + chosenNumber; break;
            case '-': computed = quizGenerator.X - chosenNumber; break;
            case '*': computed = quizGenerator.X * chosenNumber; break;
            case '/':
                if (chosenNumber != 0 && quizGenerator.X % chosenNumber == 0)
                    computed = quizGenerator.X / chosenNumber;
                break;
        }

        if (computed == quizGenerator.Z)
        {
            Debug.Log("Correct Answer!");
            ShowResultUI("Correct!", true);
            if (npcAnimator != null)
            {
                npcAnimator.SetTrigger("Clap");
            }
        }
        else
        {
            Debug.Log("Wrong Answer!");
            ShowResultUI("Wrong!", false);
        }

        awaitingAnswer = false;
        state = GameState.ShowingResult;
    }

    void ShowResultUI(string message, bool isCorrect)
    {
        resultText.text = message;
        resultPanel.SetActive(true);

        resultText.color = isCorrect ? Color.green : Color.red;

        Invoke(nameof(HideResultAndNextQuestion), 2f);
    }

    void HideResultAndNextQuestion()
    {
        resultPanel.SetActive(false);
        NextQuestion();
    }
    public void TimeOutAction()
    {
        // 時間到自動判定為錯誤並顯示結果
        if (awaitingAnswer)
        {
            CheckAnswerAsWrong();
        }
    }

    void CheckAnswerAsWrong()
    {
        quizTimer.StopTimer();
        Debug.Log("Time Up - Wrong Answer!");
        ShowResultUI("Time's up! Wrong!", false);
        awaitingAnswer = false;
        state = GameState.ShowingResult;
    }
}
