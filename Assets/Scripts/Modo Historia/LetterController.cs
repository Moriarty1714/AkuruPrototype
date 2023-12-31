﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public enum LetterType { CONSONANT, VOWEL };
public enum LetterState { NORMAL, SHOP };

public class LetterController : MonoBehaviour
{
    public LetterState letterState;
    [System.Serializable]
    private class ViewLetter
    {
        public TextMeshPro amountTMP;
        public Animation animation;
        public SpriteRenderer spr;
        public Sprite vocalImg, consonantImg;
        public TextMeshPro letterTMP;

        public GameObject extraPuntuationEffect;
        //public GameObject puntuation;
        public GameObject number;
        public GameObject coinsObj;

        // Setea la puntuación y maneja la visibilidad del elemento de puntuación
        public void SetColorPuntuation(LetterType _letterType, int basePuntuation, int extraPuntuation = 0)
        {
            if (_letterType == LetterType.CONSONANT)
            {
                // Definir los colores pastel
                Color pastelBlue = new Color(0.678f, 0.847f, 0.902f); // Pastel Blue
                Color pastelYellow = new Color(0.992f, 0.992f, 0.588f); // Pastel Yellow

                // Cambiar el color del SpriteRenderer
                float t = Mathf.InverseLerp(1, 10, basePuntuation+ extraPuntuation);
                spr.color = Color.Lerp(pastelBlue, pastelYellow, t);

            }

            // Activar o desactivar el GameObject "glow"
            extraPuntuationEffect.SetActive(extraPuntuation != 0);
        }

        // Setea la cantidad y maneja la representación visual de la misma
        public void SetAmount(int _amount)
        {
            amountTMP.text = "x" + _amount;
        }


        public void ChangeViewToStateShop(bool active)
        {
            if (!active)
            {
                spr.color = new Color(0f, 0f, 0f, 0.5f);
                letterTMP.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else
            {
                spr.color = Color.white;
                letterTMP.color = Color.white;
            }

            //puntuation.SetActive(active);
            number.SetActive(active);
            extraPuntuationEffect.SetActive(active);

            coinsObj.SetActive(!active);
        }
    }
    [SerializeField] private ViewLetter viewLetter;

    [Header("Configuation:")]
    [SerializeField] private LetterType letterType;
 
    [SerializeField] private int basePuntuation;
    [SerializeField] private char letter;
    [SerializeField] public int amount = 1;
    [SerializeField] private int extraPuntuation = 0;

    //Visual
    [Header("Drag mode:")]
    [SerializeField] private float inputResponseInSeconds = 0.1f;
    [SerializeField] GameObject letterConstructorPrefab;
    [SerializeField] public GameObject letterRef;

    Coroutine waitingForDragMode = null;

    // Define the event
    public static event Action<string,int> OnLetterMouseUp;
    public static event Action<bool> OnBuyLetter;

    private void Awake()
    {
        
        letter = char.Parse(name.Split("_")[1]);
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

        viewLetter.SetColorPuntuation(letterType, basePuntuation, extraPuntuation);
        viewLetter.SetAmount(amount);

        if(letter == 'A' || letter == 'E' ||letter == 'I' || letter == 'O' || letter == 'U')//Set img depending if is vocalo or not
        {
            viewLetter.spr.sprite = viewLetter.vocalImg;
            viewLetter.spr.gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            viewLetter.spr.gameObject.transform.position += new Vector3(0f, -0.07f, 0f);
        }
        else
        {
            viewLetter.spr.sprite = viewLetter.consonantImg;
        }

        if (amount < 1)
        {
            letterState = LetterState.SHOP;
            //Decidir si poner que se pueda comprar si empiezas la partida con la letra desactivada.
            viewLetter.ChangeViewToStateShop(false);
        }
        else
        {
            letterState = LetterState.NORMAL;
        }
    }

    private void Update()
    {
    }
    // Evento de click del mouse
    public void OnMouseDown()
    {
        //viewLetter.animation.Stop();
        //viewLetter.animation.Play("LetterAnimOnMouseDown");
        if (letterState == LetterState.NORMAL)
        {
            viewLetter.SetAmount(--amount);
            waitingForDragMode = StartCoroutine(DragMode());
        }
    }

    public void OnMouseUp() 
    {
        Debug.Log("Holi");
        //viewLetter.animation.Stop();
        //viewLetter.animation.Play("LetterAnimOnMouseUp");
        if (letterState == LetterState.NORMAL)
        {
            if (amount < 1)
            {
                letterState = LetterState.SHOP;
                viewLetter.ChangeViewToStateShop(false);
            }
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
        else if (letterState == LetterState.SHOP && OnMouseOverButton() && Profile.Instance.CanBuy(-10)) 
        {
            ReturnLetter();
            InvokeOnBuyLetter(true);
        }    
    }

    // Regresa una letra
    public void ReturnLetter()
    {
        letterState = LetterState.NORMAL;
        viewLetter.ChangeViewToStateShop(true);
        viewLetter.SetAmount(++amount);
    }

    public void LetterAccepted(int _index = -1) 
    {
        InvokeOnLetterMouseUp(letter.ToString(),_index);
    }
    protected static void InvokeOnLetterMouseUp(string letter, int index = -1)
    {
        OnLetterMouseUp?.Invoke(letter,index);
    }
    public virtual void InvokeOnBuyLetter(bool _isPoosible)
    {
        Profile.Instance.UpdateProfileCoints(-10);
        OnBuyLetter?.Invoke(_isPoosible);
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

    public void RemoveAmount()
    {
        amount--;
        viewLetter.SetAmount(amount);
    }

    public void SetShop()
    {
        if(amount<1)
        letterState = LetterState.SHOP; viewLetter.ChangeViewToStateShop(false);
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

    private bool OnMouseOverButton()
    {
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                // Check if the hit collider is the one you're interested in
                return hit.collider.gameObject == gameObject; // or some specific condition based on your setup
            }

            return false;
        }
    }
}


