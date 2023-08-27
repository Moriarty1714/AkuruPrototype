using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
public enum LetterType { CONSONANT, VOWEL };
public enum AmountLetterType { FINITE, INFINITE };
public class LetterController : MonoBehaviour
{
    [System.Serializable]
    private struct ViewLetter
    {
        public TextMeshPro puntuationTMP;
        public TextMeshPro amountTMP;

        // Setea la puntuación y maneja la visibilidad del elemento de puntuación
        public void SetPuntuation(int basePuntuation, int extraPuntuation = 0)
        {
            bool isBasePuntuationZero = basePuntuation == 0;
            puntuationTMP.gameObject.SetActive(!isBasePuntuationZero);

            if (!isBasePuntuationZero)
            {
                puntuationTMP.text = "+" + (extraPuntuation + basePuntuation).ToString();

                if (extraPuntuation != 0)
                {
                    //Feedback for extrapower
                    puntuationTMP.gameObject.GetComponentInParent<SpriteRenderer>().color = Color.yellow;
                }
            }
        }

        // Setea la cantidad y maneja la representación visual de la misma
        public void SetAmount(AmountLetterType _amountLetterType, int _amount)
        {
            amountTMP.text = _amountLetterType == AmountLetterType.INFINITE ? "∞" : "x" + _amount;
        }
    }
    [SerializeField] private ViewLetter viewLetter;

    [Header("Configuation:")]
    [SerializeField] private LetterType letterType;
    [SerializeField] private AmountLetterType amountLetterType;
    [SerializeField] private int amount = 0;
    [SerializeField] private int basePuntuation;
    [SerializeField] private int extraPuntuation = 0;
    [SerializeField] private char letter;
    // Define the event
    public static event Action<string> OnLetterClicked;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentsInChildren<TextMeshPro>()[0].text =letter.ToString();

        viewLetter.SetPuntuation(basePuntuation, extraPuntuation);
        viewLetter.SetAmount(amountLetterType, amount);
    }

    // Evento de click del mouse
    void OnMouseDown()
    {    
        //VIEW
        transform.localScale = transform.localScale * 0.7f;
    }

    private void OnMouseUp()
    {
        if (amountLetterType == AmountLetterType.FINITE)
        {
            if (amount > 1)
            {
                viewLetter.SetAmount(amountLetterType, --amount);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        InvokeOnLetterClicked(letter.ToString());

        //VIEW
        transform.localScale = transform.localScale / 0.7f;
    }

    // Regresa una letra
    public void ReturnLetter()
    {
        if (amountLetterType == AmountLetterType.FINITE)
        {
            if (!gameObject.activeInHierarchy)
            {               
                gameObject.SetActive(true);
            }
            else 
            {
                amount = amount + 1;
            }
            viewLetter.SetAmount(amountLetterType, amount);
        }
    }

    protected virtual void InvokeOnLetterClicked(string letter)
    {
        OnLetterClicked?.Invoke(letter);
    }

    //Getters
    public char GetLetterChar()
    {
        return letter;
    }
    public int GetLetterPuntuation()
    {
        return basePuntuation+extraPuntuation;
    }
}
