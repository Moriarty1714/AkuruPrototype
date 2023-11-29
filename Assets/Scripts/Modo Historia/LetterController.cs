using System;
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
        public TextMeshPro puntuationTMP;
        public TextMeshPro amountTMP;
        public Animation animation;
        public SpriteRenderer spr;
        public TextMeshPro letterTMP;

        public GameObject puntuation;
        public GameObject number;
        public GameObject coinsObj;

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
                    //puntuationTMP.gameObject.GetComponentInParent<SpriteRenderer>().color = Color.yellow;
                }
            }
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

            puntuation.SetActive(active);
            number.SetActive(active);
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
    [SerializeField] GameObject letterRef;

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
        if (amount < 1) {
            letterState = LetterState.SHOP;
            //Decidir si poner que se pueda comprar si empiezas la partida con la letra desactivada.
            viewLetter.ChangeViewToStateShop(false);
        }
        else
        {
            letterState = LetterState.NORMAL;
        }

        GetComponentsInChildren<TextMeshPro>()[0].text =letter.ToString();

        viewLetter.SetPuntuation(basePuntuation, extraPuntuation);
        viewLetter.SetAmount(amount);
       
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
        else if (letterState == LetterState.SHOP && OnMouseOverButton() && Profile.Instance.UpdateProfileCoints(-10)) 
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
    protected virtual void InvokeOnBuyLetter(bool _isPoosible)
    {
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


