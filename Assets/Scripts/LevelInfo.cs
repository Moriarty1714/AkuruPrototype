using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

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
    public int gameTimeSesionInSec = 0;
    public Language gameLanguage;

    public LevelInfo() //RandomLevelInfo
    {
        //Letter Amount
        for (int i = 0; i < letterInfo.Count; i++)
        {
            KeyValuePair<char, LetterInfo> letter = letterInfo.ElementAt(i);
            if (letter.Key == 'D' || letter.Key == 'H' || letter.Key == 'L' || letter.Key == 'N' || letter.Key == 'R' || letter.Key == 'S' || letter.Key == 'T')
            {
                letterInfo[letter.Key].amount = 3;
            }
            else if (letter.Key == 'C' || letter.Key == 'F' || letter.Key == 'G' || letter.Key == 'M' || letter.Key == 'W' || letter.Key == 'Y')
            {
                letterInfo[letter.Key].amount = 3;
            }
            else if (letter.Key == 'B' || letter.Key == 'J' || letter.Key == 'K' || letter.Key == 'P' || letter.Key == 'V' || letter.Key == 'Q' || letter.Key == 'X' || letter.Key == 'Z')
            {
                letterInfo[letter.Key].amount = 3;
            }
            else if (letter.Key == 'A' || letter.Key == 'E' || letter.Key == 'I' || letter.Key == 'O' || letter.Key == 'U')
            {
                letterInfo[letter.Key].amount = 3;
            }
        }

        int randomExtraPuntuationLetters = 4;
        int extraPuntuation = 10;

        List<char> keys = new List<char>(letterInfo.Keys);

        int randKey;
        for (int i = 0; i < randomExtraPuntuationLetters; i++)
        {
            randKey = UnityEngine.Random.Range(0, keys.Count - 1);
            char randomKey = keys[randKey];
            letterInfo[randomKey].extraPuntuation = extraPuntuation;
        }

        //Game Time Sesion In Seconds
        ///Inspector

        //Game Language
        ///Inspector

    }
    public LevelInfo(int _level)
    {
        for (int i = 0; i < letterInfo.Count; i++)
        {
            char actualLetterIndex = letterInfo.ElementAt(i).Key;
            LetterInfo letterInfoTmp = new LetterInfo(
                int.Parse(HandleTSV.Instance.levelInfo[_level][actualLetterIndex][0]), //Primer elemento
                int.Parse(HandleTSV.Instance.levelInfo[_level][actualLetterIndex][1]),  //Segundo elemento
                int.Parse(HandleTSV.Instance.levelInfo[_level][actualLetterIndex][2])); //Tercer elemento
        }
    }
}

