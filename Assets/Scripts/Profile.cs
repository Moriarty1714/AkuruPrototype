using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Profile
{
    private static Profile _instance;

    private int playerCoins = 0;
    // Constructor is private to prevent instantiation from outside the class.
    private Profile()
    {
        playerCoins = 0;
        // Initialize your singleton here
    }

    // Public property to access the instance
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

    // Example of a method in the singleton
    public bool UpdateProfileCoints(int _coins)
    {
        if (_coins + playerCoins >= 0) 
        {
            playerCoins += _coins;
            return true;
        }           

        return false;
    }

    public int GetPlayerCoints() 
    { 
        return playerCoins;
    }
}
