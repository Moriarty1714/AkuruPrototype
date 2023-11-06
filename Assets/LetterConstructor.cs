using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LetterConstructor : MonoBehaviour
{
    public enum LetterState { IDLE, DRAG }
    public LetterState state;

    // Start is called before the first frame update
    [System.Serializable]
    private struct LetterView 
    {
        public TextMeshPro letter;
    }
    [SerializeField] LetterView viewLetter;

    public int index = 0;
    // Define the event
    public static event Action<int> OnLetterClicked;

    void Start()
    {
        //state = LetterState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == LetterState.DRAG)
        {
            transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5f));
        }
    }

    private void OnMouseUp()
    {        
        if (state == LetterState.DRAG)
        { 
            Destroy(gameObject); 
        }
        InvokeOnLetterClicked(index);

    }
    protected virtual void InvokeOnLetterClicked(int _index)
    {
        OnLetterClicked?.Invoke(_index);
    }

    public void SetLetter(string _letter) 
    { 
        viewLetter.letter.text = _letter;
    }
}
