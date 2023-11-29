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
    public UnityEvent onDragMode, onGameManager;



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
        else
        {
            onMouseUp.Invoke();            
        }

        //if (gameManager.selectedLetters.Count == 8) // Check Board -.-
        //{
        //    Debug.Log("N letters:" + gameManager.selectedLetters.Count);
        //    onGameManager.Invoke();
        //}
    }

    IEnumerator DragMode()
    {
        yield return new WaitForSeconds(0.1f); //vigilar pq ha de ser el mateix que en el LetterControler

        onDragMode.Invoke();
        onMouseUp.Invoke();
    }
}
