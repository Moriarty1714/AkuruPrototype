using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class LightLetters : MonoBehaviour
{
    public float duracionIluminacion = 0.5f; // Duración en segundos de la iluminación
    public float duracionTrail = 0.5f; // Duración en segundos de la iluminación
    public GameObject[] lettersAIluminar; // Array de GameObjects a iluminar en secuencia
    public GameObject[] objetosParaIluminar; // Array de GameObjects a iluminar en secuencia
    public GameObject objetoAMover; // GameObject a mover en la secuencia

    public GameObject initialPos; // GameObject a mover en la secuencia



    private SpriteRenderer[] spriteRenderers;
    private LetterController[] letterControllers;
    private Transform[] transformLetters;

    private Coroutine secuenciaCoroutine;

    void Start()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);

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

        if(initialPos != null)
        {
            objetoAMover.transform.position = initialPos.transform.position;
        }
        else
        {
            objetoAMover.transform.position = transformLetters[0].position;
        }

        IniciarSecuenciaIluminacion();
        IniciarSecuenciaRecorrido();
    }

    void IniciarSecuenciaIluminacion()
    {
        secuenciaCoroutine = StartCoroutine(IluminarSecuencia());
    }

    void IniciarSecuenciaRecorrido()
    {
        StartCoroutine(RecorridoSecuenciaCoroutine());
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
        for (int i = 0; i < objetosParaIluminar.Length; i++)
        {
            if (letterControllers[i].letterState == LetterState.NORMAL)
            {
                spriteRenderers[i].color = new Color(1f, 1f, 0f, 1f);
                yield return new WaitForSeconds(duracionIluminacion);
                spriteRenderers[i].color = new Color(1f, 1f, 1f, 0f);
                yield return new WaitForSeconds(duracionIluminacion);
            }
        }

        IniciarSecuenciaIluminacion();
    }

    IEnumerator RecorridoSecuenciaCoroutine()
    {
        DOTween.Clear();
        if (initialPos != null)
        {
            objetoAMover.transform.position = initialPos.transform.position;
        }
        else
        {
            objetoAMover.transform.position = letterControllers[0].transform.position;
        }

        

        Vector3[] lettersPositions = new Vector3[lettersAIluminar.Length];
        for (int i = 0; i < lettersAIluminar.Length; i++)
        {
            lettersPositions[i] = transformLetters[i].position;
        }
        yield return objetoAMover.transform.DOPath(lettersPositions, duracionTrail* lettersAIluminar.Length, PathType.CatmullRom).SetEase(Ease.InOutFlash).WaitForCompletion();

        StartCoroutine(RecorridoSecuenciaCoroutine());
    }
}