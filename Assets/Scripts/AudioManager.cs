using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sounds[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    [Range(0.1f, 0.5f)]
    public float volumeChangeMultiplayer = 0.2f;
    [Range(0.1f, 0.5f)]
    public float pichChangeMultiplayer = 0.2f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("MainTheme");   
    }

    public void PlayMusic(string name)
    {
        Sounds s = Array.Find(musicSounds, x => x.name == name);

        if(s == null)
        {
            Debug.LogError("Music not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    
    public void PlaySFX(string name)
    {
        Sounds s = Array.Find(sfxSounds, x => x.name == name);

        if(s == null)
        {
            Debug.LogError("Sound not Found");
        }
        else
        {
            sfxSource.volume = UnityEngine.Random.Range(volumeChangeMultiplayer, 0.5f);                         //Set volume variation
            sfxSource.pitch = UnityEngine.Random.Range(pichChangeMultiplayer, 1f);                              //Set pich variation
            sfxSource.PlayOneShot(s.clip);
        }
    }
}
