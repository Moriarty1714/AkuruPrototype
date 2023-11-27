using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleAnimDOTww : MonoBehaviour
{
    public float scale, time;

    public bool invert = false;

    public GameObject[] afectedObj;

    void Start()
    {
        DOTween.Init();
        for (int i = 0; i < afectedObj.Length; i++)
        {
            afectedObj[i].transform.DOScale(0, 0);
        }
        
        ScaleUP();
    }
    public void ScaleUP()
    {
        for (int i = 0; i < afectedObj.Length; i++)
        {
            afectedObj[i].transform.DOScale(afectedObj[i].gameObject.transform.localScale, 1);
        }
    }
    public void ScaleDown()
    {
        for (int i = 0; i < afectedObj.Length; i++)
        {
            afectedObj[i].transform.DOScale(0, 1).OnComplete(Desactivate);
        }
    }
    void Desactivate()
    {
        this.gameObject.SetActive(false);
    }

}
