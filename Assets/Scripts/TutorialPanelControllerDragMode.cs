using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialPanelControllerDragMode :  TutorialPanelControler
{
    public UnityEvent onMouseUp;
    public ConstructorController constructorController;
    public LetterController letterTutorial;

    private void OnMouseDown()
    {
        onMouseDown.Invoke();
        if (letterTutorial != null)
        {
            letterTutorial.OnMouseDown();
        }
    }
    void OnMouseUp()
    {
        if (letterTutorial != null)
        {
            if (letterTutorial.letterRef != null) //Si tiene copia
            {
                letterTutorial.OnMouseUp();
                if (letterTutorial.letterState == LetterState.SHOP)
                {
                    onMouseUp.Invoke();
                }
            }
            else
            {
                letterTutorial.CopyLetter();
                letterTutorial.OnMouseUp();
            }
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

