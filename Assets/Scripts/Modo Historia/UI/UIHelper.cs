using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public static class UIHelper
{
    public static void DestroyChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }


    public static void PlaceObjects(Dictionary<int, GameObject> objects, RectTransform panel)
    {
        if (objects.Count == 0)
            return;

        // El tama�o del panel
        float panelWidth = panel.rect.width;
        float panelHeight = panel.rect.height;

        // El tama�o original del primer objeto
        RectTransform firstObjectTransform = objects.First().Value.GetComponent<RectTransform>();
        float originalObjectWidth = firstObjectTransform.rect.width;
        float originalObjectHeight = firstObjectTransform.rect.height;

        // El espacio que se necesitar�a para poner todos los objetos en l�nea sin cambiar su tama�o
        float requiredWidth = originalObjectWidth * objects.Count;

        // Si el espacio requerido es mayor que el ancho del panel, ajustar el tama�o de todos los GameObjects
        if (requiredWidth > panelWidth)
        {
            // Calcular el nuevo tama�o de cada objeto, manteniendo su aspecto
            float widthScale = panelWidth / requiredWidth;
            float newObjectWidth = originalObjectWidth * widthScale;
            float newObjectHeight = originalObjectHeight * widthScale;

            // Si el nuevo alto es mayor que el alto del panel, ajustar el alto y mantener su aspecto
            if (newObjectHeight > panelHeight)
            {
                float heightScale = panelHeight / newObjectHeight;
                newObjectWidth *= heightScale;
                newObjectHeight *= heightScale;
            }

            // Aplicar el nuevo tama�o a todos los GameObjects
            foreach (GameObject obj in objects.Values)
            {
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(newObjectWidth, newObjectHeight);
            }

            // Actualizar el ancho del objeto para los c�lculos de la posici�n
            originalObjectWidth = newObjectWidth;
        }

        // Centrar los GameObjects
        float startPos = -panelWidth / 2 + originalObjectWidth / 2;
        int i = 0;
        foreach (GameObject obj in objects.Values)
        {
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(startPos + i * originalObjectWidth, 0);
            rectTransform.SetParent(panel, false);

            i++;
        }
    }

    public static IEnumerator Fade(Image image, Color startColor, Color endColor, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            image.color = Color.Lerp(startColor, endColor, normalizedTime);
            yield return null;
        }
        image.color = endColor;
    }
    public static IEnumerator Fade(SpriteRenderer spriteRenderer, Color startColor, Color endColor, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            spriteRenderer.color = Color.Lerp(startColor, endColor, normalizedTime);
            yield return null;
        }
        spriteRenderer.color = endColor;
    }

}


