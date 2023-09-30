using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class WordValidator : MonoBehaviour
{
    private static readonly string url = "https://api.dictionaryapi.dev/api/v2/entries/en/";

    // Evento para notificar cuando se determina si una palabra existe o no
    public event Action<bool> OnWordValidationComplete;

    [Serializable]
    public class DictionaryResponse
    {
        public string word;
        public string phonetic;
        public string origin;
        public Meanings[] meanings;
    }

    [Serializable]
    public class Meanings
    {
        public string partOfSpeech;
        public Definitions[] definitions;
    }

    [Serializable]
    public class Definitions
    {
        public string definition;
        public string example;
    }

    public void ValidateWord(string txt)
    {
        StartCoroutine(CheckWordExists(txt.ToLower(), OnWordValidationComplete));
    }

    private IEnumerator CheckWordExists(string word, Action<bool> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url + word))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                if (www.responseCode == 404)
                {
                    Debug.Log("Palabra no encontrada en el diccionario.");
                    callback?.Invoke(false);
                }
                else
                {
                    Debug.LogError("Error while sending request: " + www.error);
                    callback?.Invoke(false);
                }
            }
            else
            {
                string fixedJson = FixJson(www.downloadHandler.text);
                DictionaryResponse[] wordInfo = JsonHelper.FromJson<DictionaryResponse>(fixedJson);

                if (wordInfo.Length > 0 && wordInfo[0].word == word)
                {
                    Debug.Log("Palabra válida: " + wordInfo[0].word);
                    callback?.Invoke(true);
                }
                else
                {
                    Debug.Log("Palabra no encontrada en el diccionario.");
                    callback?.Invoke(false);
                }
            }
        }
    }


    private string FixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }
}
