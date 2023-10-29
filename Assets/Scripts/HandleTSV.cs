using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleTSV
{
    private static HandleTSV _instance;
    public static HandleTSV Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new HandleTSV();
            }
            return _instance;
        }
    }
    public List<Dictionary<char, string[]>> levelInfo = new List<Dictionary<char, string[]>>();

    private HandleTSV()
    {
        int level = 1;

        while (Resources.Load<TextAsset>("Level"+level.ToString()) != null) { 
            string []infoLetter = Resources.Load<TextAsset>("Level" + level.ToString()).text.Split('\n');

            Dictionary<char, string[]> tmpLevelInfo = new Dictionary<char, string[]>();  
            for (int i = 0; i < infoLetter.Length; i++)
            {
                string[] splittedInfoLetter = infoLetter[i].Split('\t');
                tmpLevelInfo[splittedInfoLetter[0].ToCharArray()[0]] = splittedInfoLetter.Skip(1).ToArray();
            }
            levelInfo.Add(tmpLevelInfo);

            level++;
        }
    }
}