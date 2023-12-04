using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static ConstructorController;

public class GameOverController : MonoBehaviour
{
    [System.Serializable]
    public class GameOverView 
    {
        [SerializeField] TextMeshProUGUI puntuation;

        public void SetPlayPuntuation(int _puntuation)
        { 
            puntuation.text = _puntuation.ToString();
        }

    }
    public GameOverView gameOverView;
    private void OnEnable()
    {
        gameOverView.SetPlayPuntuation(Profile.Instance.GetActualPuntuation());
    }

    void Update()
    {
        
    }
}
