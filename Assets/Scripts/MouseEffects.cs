using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEffects : MonoBehaviour
{
    [SerializeField] private GameObject particle;

    private Vector2 mousePos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);                         //Set the mouse pos
            Instantiate(particle, mousePos, Quaternion.identity);                                   //Instanciate particles


            AudioManager.Instance.PlaySFX("TouchScreen");
        }
    }
}
