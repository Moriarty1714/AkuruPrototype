using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelInfo
{
    public class LetterInfo
    {
        public int amount;
        public int basePuntuation;
        public int extraPuntuation;

        public LetterInfo() { }
        public LetterInfo(int _amount, int _basePuntuation, int _extraPuntuation)
        {
            amount = _amount;
            basePuntuation = _basePuntuation;
            extraPuntuation = _extraPuntuation;
        }
    }

    public Dictionary<char, LetterInfo> letterInfo = new Dictionary<char, LetterInfo>() { { 'A', new LetterInfo() }, { 'B', new LetterInfo() }, { 'C', new LetterInfo() }, { 'D', new LetterInfo() }, { 'E', new LetterInfo() }, { 'F', new LetterInfo() }, { 'G', new LetterInfo() }, { 'H', new LetterInfo() }, { 'I', new LetterInfo() }, { 'J', new LetterInfo() }, { 'K', new LetterInfo() }, { 'L', new LetterInfo() }, { 'M', new LetterInfo() }, { 'N', new LetterInfo() }, { 'O', new LetterInfo() }, { 'P', new LetterInfo() }, { 'Q', new LetterInfo() }, { 'R', new LetterInfo() }, { 'S', new LetterInfo() }, { 'T', new LetterInfo() }, { 'U', new LetterInfo() }, { 'V', new LetterInfo() }, { 'W', new LetterInfo() }, { 'X', new LetterInfo() }, { 'Y', new LetterInfo() }, { 'Z', new LetterInfo() } };
    public int gameTimeSesionInSec = 300;
    public Language gameLanguage = Language.CATALAN;

    public void SetRandomLevel() //RandomLevelInfo
    {
        //Letter Amount
        for (int i = 0; i < letterInfo.Count; i++)
        {
            char  letterIndex = letterInfo.ElementAt(i).Key;
            letterInfo[letterIndex].basePuntuation = 1;
            if (letterIndex == 'D' || letterIndex == 'H' || letterIndex == 'L' || letterIndex == 'N' || letterIndex == 'R' || letterIndex == 'S' || letterIndex == 'T')
            {
                letterInfo[letterIndex].amount = 3;
            }
            else if (letterIndex == 'C' || letterIndex == 'F' || letterIndex == 'G' || letterIndex == 'M' || letterIndex == 'W' || letterIndex == 'Y')
            {
                letterInfo[letterIndex].amount = 2;
            }
            else if (letterIndex == 'B' || letterIndex == 'J' || letterIndex == 'K' || letterIndex == 'P' || letterIndex == 'V' || letterIndex == 'Q' || letterIndex == 'X' || letterIndex == 'Z')
            {
                letterInfo[letterIndex].amount = 1;
            }
            else if (letterIndex == 'A' || letterIndex == 'E' || letterIndex == 'I' || letterIndex == 'O' || letterIndex == 'U')
            {
                letterInfo[letterIndex].amount = 5;
            }
        }

        int randomExtraPuntuationLetters = 4;
        int extraPuntuation = 10;

        List<char> keys = new List<char>(letterInfo.Keys);

        int randKey;
        for (int i = 0; i < randomExtraPuntuationLetters; i++)
        {
            randKey = (DateTime.Now.Second+(i*200)) % (keys.Count - 1); //Random UnityEngine doesn't work (UnityExeption) NO FUNCA!
            char randomKey = keys[randKey];
            letterInfo[randomKey].extraPuntuation = extraPuntuation;
        }

        //Game Time Sesion In Seconds
        ///Inspector

        //Game Language
        ///Inspector

    }
    public void SetTableConfiguration(int _level)
    {
        if (_level <= 9)
        {
            for (int i = 0; i < letterInfo.Count; i++)
            {
                char actualLetterIndex = letterInfo.ElementAt(i).Key;
                int primerElemento = 0;
                int segundoElemento = 0;
                int tercerElemento = 0;

                int.TryParse(HandleTSV.Instance.levelInfo[_level][actualLetterIndex][0], out primerElemento);
                int.TryParse(HandleTSV.Instance.levelInfo[_level][actualLetterIndex][1], out segundoElemento);
                int.TryParse(HandleTSV.Instance.levelInfo[_level][actualLetterIndex][2], out tercerElemento);

                LetterInfo letterInfoTmp = new LetterInfo(primerElemento, segundoElemento, tercerElemento);
                letterInfo[actualLetterIndex] = letterInfoTmp;
            }
        }
        else //Tmp prototype
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    
}

