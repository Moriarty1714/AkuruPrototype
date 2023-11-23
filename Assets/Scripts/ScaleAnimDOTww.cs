using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleAnimDOTww : MonoBehaviour
{
    public float scale, time;

    public bool invert = false;

    public GameObject popUP;

    void Start()
    {
        DOTween.Init();
        popUP.transform.DOScale(0, 0);
        ScaleUP();
    }

    public void ScaleUP()
    {
        popUP.transform.DOScale(1, 1);
    }
    public void ScaleDown()
    {
        popUP.transform.DOScale(0, 1).OnComplete(Desactivate);
    }

    void Desactivate()
    {
        this.gameObject.SetActive(false);
    }

}
