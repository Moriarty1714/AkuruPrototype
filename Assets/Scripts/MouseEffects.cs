using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEffects : MonoBehaviour
{
    [SerializeField] private GameObject particle;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip clickSound;
    [Range(0.1f, 0.5f)]
    public float volumeChangeMultiplayer = 0.2f;
    [Range(0.1f, 0.5f)]
    public float pichChangeMultiplayer = 0.2f;


    private Vector2 mousePos;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);                         //Set the mouse pos
            Instantiate(particle, mousePos, Quaternion.identity);                                   //Instanciate particles


            AudioManager.Instance.PlaySFX("TouchScreen");

            audioSource.Play();                                                                     //Play audio
        }
    }
}
