using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
public enum LetterType { CONSONANT, VOWEL };
public class LetterController : MonoBehaviour
{    
    private enum LetterState {IDLE, DRAG }
    private LetterState state;
    [System.Serializable]
    private class ViewLetter
    {
        public TextMeshPro puntuationTMP;
        public TextMeshPro amountTMP;
        public Animation animation;

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

    //Visual
    private Vector2 initLetterPos;
    private Vector2 initTouchPos;

    public bool draggingLetter = false;

    // Define the event
    public static event Action<string> OnLetterMouseUp;
    public static event Action<string> OnLetterMouseDown;

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

        initLetterPos = transform.position;

        state = LetterState.IDLE;
    }

    private void Update()
    {
        switch (state)
        {
            case LetterState.IDLE:
                {
                    if (draggingLetter)
                    {
                        initTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        state = LetterState.DRAG;
                    }
                }
                break;
            case LetterState.DRAG:
                {
                    Vector2 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    transform.position = initLetterPos - (initTouchPos - dragPos);
                    
                    if (Input.GetMouseButtonUp(0)) //OnMouseUp
                    {
                        Debug.Log("GFFS");
                        ////VIEW
                        //viewLetter.animation.Stop();
                        //viewLetter.animation.Play("LetterAnimOnMouseUp");

                        draggingLetter = false;

                        InvokeOnLetterMouseUp(letter.ToString());

                        Destroy(this.gameObject);

                        transform.position = initLetterPos; 
                        state = LetterState.IDLE;
                    }
                }
                break;
            default:
                break;
        }
    }
    // Evento de click del mouse
    void OnMouseDown()
    {
        if (amount > 1)
        {
            viewLetter.SetAmount(--amount);
        }
        else
        {
            gameObject.SetActive(false);
        }

        //VIEW
        //viewLetter.animation.Stop();
        //viewLetter.animation.Play("LetterAnimOnMouseDown");

        draggingLetter = true;

        InvokeOnLetterMouseDown(letter.ToString());
    }

    private void OnMouseUp() //No funciona si es la copia con la que interecuamos
    {
      
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

    protected virtual void InvokeOnLetterMouseUp(string letter)
    {
        OnLetterMouseUp?.Invoke(letter);
    }
    protected virtual void InvokeOnLetterMouseDown(string letter)
    {
        OnLetterMouseDown?.Invoke(letter);
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


