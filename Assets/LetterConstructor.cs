using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LetterConstructor : MonoBehaviour
{
    public enum LetterState { IDLE, DRAG }
    public LetterState state = LetterState.IDLE;

    //Only for drag mode:
    public bool addLetterAviable = false;
    Coroutine waitingForDragMode = null;

    // Start is called before the first frame update
    [System.Serializable]
    public class LetterView
    {
        [SerializeField] private TextMeshPro letter;
        [SerializeField] private GameObject putPositionMark;
        public void SetLetter(string _letter)
        {
            letter.text = _letter;
        }

        public void SetPositionMark(bool _state)
        {
            putPositionMark.SetActive(_state);
        }

        public string GetLetter()
        {
            return letter.text.ToString();
        }
    }
    public LetterView viewLetter;

    public int index = 0;
    private float inputResponseInSeconds = 0.1f;

    // Define the event
    public static event Action<int, bool> OnLetterMouseUp;
    public static event Action<string, int> OnLetterMouseUpDraggging;
    public static event Action<string> OnReturnLetterInLimbo;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (state == LetterState.DRAG)
        {
            Vector3 dragPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x , Input.mousePosition.y, 5f));
            transform.position = new Vector2(dragPosition.x - transform.localScale.x,dragPosition.y+ transform.localScale.y );

            if (Input.GetMouseButtonUp(0))
            {
                if (addLetterAviable)
                {
                    LetterAccepted(index);
                }
                else
                {
                    InvokeOnLetterClickedInLimbo(viewLetter.GetLetter());
                }
                Destroy(this.gameObject);
            }
        }
    }


    private void OnMouseUp() //Solo pasa con los GO que estan en el consntructor
    {
        InvokeOnLetterClicked(index);
        if (waitingForDragMode != null)
        {
            StopCoroutine(waitingForDragMode);
        }

    }

    public void LetterAccepted(int _index = -1)
    {
        InvokeOnLetterMouseUp(viewLetter.GetLetter(), _index);
    }
    private void OnMouseDown()
    {
        waitingForDragMode = StartCoroutine(DragMode());
    }

    public void AddLetterAvaiable(bool _state, int _index = 0)
    {
        addLetterAviable = _state;
        index = _index;
    }

    IEnumerator DragMode()
    {
        yield return new WaitForSeconds(inputResponseInSeconds);
        Debug.Log("DRAG MODE ACTIVE");
        state = LetterState.DRAG;
        CopyLetter();
    }
    public void CopyLetter()
    {
        GameObject letterRef;
        letterRef = Instantiate(this.gameObject, transform.parent); //esta copia se queda
        letterRef.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5f));
        letterRef.GetComponent<LetterConstructor>().state = LetterConstructor.LetterState.DRAG;
        letterRef.GetComponent<LetterConstructor>().viewLetter.SetLetter(viewLetter.GetLetter());
        //letterRef.GetComponent<LetterConstructor>().AddLetterAvaiable(true);
        letterRef.GetComponent<LetterConstructor>().index = index;
        letterRef.tag = "DragLetter";
        letterRef.name = this.gameObject.name;

        InvokeOnLetterClicked(index, true);
    }

    protected static void InvokeOnLetterMouseUp(string letter, int index = -1)
    {
        OnLetterMouseUpDraggging?.Invoke(letter, index);
    }

    protected virtual void InvokeOnLetterClicked(int _index, bool _isDrag = false)
    {
        OnLetterMouseUp?.Invoke(_index, _isDrag);
    }
    protected virtual void InvokeOnLetterClickedInLimbo(string _letter)
    {
        OnReturnLetterInLimbo?.Invoke(_letter);
    }
   

}
