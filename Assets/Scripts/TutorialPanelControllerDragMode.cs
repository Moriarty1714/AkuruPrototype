using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialPanelControllerDragMode :  TutorialPanelControler
{
    public UnityEvent onMouseUp;
    public ConstructorController constructorController;
    public LetterController letterD;

    private void OnMouseDown()
    {
        onMouseDown.Invoke();

        letterD.OnMouseDown();
    }
    void OnMouseUp()
    {
        if (letterD.letterRef != null) //Si tiene copia
        {
            letterD.OnMouseUp();
            if (letterD.letterState == LetterState.SHOP)
            {
                onMouseUp.Invoke();
            }
        }
        else
        {
            letterD.CopyLetter();
            letterD.OnMouseUp();
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
}

