using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class Move_DoTween : MonoBehaviour
{
    public float distance, time;

    public bool xAxis = false, yAxis = false;

    public bool invert = false;

    void Start()
    {
        DOTween.Init();

       StartCoroutine(WaitForPopUpScaleUp());
    }

    void MoveYUP()
    {
        transform.DOMoveY(transform.position.y + distance, time).OnComplete(MoveYDown);
    }
    void MoveYDown()
    {
        transform.DOMoveY(transform.position.y - distance, time).OnComplete(MoveYUP);
    }
    
    void MoveXUP()
    {
        transform.DOMoveX(transform.position.x + distance, time).OnComplete(MoveXDown);
    }
    void MoveXDown()
    {
        transform.DOMoveX(transform.position.x - distance, time).OnComplete(MoveXUP);
    }

    IEnumerator WaitForPopUpScaleUp() {
        yield return new WaitForSeconds(1f);

        if (xAxis)
        {
            if (invert)
                MoveXUP();
            else
                MoveXDown();
        }
        else if (yAxis)
        {
            if (invert)
                MoveYUP();
            else
                MoveYDown();
        }
    }
}
