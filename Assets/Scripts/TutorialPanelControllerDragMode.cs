using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialPanelControllerDragMode :  TutorialPanelControler
{
    Coroutine waitingForDragMode;
    bool isDragMode = false;
    public UnityEvent onMouseUp;
    public ConstructorController constructorController;
    public LetterController letterD;

    private void OnMouseDown()
    {
        onMouseDown.Invoke();
        waitingForDragMode = StartCoroutine(TutorialDragMode());

        letterD.OnMouseDown();
    }
    void OnMouseUp()
    {
        if (isDragMode)
        {
            letterD.OnMouseUp();
            if (letterD.letterState == LetterState.SHOP)
            {
                onMouseUp.Invoke();
            }
            isDragMode = false;
        }
        else 
        {
            letterD.CopyLetter();
            letterD.OnMouseUp();
            StopCoroutine(waitingForDragMode);
        }   
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if(constructorController != null)
            constructorController.OnTriggerStay2D(collision);
        
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (constructorController != null)
            constructorController.OnTriggerExit2D(collision);
    }

    IEnumerator TutorialDragMode() //vigilar pq ha de ser el mateix que en el LetterControler
    {
        yield return new WaitForSeconds(0.1f); //vigilar pq ha de ser el mateix que en el LetterControler
        isDragMode = true;
    }
}

