using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManagerForPrototype : MonoBehaviour
{
    // Start is called before the first frame update
    public void NextLevelNormal()
    {
        StoryGameSettings.instance.NextLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
