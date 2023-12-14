using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Profile
{
    private static Profile _instance;

    private int playerCoins = 0;
    private int actualPlayPuntuation = -1;
    private Profile()
    {
        // Initialize your singleton here
    }

    public static Profile Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Profile();
            }
            return _instance;
        }
    }

    public bool CanBuy(int _coins)
    {
        return _coins + playerCoins >= 0;
    }
    public void UpdateProfileCoints(int _coins)
    {
        if (_coins + playerCoins >= 0) 
        {
            playerCoins += _coins;
        }           
    }

    public void SetActualPuntuation(int _puntuation) 
    { 
        actualPlayPuntuation = _puntuation;
    }


    public int GetPlayerCoints() 
    { 
        return playerCoins;
    }

    public int GetActualPuntuation() 
    {
        return actualPlayPuntuation;
    }
}
