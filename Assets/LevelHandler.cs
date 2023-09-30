using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{

    private enum StoryLevelType { LVL1, LVL2, TESTLVL }
    [SerializeField] private StoryLevelType storyLevelType;

    [Serializable]
    public class Level
    {
        public Dictionary<char, int> letterAmount = new Dictionary<char, int>() { { 'a', 0 }, { 'b', 0 }, { 'c', 0 }, { 'd', 0 }, { 'e', 0 }, { 'f', 0 }, { 'g', 0 }, { 'h', 0 }, { 'i', 0 }, { 'j', 0 }, { 'k', 0 }, { 'l', 0 }, { 'm', 0 }, { 'n', 0 }, { 'o', 0 }, { 'p', 0 }, { 'q', 0 }, { 'r', 0 }, { 's', 0 }, { 't', 0 }, { 'u', 0 }, { 'v', 0 }, { 'w', 0 }, { 'x', 0 }, { 'y', 0 }, { 'z', 0 } };
        public Dictionary<char, int> letterExtraPuntuation = new Dictionary<char, int>() { { 'a', 0 }, { 'b', 0 }, { 'c', 0 }, { 'd', 0 }, { 'e', 0 }, { 'f', 0 }, { 'g', 0 }, { 'h', 0 }, { 'i', 0 }, { 'j', 0 }, { 'k', 0 }, { 'l', 0 }, { 'm', 0 }, { 'n', 0 }, { 'o', 0 }, { 'p', 0 }, { 'q', 0 }, { 'r', 0 }, { 's', 0 }, { 't', 0 }, { 'u', 0 }, { 'v', 0 }, { 'w', 0 }, { 'x', 0 }, { 'y', 0 }, { 'z', 0 } };
        public int gameTimeSesionInSec = 0;
        public Language gameLanguage;
    }
    public Level levelInfo;

    public void LevelSelected()
    {
        StoryGameSettings.instance.letterAmount = levelInfo.letterAmount;
        StoryGameSettings.instance.letterExtraPuntuation = levelInfo.letterExtraPuntuation;
        StoryGameSettings.instance.gameTimeSesionInSec = levelInfo.gameTimeSesionInSec;
        StoryGameSettings.instance.gameLanguage = levelInfo.gameLanguage;
            

        SceneManager.LoadScene("StoryGame");
    }

    public void Start()
    {
        switch (storyLevelType)
        {
            case StoryLevelType.LVL1:
                break;
            case StoryLevelType.LVL2:
                break;
            case StoryLevelType.TESTLVL:
                SetTestLevel();
                break;
            default:
                break;
        }

    }

    private void SetTestLevel()
    {
        //Letter Amount
        for (int i = 0; i < levelInfo.letterAmount.Count; i++)
        {
            KeyValuePair<char,int> letter = levelInfo.letterAmount.ElementAt(i);
            if (letter.Key == 'd' || letter.Key == 'h' || letter.Key == 'l' || letter.Key == 'n' || letter.Key == 'r' || letter.Key == 's' || letter.Key == 't')
            {
                levelInfo.letterAmount[letter.Key] = 3;
            }
            else if (letter.Key == 'c' || letter.Key == 'f' || letter.Key == 'g' || letter.Key == 'm' || letter.Key == 'w' || letter.Key == 'y')
            {
                levelInfo.letterAmount[letter.Key] = 2;
            }
            else if (letter.Key == 'b' || letter.Key == 'j' || letter.Key == 'k' || letter.Key == 'p' || letter.Key == 'v' || letter.Key == 'q' || letter.Key == 'x' || letter.Key == 'z')
            {
                levelInfo.letterAmount[letter.Key] = 1;
            }
            else if (letter.Key == 'a' || letter.Key == 'e' || letter.Key == 'i' || letter.Key == 'o' || letter.Key == 'u')
            {
                levelInfo.letterAmount[letter.Key] = 5;
            }
        }



        int randomExtraPuntuationLetters = 4;
        int extraPuntuation = 10;

        List<char> keys = new List<char>(levelInfo.letterExtraPuntuation.Keys);

        int randKey;
        for (int i = 0; i < randomExtraPuntuationLetters; i++)
        {
            randKey = UnityEngine.Random.Range(0, keys.Count-1);
            char randomKey = keys[randKey];
            levelInfo.letterExtraPuntuation[randomKey] = extraPuntuation;
        }

        //Game Time Sesion In Seconds
        ///Inspector

        //Game Language
        ///Inspector

    }
}




