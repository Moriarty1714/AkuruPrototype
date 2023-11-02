using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePrefab : MonoBehaviour
{
    public GameObject obj;
    bool state = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActivatePrefab()
    {     
        if (state)
        {
            obj.SetActive(true);
            state = false;
        }
        else
        {
            obj.SetActive(false);
            state = true;
        }
    }
}
