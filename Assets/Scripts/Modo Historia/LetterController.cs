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
    public LetterController clone;
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
    private Vector2 initLetterPos;
    private Vector2 initTouchPos;

    Coroutine waitingForDragMode = null;

    // Define the event
    public static event Action<string> OnLetterMouseUp;
    public static event Action<string> OnLetterDrag;
    public static event Action<GameObject> OnLetterDragGetReference;

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

        initLetterPos = transform.position;

        state = LetterState.IDLE;
    }

    private void Update()
    {
        if(state == LetterState.DRAG) 
        { 
            Vector2 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = initLetterPos - (initTouchPos - dragPos);
        }
    }
    // Evento de click del mouse
    void OnMouseDown()
    {
        //viewLetter.animation.Stop();
        //viewLetter.animation.Play("LetterAnimOnMouseDown");

        waitingForDragMode = StartCoroutine(DragMode());
        initTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (amount > 1) viewLetter.SetAmount(--amount);
    }

    private void OnMouseUp() 
    {
        //viewLetter.animation.Stop();
        //viewLetter.animation.Play("LetterAnimOnMouseUp");

        if(waitingForDragMode != null) StopCoroutine(waitingForDragMode);
        if (state == LetterState.DRAG) 
        {
            if (amount > 1) {
                clone.ReturnLetter(); //Devuelve la letra
                Destroy(this.gameObject); //Y destruyete, ya tienes una copia tuya en el tablero
            }
            transform.position = initLetterPos;
            state = LetterState.IDLE;
        }
        else 
        {
            Debug.Log(amount);
            if (amount == 1) gameObject.SetActive(false);
            LetterAccepted();
        }

        
    }

    // Regresa una letra
    public void ReturnLetter()
    {
        if(amount > 1) 
        {
            amount = amount + 1;
            viewLetter.SetAmount(amount);
        }
        else gameObject.SetActive(true);
    }

    public void LetterAccepted() 
    {
        InvokeOnLetterMouseUp(letter.ToString());
    }
    protected virtual void InvokeOnLetterMouseUp(string letter)
    {
        OnLetterMouseUp?.Invoke(letter);
    }
    protected virtual void InvokeOnLetterDrag(string letter)
    {
        OnLetterDrag?.Invoke(letter);
    }
    protected virtual void InvokeOnLetterDragConstructor(GameObject _this)
    {
        OnLetterDragGetReference?.Invoke(_this);
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
        state = LetterState.DRAG;
        InvokeOnLetterDragConstructor(this.gameObject);

        if (amount > 1) InvokeOnLetterDrag(letter.ToString());
    }
}


