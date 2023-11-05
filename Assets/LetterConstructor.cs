using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LetterConstructor : MonoBehaviour
{
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
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseUp()
    {
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
