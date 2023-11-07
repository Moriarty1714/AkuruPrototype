using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordCompletedController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI wordCompletedTMP;

    public void SetWord(string _word)
    {
        wordCompletedTMP.text = _word;
    }
}
