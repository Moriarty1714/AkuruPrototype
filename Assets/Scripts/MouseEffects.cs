using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEffects : MonoBehaviour
{
    [SerializeField]
    private GameObject particle;

    private Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(particle, mousePos, Quaternion.identity);            
        }
    }
}
