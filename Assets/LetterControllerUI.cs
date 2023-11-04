using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterControllerUI : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    private struct LetterView 
    {
        public TextMeshPro letter;
    }
    [SerializeField] LetterView viewLetter;

    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLetter(string _letter) 
    { 
        viewLetter.letter.text = _letter;
    }


}
