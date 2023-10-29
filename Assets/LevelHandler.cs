using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{

    private enum StoryLevelType { LVL1, LVL2, TESTLVL }
    [SerializeField] private StoryLevelType storyLevelType;

    public LevelInfo levelInfo;
    //Game Time Sesion In Seconds -->Inspector
    //Game Language -->Inspector

    public void LevelSelected()
    {
        StoryGameSettings.instance.levelInfo = levelInfo;

        SceneChanger.Instance.LoadScene("StoryGame");
    }

    public void Start()
    {
        switch (storyLevelType)
        {
            case StoryLevelType.LVL1:
                levelInfo.SetTableConfiguration(0);
                break;
            case StoryLevelType.LVL2:
                levelInfo.SetTableConfiguration(1);
                break;
            case StoryLevelType.TESTLVL:
                levelInfo.SetRandomLevel();
                break;
            default:
                break;
        }

    }
}




