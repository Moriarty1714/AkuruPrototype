using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private enum GameState { PLAYING, GAMEENDED}
    private GameState gameState = GameState.PLAYING;

    private const int MAX_LETTERS_IN_WORD = 10;
    
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

        public GameObject posGameEndedPanel;
        public GameObject negGameEndedPanel;

        public TextMeshProUGUI playerCoinsTMP;

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
        public void UpdatePlayerCoins()
        {
            playerCoinsTMP.text = Profile.Instance.GetPlayerCoints().ToString();
        }


        public void GameEndedPanel(bool _isPos) 
        {
            if (_isPos)
                posGameEndedPanel.SetActive(true);
            else 
                negGameEndedPanel.SetActive(true);
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

    private Dictionary<string, LetterController> lettersCtrl = new Dictionary<string, LetterController>();
    public List<string> initSelectedLetters = new List<string>();

    [SerializeField] private GameObject wordPrefab;
    private string editingWord;
    public List<string> wordsCompleted = new List<string>();
    private List<GameObject> wordObjects = new List<GameObject>();

    private float bonusMultiplyer = 1f;
    private float accBonusMultiplyer;
    private int accPuntuation;
    private int puntuation;

    private float startSesionInSeconds;


    private void OnDisable()
    {
        wordValidator.OnWordValidationComplete -= OnWordValidationComplete;
        
        LetterController.OnLetterMouseUp -= AddLetter;
        LetterController.OnBuyLetter -= UpdatePlayerCoins;

        LetterConstructor.OnLetterMouseUp -= RemoveLetter;
        LetterConstructor.OnReturnLetterInLimbo -= ReturnLetterInLimbo;
        LetterConstructor.OnLetterMouseUpDraggging -= AddLetter;
        
    }

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (!(StoryGameSettings.instance == null))
        {
            gameTimeSesionInSec = StoryGameSettings.instance.levelInfo.gameTimeSesionInSec;
            gameLanguage = StoryGameSettings.instance.levelInfo.gameLanguage;
        }
        
    }
    private void Start()
    {
        wordValidator.OnWordValidationComplete += OnWordValidationComplete;
        
        LetterController.OnLetterMouseUp += AddLetter;
        LetterController.OnBuyLetter += UpdatePlayerCoins;

        LetterConstructor.OnLetterMouseUp += RemoveLetter;
        LetterConstructor.OnReturnLetterInLimbo += ReturnLetterInLimbo;
        LetterConstructor.OnLetterMouseUpDraggging += AddLetter;

        for (int i = 0; i < lettersInGameGO.Count; i++)
        {
            LetterController lCtrl = lettersInGameGO[i].GetComponent<LetterController>();
            lettersCtrl[lCtrl.GetLetterChar().ToString()] = lCtrl;
        }

        editingWord = string.Empty;
        startSesionInSeconds = Time.time;

        //VIEW
        constructorController.constructorView.UpdateEditingWord(editingWord, this);
        constructorController.constructorView.UpdateEditingWord(editingWord, this);
        uiElements.UpdateAccPuntAndBonMult(accPuntuation, accBonusMultiplyer);
        uiElements.UpdatePuntuation(puntuation);
        uiElements.UpdatePlayerCoins();

        if (initSelectedLetters.Count > 0) {
            foreach (string letter in initSelectedLetters)
            {
                AddLetter(letter);
            }
        }
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.PLAYING:
                {
                    int actualTimer = gameTimeSesionInSec - (int)(Time.time - startSesionInSeconds);

                    //View
                    uiElements.UpdateTimerTicking(actualTimer);

                    if (actualTimer <= 0)
                    {
                        gameState = GameState.GAMEENDED;
                        uiElements.GameEndedPanel(false);
                    }
                }
                break;
            case GameState.GAMEENDED:
                {
                  
                }
                break;            
            default:
                break;
        }
       
    }

    public void CheckBoardEmpty()
    {
        for (int i = 5; i < lettersInGameGO.Count; i++)
        {
            if (lettersInGameGO[i].GetComponent<LetterController>().letterState != LetterState.SHOP)
            {
                Debug.Log("Check" + lettersInGameGO[i].name);
                return;
            }
        }

        //EndGame
        gameState = GameState.GAMEENDED;
        Profile.Instance.SetActualPuntuation(puntuation);
        uiElements.GameEndedPanel(true);
    }
    private void OnWordValidationComplete(bool exists)
    {
        Color colorFeedback = Color.white;

        if (exists && !wordsCompleted.Contains(editingWord) && editingWord.Length > 2)
        {         
            //WordCompeted
            wordsCompleted.Add(editingWord);

            //Puntuation
            puntuation += (int)(accPuntuation * accBonusMultiplyer);

            //VIEW
            colorFeedback = Color.green;
            uiElements.UpdatePuntuation(puntuation);
            GenerateAcceptedWords();

            editingWord = string.Empty;

            accPuntuation = 0;
            accBonusMultiplyer = 0;

            CheckBoardEmpty();
        }
        else
        {
            colorFeedback = Color.red;
        }

        //VIEW
        StartCoroutine(FadeToAndFromColor(colorFeedback, 0.5f));
        constructorController.constructorView.UpdateEditingWord(editingWord, this);
        uiElements.UpdateAccPuntAndBonMult(accPuntuation, accBonusMultiplyer);
        uiElements.UpdateAcceptButton(true);
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
                wordObj.GetComponent<WordCompletedController>().SetWord(word + ",");
                wordObj.GetComponentInChildren<Button>().onClick.AddListener(() => RemoveAcceptedWord(word));

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

    public void RemoveAcceptedWord(string _word)
    {
        int index = wordsCompleted.IndexOf(_word);
        int restAccPuntuation = 0;

        if (index != -1)
        {
            ReturnEditingWord();

            foreach (char letter in _word)
            {
                restAccPuntuation += lettersCtrl[letter.ToString()].GetLetterPuntuation();
            }

            int restPuntuation = (int)(restAccPuntuation * (bonusMultiplyer * _word.Length));

            puntuation = puntuation - restPuntuation >= 0 ? puntuation - restPuntuation : 0; 

            wordsCompleted.RemoveAt(index);

            GenerateAcceptedWords();

            //VIEW
            uiElements.UpdateRestPuntuation(restPuntuation, this);
            uiElements.UpdatePuntuation(puntuation);

            foreach (char letter in _word)
            {
                AddLetter(letter.ToString());
            }
        }
    }

    public void ReturnEditingWord()
    {
        int newletters = editingWord.Length;
        for (int i = 0; i < newletters; i++)
        {
            RemoveLetter(0);
        }
    }
  
    public void AddLetter(string letterValue, int _index = -1) //SI index == -1 ->Se borra la última referencia)
    {
        if (editingWord.Length < MAX_LETTERS_IN_WORD)
        {
            int letterIndex = _index == -1 ? editingWord.Length : _index;

            Debug.Log("Added " + letterValue + " IN " + _index.ToString());

            //Word
            editingWord = editingWord.Insert(letterIndex,letterValue); 
            constructorController.constructorView.UpdateEditingWord(editingWord, this);

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
    public void RemoveLetter(int _index, bool _isDrag = false) //SI index == -1 ->Se borra la última referencia
    {
        if (editingWord.Length > 0)
        {
            int letterIndex = _index == -1? editingWord.Length - 1: _index;
            string letterValue = editingWord[letterIndex].ToString();

            Debug.Log("Removed " + letterValue + " IN " + letterIndex);

            //Word
            if(!_isDrag)
            lettersCtrl[letterValue].ReturnLetter();

            editingWord = editingWord.Remove(letterIndex);
            constructorController.constructorView.UpdateEditingWord(editingWord, this);

            //Puntuation
            accPuntuation -= lettersCtrl[letterValue].GetLetterPuntuation();
            accBonusMultiplyer -= bonusMultiplyer;

            //VIEW
            uiElements.UpdateAccPuntAndBonMult(accPuntuation, accBonusMultiplyer);
        }
    }

    public void RemoveLetter(int _index) //SI index == -1 ->Se borra la última referencia
    {
        if (editingWord.Length > 0)
        {
            int letterIndex = editingWord.Length - 1;
            string letterValue = editingWord[letterIndex].ToString();

            Debug.Log("Removed " + letterValue + " IN " + letterIndex);
            
            lettersCtrl[letterValue].ReturnLetter();

            editingWord =editingWord.Remove(letterIndex);
            constructorController.constructorView.UpdateEditingWord(editingWord, this);

            //Puntuation
            accPuntuation -= lettersCtrl[letterValue].GetLetterPuntuation();
            accBonusMultiplyer -= bonusMultiplyer;

            //VIEW
            uiElements.UpdateAccPuntAndBonMult(accPuntuation, accBonusMultiplyer);
        }
    }

    public void ReturnLetterInLimbo( string _letter) //SI index == -1 ->Se borra la última referencia
    {
            string letterValue = _letter;
            lettersCtrl[_letter].ReturnLetter();
    }

    public void UpdatePlayerCoins(bool _isPossible) {
       
        if(_isPossible)uiElements.UpdatePlayerCoins();
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

    //public void RemoveAllLettersInConstructor()
    //{
    //    constructorController.constructorView.UpdateEditingWord("", this);

    //    foreach (char letter in editingWord)
    //    {
    //        lettersCtrl[letter.ToString()].ReturnLetter();
    //    }

    //    selectedLetters.Clear();
    //    editingWord = string.Empty;
    //}
}
