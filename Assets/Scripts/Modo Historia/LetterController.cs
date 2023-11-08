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
    [Header("Drag mode:")]
    [SerializeField] private float inputResponseInSeconds = 0.1f;
    [SerializeField] GameObject letterConstructorPrefab;
    [SerializeField] GameObject letterRef;


    Coroutine waitingForDragMode = null;


    // Define the event
    public static event Action<string,int> OnLetterMouseUp;

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
        if (amount < 1) gameObject.SetActive(false);

        GetComponentsInChildren<TextMeshPro>()[0].text =letter.ToString();

        viewLetter.SetPuntuation(basePuntuation, extraPuntuation);
        viewLetter.SetAmount(amount);
       
    }

    private void Update()
    {
       
    }
    // Evento de click del mouse
    void OnMouseDown()
    {
        //viewLetter.animation.Stop();
        //viewLetter.animation.Play("LetterAnimOnMouseDown");

        viewLetter.SetAmount(--amount);
        waitingForDragMode = StartCoroutine(DragMode());
    }

    private void OnMouseUp() 
    {
        //viewLetter.animation.Stop();
        //viewLetter.animation.Play("LetterAnimOnMouseUp");
       
        if (amount < 1) gameObject.SetActive(false);        
        if (waitingForDragMode != null) StopCoroutine(waitingForDragMode);

        if (letterRef != null) //Si te has copiado
        {
            if (letterRef.GetComponent<LetterConstructor>().addLetterAviable)
            {
                LetterAccepted(letterRef.GetComponent<LetterConstructor>().index);
            }
            else
            {
                ReturnLetter();
            }
            Destroy(letterRef);         
        }
        else
        {
            LetterAccepted();
        }

    }

    // Regresa una letra
    public void ReturnLetter()
    {
        gameObject.SetActive(true);

        amount++;
        viewLetter.SetAmount(amount);
    }

    public void LetterAccepted(int _index = -1) 
    {
        InvokeOnLetterMouseUp(letter.ToString(),_index);
    }
    protected static void InvokeOnLetterMouseUp(string letter, int index = -1)
    {
        OnLetterMouseUp?.Invoke(letter,index);
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

    IEnumerator DragMode()
    {
        yield return new WaitForSeconds(inputResponseInSeconds);
        Debug.Log("DRAG MODE ACTIVE");

        CopyLetter();
    }

    public void CopyLetter()
    {
        letterRef = Instantiate(letterConstructorPrefab); //esta copia se queda
        letterRef.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5f));
        letterRef.GetComponent<LetterConstructor>().state = LetterConstructor.LetterState.DRAG;
        letterRef.GetComponent<LetterConstructor>().viewLetter.SetLetter(letter.ToString());
        letterRef.tag = "DragLetter";
        letterRef.name = this.gameObject.name;
    }
}


