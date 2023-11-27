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
    public UnityEvent onMouseDown;

    void OnMouseDown()
    {
        Debug.Log("Click");
        onMouseDown.Invoke();
    }
}
