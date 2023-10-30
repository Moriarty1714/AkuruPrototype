using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
public enum LetterType { CONSONANT, VOWEL };
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
        public void SetAmount(int _amount)
        {
            amountTMP.text = "x" + _amount;
        }
    }
    [SerializeField] private ViewLetter viewLetter;

    [Header("Configuation:")]
    [SerializeField] private LetterType letterType;
 
    [SerializeField] private int basePuntuation;   
    [SerializeField] private char letter;
    [SerializeField] int amount = 1;
    [SerializeField] private int extraPuntuation = 0;

    // Define the event
    public static event Action<string> OnLetterClicked;

    private void Awake()
    {
        if (!(StoryGameSettings.instance == null))
        {
            amount = StoryGameSettings.instance.levelInfo.letterInfo[letter].amount;
            basePuntuation = StoryGameSettings.instance.levelInfo.letterInfo[letter].basePuntuation;
            extraPuntuation = StoryGameSettings.instance.levelInfo.letterInfo[letter].extraPuntuation;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponentsInChildren<TextMeshPro>()[0].text =letter.ToString();

        viewLetter.SetPuntuation(basePuntuation, extraPuntuation);
        viewLetter.SetAmount(amount);
    }

    // Evento de click del mouse
    void OnMouseDown()
    {    
        //VIEW
        transform.localScale = transform.localScale * 0.7f;
    }

    private void OnMouseUp()
    {
        if (amount > 1)
        {
            viewLetter.SetAmount( --amount);
        }
        else
        {
            gameObject.SetActive(false);
        }
        
        InvokeOnLetterClicked(letter.ToString());

        //VIEW
        transform.localScale = transform.localScale / 0.7f;
    }

    // Regresa una letra
    public void ReturnLetter()
    {
        if (!gameObject.activeInHierarchy)
        {               
            gameObject.SetActive(true);
        }
        else 
        {
            amount = amount + 1;
        }
        viewLetter.SetAmount(amount);
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
