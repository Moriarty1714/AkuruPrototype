using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonResize : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private LayoutElement layoutElement;

    public float selectedWidth = 150;
    private float originalWidth;

    void Start()
    {
        layoutElement = GetComponent<LayoutElement>();
        originalWidth = layoutElement.preferredWidth;
    }

    public void OnSelect(BaseEventData eventData)
    {
        layoutElement.preferredWidth = selectedWidth;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        layoutElement.preferredWidth = originalWidth;
    }
}
