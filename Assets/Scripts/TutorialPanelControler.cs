using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class TutorialPanelControler : MonoBehaviour
{

    Coroutine waitingForDragMode = null;
    public UnityEvent onMouseDown, onMouseUp;
    public UnityEvent onDragMode, onTrigger, onConstructor;

    public GameManager gameManager;

    bool dragMode = false, inTrigger = false;


    private void Start()
    {        
       
    }
    void OnMouseDown()
    {
        onMouseDown.Invoke();

        waitingForDragMode = StartCoroutine(DragMode());
    }

    public void OnMouseUp()
    {
        if (waitingForDragMode != null) StopCoroutine(waitingForDragMode);

        if (dragMode)
        {
            if (gameManager != null)
            {               
                if (inTrigger) // Check Board -.-
                {
                    Debug.Log("NN letters:" + gameManager.selectedLetters.Count);
                    onConstructor.Invoke();
                }
            }
        }
    }

    IEnumerator DragMode()
    {
        yield return new WaitForSeconds(0.1f); //vigilar pq ha de ser el mateix que en el LetterControler
        dragMode = true;
        onDragMode.Invoke();
        onMouseUp.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DragLetter")
        {
            onTrigger.Invoke();
            inTrigger = true;
        }
    }
}
