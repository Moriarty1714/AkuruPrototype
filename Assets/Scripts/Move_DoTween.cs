using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Move_DoTween : MonoBehaviour
{
    public float distance, time;

    void Start()
    {
        DOTween.Init();

        MoveUP();
    }

    void MoveUP()
    {
        transform.DOMoveY(transform.position.y + distance, time).OnComplete(MoveDown);
    }
    void MoveDown()
    {
        transform.DOMoveY(transform.position.y - distance, time).OnComplete(MoveUP);
    }

}
