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
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    private enum GameState { PLAYING, ENDED}
    private GameState gameState = GameState.PLAYING;

    private const int MAX_LETTERS_IN_WORD = 15;
    
    private int gameTimeSesionInSec = 300;
    private Language gameLanguage;

    [System.Serializable]
    public class UIElements
    {
        public TextMeshProUGUI puntuationTMP;
        public TextMeshProUGUI accumulatePuntuationTMP;
        public TextMeshProUGUI accumulateBonusMultiplayerTMP;
        public TextMeshProUGUI restPuntuationTMP;

        public TextMeshProUGUI timerTicking;
        public Transform[] wordContainers;
        public SpriteRenderer constructorPanelTrans;

        public AcceptButtonController acceptButton;

        public void UpdateAccPuntAndBonMult(int _accumulatePuntuation, float _accBonusMultiplyer)
        {
            float accBonusMultRounded = Mathf.Round(_accBonusMultiplyer * 10f) / 10f; ;

            accumulatePuntuationTMP.text = _accumulatePuntuation <= 0 ? string.Empty : ("+" + _accumulatePuntuation.ToString());
            accumulateBonusMultiplayerTMP.text = accBonusMultRounded <= 1 ? string.Empty : ("x" + accBonusMultRounded.ToString());
        }

        public void UpdatePuntuation(int _puntuation)
        {
            puntuationTMP.text = _puntuation.ToString();
        }
        public void UpdateRestPuntuation(int _restPuntuation, GameManager _tableManager)
        {
            restPuntuationTMP.text = "-" + _restPuntuation.ToString();
            _tableManager.StartCoroutine(RestPuntuationFeedback(1));
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

        IEnumerator RestPuntuationFeedback(int _timerInSeconds)
        {
            restPuntuationTMP.color += new Color(0, 0, 0, 1f);

            while (restPuntuationTMP.color.a > 0.0f)
            {
                restPuntuationTMP.color -= new Color(0, 0, 0, (Time.deltaTime / _timerInSeconds));
                yield return null;
            }

        }
    }
    [SerializeField] UIElements uiElements;

    [Header("Word Validator")]
    [SerializeField] WordValidator wordValidator;

    [Header("Letters GameObjects")]
    [SerializeField] private List<GameObject> lettersInGameGO = new List<GameObject>();

    [Header("Constructor")]
    [SerializeField] ConstructorController constructorController;
   
    [Header("Board")]
    [SerializeField] Transform boardTrans;

    private Dictionary<string, LetterController> lettersCtrl = new Dictionary<string, LetterController>();
    private List<string> selectedLetters = new List<string>();

    [SerializeField] private GameObject wordPrefab;
    private string editingWord;
    private List<string> wordsCompleted = new List<string>();
    private List<GameObject> wordObjects = new List<GameObject>();

    private float bonusMultiplyer = 1f;
    private float accBonusMultiplyer;
    private int accPuntuation;
    private int puntuation;

    private float startSesionInSeconds;

    private void OnDisable()
    {
        wordValidator.OnWordValidationComplete -= OnWordValidationComplete;

        LetterController.OnLetterDrag -= CopyLetter;
        LetterController.OnLetterMouseUp -= AddLetter;

        LetterConstructor.OnLetterClicked -= RemoveLetter;
    }

    private void Awake()
    {
        if (!(StoryGameSettings.instance == null))
        {
            gameTimeSesionInSec = StoryGameSettings.instance.levelInfo.gameTimeSesionInSec;
            gameLanguage = StoryGameSettings.instance.levelInfo.gameLanguage;
        }
        
    }
    private void Start()
    {
        wordValidator.OnWordValidationComplete += OnWordValidationComplete;
        LetterController.OnLetterDrag += CopyLetter;
        LetterController.OnLetterMouseUp += AddLetter;

        LetterConstructor.OnLetterClicked += RemoveLetter;

        for (int i = 0; i < lettersInGameGO.Count; i++)
        {
            LetterController lCtrl = lettersInGameGO[i].GetComponent<LetterController>();
            lettersCtrl[lCtrl.GetLetterChar().ToString()] = lCtrl;
        }

        editingWord = string.Empty;
        startSesionInSeconds = Time.time;

        //VIEW
        constructorController.constructorView.UpdateEditingWord(editingWord, this);
        uiElements.UpdateAccPuntAndBonMult(accPuntuation, accBonusMultiplyer);
        uiElements.UpdatePuntuation(puntuation);
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.PLAYING:
                {
                    int actualTimer = gameTimeSesionInSec - (int)(Time.time - startSesionInSeconds);

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
            puntuation += (int)(accPuntuation * accBonusMultiplyer);

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
        accBonusMultiplyer = 0;

        //VIEW
        StartCoroutine(FadeToAndFromColor(colorFeedback, 0.5f));
        constructorController.constructorView.UpdateEditingWord(editingWord, this);
        uiElements.UpdateAccPuntAndBonMult(accPuntuation, accBonusMultiplyer);
        uiElements.UpdateAcceptButton(true);
    }
    private void ConcatenateLetters()
    {
        editingWord = string.Empty;
        foreach (string selectedLetter in selectedLetters)
        {          
            editingWord += selectedLetter;
        }
        constructorController.constructorView.UpdateEditingWord(editingWord, this);
    }
    private IEnumerator FadeToAndFromColor(Color color, float duration)
    {
        Color originalColor = uiElements.constructorPanelTrans.color;
        yield return UIHelper.Fade(uiElements.constructorPanelTrans, originalColor, color, duration);
        yield return UIHelper.Fade(uiElements.constructorPanelTrans, color, originalColor, duration);
        uiElements.constructorPanelTrans.color = Color.white;
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

                //Comprobamos que quepa en la misma fila
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
        int restAccPuntuation = 0;

        if (index != -1 && wordObjects[index].GetComponent<WordCompletedController>().preparedToRemove)
        {
            foreach (char letter in word)
            {
                lettersCtrl[letter.ToString()].ReturnLetter();
                restAccPuntuation += lettersCtrl[letter.ToString()].GetLetterPuntuation();
            }

            int restPuntuation = (int)(restAccPuntuation * (bonusMultiplyer * word.Length));
            puntuation -= restPuntuation;

            wordsCompleted.RemoveAt(index);
            Destroy(wordObjects[index]);
            wordObjects.RemoveAt(index);

            GenerateAcceptedWords();

            //VIEW
            uiElements.UpdateRestPuntuation(restPuntuation, this);
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

    public void CopyLetter(string _letterValue) 
    {
        GameObject letterCopy = Instantiate(lettersCtrl[_letterValue].gameObject, boardTrans); //esta copia se queda
        letterCopy.name = lettersCtrl[_letterValue].name;

        lettersCtrl[_letterValue].clone = letterCopy.GetComponent<LetterController>();
        lettersCtrl[_letterValue] = letterCopy.GetComponent<LetterController>();
    }
    public void AddLetter(string letterValue)
    {
        if (selectedLetters.Count < MAX_LETTERS_IN_WORD)
        {
            
            Debug.Log("Added " + letterValue + " IN " + (selectedLetters.Count).ToString());

            //Word
            selectedLetters.Add(letterValue);
            ConcatenateLetters();

            //Puntuation
            accPuntuation += lettersCtrl[letterValue].GetLetterPuntuation();
            accBonusMultiplyer += bonusMultiplyer;
            
            //VIEW
            uiElements.UpdateAccPuntAndBonMult(accPuntuation, accBonusMultiplyer);
        }
        else
        {
            lettersCtrl[letterValue].ReturnLetter();
        }
    }
    public void RemoveLetter(int _index = -1) //SI index == -1 ->Se borra la última referencia
    {
        if (selectedLetters.Count > 0)
        {
            int letterIndex = _index == -1? selectedLetters.Count - 1: _index;

            string letterValue = selectedLetters[letterIndex];

            Debug.Log("Removed " + letterValue + " IN " + (selectedLetters.Count - 1).ToString());

            //Word
            lettersCtrl[letterValue].ReturnLetter();
            selectedLetters.RemoveAt(letterIndex);
            ConcatenateLetters();

            //Puntuation
            accPuntuation -= lettersCtrl[letterValue].GetLetterPuntuation();
            accBonusMultiplyer -= bonusMultiplyer;

            //VIEW
            uiElements.UpdateAccPuntAndBonMult(accPuntuation, accBonusMultiplyer);
        }
    }
   

    public void ValidatorButton()
    {
        wordValidator.ValidateWord(editingWord);

        //VIEW
        uiElements.UpdateAcceptButton(false);
    }

    public void ChangeSceneTo(string _text) 
    {
        SceneChanger.Instance.LoadScene(_text);
    }

    public void RemoveAllLettersInConstructor()
    {
        constructorController.constructorView.RemoveWords(this);
    }
}
