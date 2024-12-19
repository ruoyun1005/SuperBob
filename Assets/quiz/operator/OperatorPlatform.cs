using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorPlatform : MonoBehaviour
{
    public char operatorSymbol; // '+', '-', '*', '/'

    public void NotifyQuizManager()
    {
        QuizManager qm = FindObjectOfType<QuizManager>();
        if (qm != null)
        {
            qm.SetOperator(operatorSymbol);
            Debug.Log("Player stepped on operator platform: " + operatorSymbol);
        }
    }
}
