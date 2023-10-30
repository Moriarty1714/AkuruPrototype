using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustTableToScreen : MonoBehaviour
{
    void Start()
    {
        // Obtén el tamaño de la pantalla
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        float screenHeightRef = 1920f;
        float screenWidthRef = 1080f;

        float refRelation = screenWidthRef / screenHeightRef;
        float screenRelation = screenWidth / screenHeight;

        if(screenRelation < refRelation)
        {
            // La pantalla es más alta que la referencia
            // Ajusta el ancho
            // Calcula el factor de escala
            // (ancho de la pantalla de referencia) / (ancho de la pantalla actual)
            float x = screenRelation/refRelation;
            Debug.Log(x);
            // Ajusta la escala del objeto padre
            transform.localScale = transform.localScale * x;
        }

       
    }
}
