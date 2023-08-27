using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcceptButtonController : MonoBehaviour
{
    [SerializeField] private Image acceptButtonImg;
    [SerializeField] private Sprite waitingSprite;
    [SerializeField] private Sprite okSprite;
    [SerializeField] private float fadeDuration = 0.5f; // Duración del efecto en segundos

    public void WaitingForResponse()
    {
        StartCoroutine(FadeImageInOut(waitingSprite));
    }

    public void ResponseRecived()
    {
        StartCoroutine(FadeImageInOut(okSprite));
    }

    private IEnumerator FadeImageInOut(Sprite _sprite)
    {
        // Fade Out
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            // Ajusta la alfa de la imagen
            acceptButtonImg.color = new Color(acceptButtonImg.color.r, acceptButtonImg.color.g, acceptButtonImg.color.b, Mathf.Lerp(1f, 0f, t / fadeDuration));
            yield return null;
        }

        // Asegura que la imagen esté completamente invisible
        acceptButtonImg.color = new Color(acceptButtonImg.color.r, acceptButtonImg.color.g, acceptButtonImg.color.b, 0f);

        // Cambia el sprite de la imagen
        acceptButtonImg.sprite = _sprite;

        // Fade In
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            // Ajusta la alfa de la imagen
            acceptButtonImg.color = new Color(acceptButtonImg.color.r, acceptButtonImg.color.g, acceptButtonImg.color.b, Mathf.Lerp(0f, 1f, t / fadeDuration));
            yield return null;
        }

        // Asegura que la imagen esté completamente visible
        acceptButtonImg.color = new Color(acceptButtonImg.color.r, acceptButtonImg.color.g, acceptButtonImg.color.b, 1f);    
    }
}
