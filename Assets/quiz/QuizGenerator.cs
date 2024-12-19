using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizGenerator : MonoBehaviour
{
    public enum Difficulty { Simple, Hard }

    private char[] ops = new char[] { '+', '-', '*', '/' };

    public int X { get; private set; }
    public int Z { get; private set; }
    public Difficulty currentDifficulty { get; set; }

    // 儲存所有可行解法，用具名元組確保有 .op 和 .Y 欄位
    public List<(char op, int Y)> correctSolutions = new List<(char op, int Y)>();

    public void GenerateQuestion()
    {
        correctSolutions.Clear(); // 清除前一次的解答

        if (currentDifficulty == Difficulty.Simple)
        {
            GenerateSimpleQuestion();
        }
        else
        {
            GenerateHardQuestion();
        }

        Debug.Log(X + " ? ? = " + Z + (currentDifficulty == Difficulty.Simple ? " (Simple)" : " (Hard)"));
    }

    private void GenerateSimpleQuestion()
    {
        bool valid = false;
        int tries = 0;

        while(!valid && tries < 100)
        {
            int tempX = Random.Range(0, 11);
            int tempZ = Random.Range(0, 11);
            char op = ops[Random.Range(0, ops.Length)];

            int Y = -1;
            bool canSolve = false;
            switch(op)
            {
                case '+': 
                    Y = tempZ - tempX; 
                    canSolve = (Y >= 0 && Y <= 9); 
                    break;
                case '-':
                    Y = tempX - tempZ; 
                    canSolve = (Y >= 0 && Y <= 9); 
                    break;
                case '*':
                    if (tempX != 0 && tempZ % tempX == 0) {
                        Y = tempZ / tempX;
                        canSolve = (Y >= 0 && Y <= 9);
                    }
                    break;
                case '/':
                    if (tempZ != 0 && tempX % tempZ == 0) {
                        Y = tempX / tempZ;
                        canSolve = (Y >= 0 && Y <= 9);
                    }
                    break;
            }

            if (canSolve)
            {
                // 找出所有解法
                var allSolutions = FindAllSolutions(tempX, tempZ);
                // 確保該題只有一種解法，且正是這組(op,Y)
                if (allSolutions.Count == 1 && allSolutions[0].op == op && allSolutions[0].Y == Y)
                {
                    X = tempX;
                    Z = tempZ;
                    correctSolutions.Clear();
                    correctSolutions.Add((op: op, Y: Y));
                    valid = true;
                }
            }

            tries++;
        }

        if (!valid)
        {
            Debug.LogWarning("Failed to generate a simple unique solution question within 100 tries.");
        }
    }

    private void GenerateHardQuestion()
    {
        bool valid = false;
        int tries = 0;
        while (!valid && tries < 100)
        {
            int tempX = Random.Range(0, 11);
            int tempZ = Random.Range(0, 11);

            // 找出所有解法
            var tempSolutions = FindAllSolutions(tempX, tempZ);

            // Hard 題要至少2解
            if (tempSolutions.Count >= 2)
            {
                X = tempX;
                Z = tempZ;
                correctSolutions = tempSolutions;
                valid = true;
            }

            tries++;
        }

        if (!valid)
        {
            Debug.LogWarning("Failed to generate a hard multiple solution question within 100 tries.");
        }
    }

    // 尋找所有解法 (op,Y)
    private List<(char op, int Y)> FindAllSolutions(int givenX, int givenZ)
    {
        List<(char op, int Y)> solutions = new List<(char op, int Y)>();

        foreach (char op in ops)
        {
            for (int Y = 0; Y <= 9; Y++)
            {
                if (CheckSolution(givenX, op, Y, givenZ))
                {
                    // 指定名稱(op: op, Y: Y) 以確保命名欄位可用
                    solutions.Add((op: op, Y: Y));
                }
            }
        }

        return solutions;
    }

    private bool CheckSolution(int X, char op, int Y, int Z)
    {
        int computed = 0;
        switch(op)
        {
            case '+': computed = X + Y; break;
            case '-': computed = X - Y; break;
            case '*': computed = X * Y; break;
            case '/':
                if (Y == 0) return false;
                if (X % Y != 0) return false; 
                computed = X / Y; 
                break;
        }
        return (computed == Z);
    }
}
