using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordCompletedController : MonoBehaviour
{
        public float vibrationMagnitude = 0.1f; // Qué tanto se moverá el objeto en cada vibración
        public bool preparedToRemove = false;

        private Vector3 originalPosition;

    [SerializeField] TextMeshProUGUI wordCompletedTMP;

    public void WantToRemove()
    {
        if (!preparedToRemove)
        {
            StartCoroutine(Vibrate());
            wordCompletedTMP.color = Color.red;
            preparedToRemove = true;
        }
        else {
            wordCompletedTMP.color = Color.black;
            preparedToRemove = false;
        
        }
    }

    IEnumerator Vibrate()
    {
        preparedToRemove = true;
        originalPosition = transform.position;

        while (preparedToRemove)
        {
            float x = originalPosition.x + Random.Range(-vibrationMagnitude, vibrationMagnitude);
            float y = originalPosition.y + Random.Range(-vibrationMagnitude, vibrationMagnitude);

            transform.position = new Vector3(x, y, originalPosition.z);

            yield return null; // Esperar hasta el próximo frame
        }
    }
    
}
