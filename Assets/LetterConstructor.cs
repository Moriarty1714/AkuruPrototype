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
    }
    public LetterView viewLetter;

    public int index = 0;
    // Define the event
    public static event Action<int> OnLetterClicked;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (state == LetterState.DRAG)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5f));
        }
    }

    private void OnMouseUp() //Solo pasa con los GO que estan en el consntructor
    {
        InvokeOnLetterClicked(index);

    }

    private void OnMouseDown()
    {
        
    }

    protected virtual void InvokeOnLetterClicked(int _index)
    {
        OnLetterClicked?.Invoke(_index);
    }

    public void AddLetterAvaiable(bool _state, int _index = 0)
    {
        addLetterAviable = _state;
        index = _index;
    }
}
