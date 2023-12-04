using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class LightLetters : MonoBehaviour
{
    public float duracionIluminacion = 0.5f; // Duración en segundos de la iluminación
    public GameObject[] lettersAIluminar; // Array de GameObjects a iluminar en secuencia
    public GameObject[] objetosParaIluminar; // Array de GameObjects a iluminar en secuencia
    public GameObject objetoAMover; // Array de GameObjects a iluminar en secuencia

    private SpriteRenderer[] spriteRenderers;
    private LetterController[] letterControllers;
    private Transform[] transformLetters;



    private Coroutine secuenciaCoroutine, sequenciaReq;

    void Start()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);


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
        
        transformLetters = new Transform[lettersAIluminar.Length];
        for (int i = 0; i < lettersAIluminar.Length; i++)
        {
            transformLetters[i] = lettersAIluminar[i].GetComponent<Transform>();
        }

        //Set la pos del objeto que se mueve a la primera letra introducida
        objetoAMover.transform.position = transformLetters[0].position;

        // Iniciar la secuencia de iluminación
        IniciarSecuenciaIluminacion();

        IniciarSecuenciaRecorrido();
    }   

    void IniciarSecuenciaIluminacion()
    {
        secuenciaCoroutine = StartCoroutine(IluminarSecuencia());
    }
    void IniciarSecuenciaRecorrido()
    {
        RecorridoSequencia();
    }


    void DetenerSecuenciaIluminacion()
    {
        if (secuenciaCoroutine != null)
        {
            StopCoroutine(secuenciaCoroutine);
        }
    }
    
    void DetenerSecuenciaRecorrido()
    {
        if (sequenciaReq != null)
        {
            StopCoroutine(sequenciaReq);

            //sequenciaReq = StartCoroutine(RecorridoSeq());
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
                yield return new WaitForSeconds(duracionIluminacion);

            }
        }
        // Reiniciar la secuencia 
        IniciarSecuenciaIluminacion();
    }


    void RecorridoSequencia()
    {
        DOTween.Clear();
        objetoAMover.transform.position = letterControllers[0].transform.position;

        for (int i = 0; i < lettersAIluminar.Length; i++)
        {
            if (letterControllers[i].letterState == LetterState.NORMAL)
            {
                objetoAMover.transform.DOMove(transformLetters[i].position, duracionIluminacion*2f).SetEase(Ease.InOutSine).OnKill(RecorridoSequencia);
            }
        }

        //objetoAMover.SetActive(false);
    }

    //IEnumerator RecorridoSeq()
    //{
    //    objetoAMover.transform.position = letterControllers[0].transform.position;

    //    for (int i = 0; i < lettersAIluminar.Length; i++)
    //    {
    //        if (letterControllers[i].letterState == LetterState.NORMAL)
    //        {
    //            objetoAMover.transform.DOMove(transformLetters[i].position, duracionIluminacion).SetEase(Ease.InOutSine).OnComplete(DetenerSecuenciaRecorrido);
    //        }
    //    }

    //    yield return new WaitForSeconds(4f);
        
    //}
}
