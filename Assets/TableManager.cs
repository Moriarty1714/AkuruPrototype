using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class TableManager : MonoBehaviour
{
    private enum GameState { PLAYING, ENDED}
    private GameState gameState = GameState.PLAYING;

    [System.Serializable]
    public class UIElements
    {
        public TextMeshProUGUI puntuationTMP;
        public TextMeshProUGUI accumulatePuntuationTMP;
        public TextMeshProUGUI bonusMultiplayerTMP;

        public TextMeshProUGUI timerTicking;
        public TextMeshProUGUI editingWordTMP;
        public Transform[] wordContainers;
        public RectTransform botPlacerRT;

        public AcceptButtonController acceptButton;

        public void UpdateEditingWord(string _editingWord)
        {
            editingWordTMP.text = _editingWord;
        }
        public void UpdateAccPuntAndBonMult(int _accumulatePuntuation, int _bonusMultiplyer)
        {
            accumulatePuntuationTMP.text = _accumulatePuntuation <= 0 ? string.Empty : ("+" + _accumulatePuntuation.ToString());
            bonusMultiplayerTMP.text = _bonusMultiplyer <= 1 ? string.Empty : ("x" + _bonusMultiplyer.ToString());
        }

        public void UpdatePuntuation(int _puntuation)
        {
            puntuationTMP.text = _puntuation.ToString();
        }

        public void UpdateTimerTicking(int _seconds)
        {
            int minutes = _seconds / 60;
            int seconds = _seconds % 60;

            timerTicking.text = String.Format("{0:D2}:{1:D2}", minutes, seconds);
        }

        public void UpdateAcceptButton(bool _haveResponse)
        {
            if (_haveResponse) acceptButton.ResponseRecived();
            else acceptButton.WaitingForResponse();
        }
    }
    [SerializeField] UIElements uiElements;

    [Header("Constants")]
    public const int MAX_LETTERS_IN_WORD = 20;
    public const int TIMER_IN_SECONDS = 300;

    [Header("Word Validator")]
    [SerializeField] WordValidator wordValidator;
    
    [Header("Letters GameObjects")]
    [SerializeField] private List<GameObject> lettersInGameGO = new List<GameObject>();

    private Dictionary<string, LetterController> lettersCtrl = new Dictionary<string, LetterController>();
    private List<string> selectedLetters = new List<string>();

    [SerializeField] private GameObject wordPrefab;
    private string editingWord;
    private List<string> wordsCompleted = new List<string>();
    private List<GameObject> wordObjects = new List<GameObject>();

    private int puntuation;
    private int accPuntuation;
    private int bonusMultiplyer;

    private float startSesionInSeconds;

    private void OnDisable()
    {
        wordValidator.OnWordValidationComplete -= OnWordValidationComplete;
        LetterController.OnLetterClicked -= AddLetter;
    }
    private void Start()
    {
        wordValidator.OnWordValidationComplete += OnWordValidationComplete;
        LetterController.OnLetterClicked += AddLetter;

        for (int i = 0; i < lettersInGameGO.Count; i++)
        {
            LetterController lCtrl = lettersInGameGO[i].GetComponent<LetterController>();
            lettersCtrl[lCtrl.GetLetterChar().ToString()] = lCtrl;
        }

        editingWord = string.Empty;
        startSesionInSeconds = Time.time;

        //VIEW
        uiElements.UpdateEditingWord(editingWord);
        uiElements.UpdateAccPuntAndBonMult(accPuntuation, bonusMultiplyer);
        uiElements.UpdatePuntuation(puntuation);
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.PLAYING:
                {
                    int actualTimer = TIMER_IN_SECONDS - (int)(Time.time - startSesionInSeconds);

                    if (actualTimer <= 0) gameState = GameState.ENDED;

                    //View
                    uiElements.UpdateTimerTicking(actualTimer);                   
                }
                break;
            case GameState.ENDED:
                { }
                break;
            default:
                break;
        }
       
    }
    private void OnWordValidationComplete(bool exists)
    {
        Color colorFeedback = Color.white;

        if (exists && !wordsCompleted.Contains(editingWord) && editingWord.Length > 1)
        {         
            //WordCompeted
            wordsCompleted.Add(editingWord);

            //Puntuation
            puntuation += accPuntuation * bonusMultiplyer;

            //VIEW
            colorFeedback = Color.green;
            uiElements.UpdatePuntuation(puntuation);
            GenerateAcceptedWords();
        }
        else
        {
            //ReturnEditingWord
            foreach (string letterValue in selectedLetters)
            {
                lettersCtrl[letterValue].ReturnLetter();
            }
            colorFeedback = Color.red;
        }

        editingWord = string.Empty;
        selectedLetters.Clear();

        accPuntuation = 0;
        bonusMultiplyer = 0;

        //VIEW
        StartCoroutine(FadeToAndFromColor(colorFeedback, 0.5f));
        uiElements.UpdateEditingWord(editingWord);
        uiElements.UpdateAccPuntAndBonMult(accPuntuation, bonusMultiplyer);
        uiElements.UpdateAcceptButton(true);
    }
    private void ConcatenateLetters()
    {
        editingWord = string.Empty;
        foreach (string selectedLetter in selectedLetters)
        {          
            editingWord += selectedLetter;
        }
        uiElements.UpdateEditingWord(editingWord);
    }
    private IEnumerator FadeToAndFromColor(Color color, float duration)
    {
        Color originalColor = uiElements.botPlacerRT.GetComponent<Image>().color;
        yield return UIHelper.Fade(uiElements.botPlacerRT.GetComponent<Image>(), originalColor, color, duration);
        yield return UIHelper.Fade(uiElements.botPlacerRT.GetComponent<Image>(), color, originalColor, duration);
        uiElements.botPlacerRT.GetComponent<Image>().color = Color.white;
    }
    public void GenerateAcceptedWords()
    {
        foreach (GameObject wordObj in wordObjects)
        {
            Destroy(wordObj);
        }
        wordObjects.Clear();

        float accumulatedWidht = 0;
        int rowIndex = 0;

        try
        {
            foreach (string word in wordsCompleted)
            {
                GameObject wordObj = Instantiate(wordPrefab, uiElements.wordContainers[rowIndex]);
                wordObj.GetComponentInChildren<TextMeshProUGUI>().text = word + ", ";
                wordObj.GetComponent<Button>().onClick.AddListener(() => RemoveAcceptedWord(word));

                //Comprobamos que cepa en la misma fila
                float wordObjWidth = wordObj.GetComponentInChildren<TextMeshProUGUI>().preferredWidth;
                float containerWidth = uiElements.wordContainers[rowIndex].GetComponent<RectTransform>().rect.width;
                float wordSpacing = uiElements.wordContainers[rowIndex].GetComponent<HorizontalLayoutGroup>().spacing;

                if (accumulatedWidht + wordObjWidth > containerWidth)
                {
                    wordObj.transform.SetParent(uiElements.wordContainers[++rowIndex]);
                    accumulatedWidht = 0;
                }

                accumulatedWidht += wordObjWidth + wordSpacing;

                wordObjects.Add(wordObj);
            }
        }
        catch (Exception)
        {
            Debug.Log("Max completed words limit reach");
            throw;
        }
           

    }

    private void RemoveAcceptedWord(string word)
    {
        int index = wordsCompleted.IndexOf(word);

       if (index != -1 && wordObjects[index].GetComponent<WordCompletedController>().preparedToRemove)
        {
            foreach (char letter in word)
            {
                lettersCtrl[letter.ToString()].ReturnLetter();
                accPuntuation -= lettersCtrl[letter.ToString()].GetLetterPuntuation();
            }

            wordsCompleted.RemoveAt(index);
            Destroy(wordObjects[index]);
            wordObjects.RemoveAt(index);

            GenerateAcceptedWords();
            
            //VIEW
            uiElements.UpdatePuntuation(puntuation);
       }
    }

    public void TrashButtonRemoveWord() 
    {
        foreach (GameObject wordObj in wordObjects)
        {
            wordObj.GetComponent<WordCompletedController>().WantToRemove();
        }
    }

    public void AddLetter(string letterValue)
    {
        Debug.Log(letterValue);

        if (selectedLetters.Count < MAX_LETTERS_IN_WORD)
        {
            Debug.Log("Added " + letterValue + " IN " + (selectedLetters.Count).ToString());

            //Word
            selectedLetters.Add(letterValue);
            ConcatenateLetters();

            //Puntuation
            accPuntuation += lettersCtrl[letterValue].GetLetterPuntuation();
            bonusMultiplyer++;
            
            //VIEW
            uiElements.UpdateAccPuntAndBonMult(accPuntuation, bonusMultiplyer);
        }
        else
        {
            lettersCtrl[letterValue].ReturnLetter();
        }
    }
    public void RemoveLastLetter()
    {
        if (selectedLetters.Count > 0)
        {
            string letterValue = selectedLetters[selectedLetters.Count - 1];

            Debug.Log("Removed " + letterValue + " IN " + (selectedLetters.Count - 1).ToString());

            //Word
            lettersCtrl[letterValue].ReturnLetter();
            selectedLetters.RemoveAt(selectedLetters.Count - 1);
            ConcatenateLetters();

            //Puntuation
            accPuntuation -= lettersCtrl[letterValue].GetLetterPuntuation();
            bonusMultiplyer--;

            //VIEW
            uiElements.UpdateAccPuntAndBonMult(accPuntuation, bonusMultiplyer);
        }
        
    }
    public void ValidatorButton()
    {
        wordValidator.ValidateWord(editingWord);

        //VIEW
        uiElements.UpdateAcceptButton(false);
    }
}
