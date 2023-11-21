using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleAnimDOTww : MonoBehaviour
{
    public float scale, time;

    public bool invert = false;

    void Start()
    {
        DOTween.Init();
        transform.DOScale(0, 0);
        ScaleUP();
    }

    public void ScaleUP()
    {
        transform.DOScale(1, 1);
    }
    public void ScaleDown()
    {
        transform.DOScale(0, 1).OnComplete(Desactivate);
    }

    void Desactivate()
    {
        this.gameObject.SetActive(false);
    }

}
