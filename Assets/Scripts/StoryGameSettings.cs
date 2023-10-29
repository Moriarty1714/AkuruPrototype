using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryGameSettings : MonoBehaviour
{
    public static StoryGameSettings instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public LevelInfo levelInfo;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
