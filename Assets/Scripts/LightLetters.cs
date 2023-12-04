using System.Collections;
using UnityEngine;

public class LightLetters : MonoBehaviour
{
    public float duracionIluminacion = 0.5f; // Duración en segundos de la iluminación
    public GameObject[] lettersAIluminar; // Array de GameObjects a iluminar en secuencia
    public GameObject[] objetosParaIluminar; // Array de GameObjects a iluminar en secuencia

    private SpriteRenderer[] spriteRenderers;
    private LetterController[] letterControllers;
    private Coroutine secuenciaCoroutine;

    void Start()
    {
        // Obtener los SpriteRenderers de los GameObjects
        spriteRenderers = new SpriteRenderer[objetosParaIluminar.Length];


        for (int i = 0; i < objetosParaIluminar.Length; i++)
        {
            spriteRenderers[i] = objetosParaIluminar[i].GetComponent<SpriteRenderer>();
        }

        letterControllers = new LetterController[lettersAIluminar.Length];
        for (int i = 0; i < lettersAIluminar.Length; i++)
        {
            letterControllers[i] = lettersAIluminar[i].GetComponent<LetterController>();
        }

        // Iniciar la secuencia de iluminación
        IniciarSecuenciaIluminacion();
    }

    void IniciarSecuenciaIluminacion()
    {
        secuenciaCoroutine = StartCoroutine(IluminarSecuencia());
    }

    void DetenerSecuenciaIluminacion()
    {
        if (secuenciaCoroutine != null)
        {
            StopCoroutine(secuenciaCoroutine);
        }
    }

    IEnumerator IluminarSecuencia()
    {

        for (int i = 0; i < lettersAIluminar.Length; i++)
        {
            if(letterControllers[i].letterState == LetterState.NORMAL)
            {
                // Iluminar el objeto
                spriteRenderers[i].color = new Color(1f, 1f, 0f, 1f);

                // Esperar la duración de la iluminación
                yield return new WaitForSeconds(duracionIluminacion);

                // Apagar el objeto
                spriteRenderers[i].color = new Color(1f, 1f, 1f, 0f); // Puedes ajustar el color según tus necesidades

                // Esperar un breve tiempo antes de pasar al siguiente objeto
                yield return new WaitForSeconds(0.1f);

            }
        }

        // Reiniciar la secuencia 
        IniciarSecuenciaIluminacion();
    }
}
