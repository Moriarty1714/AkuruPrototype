using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveBtwTwoPoints : MonoBehaviour
{
    public GameObject[] posRecorrido; // Array de GameObjects a iluminar en secuencia

    public GameObject objetoAMover, sprite; // GameObject a mover en la secuencia

    public float time;
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();

        objetoAMover.transform.position = posRecorrido[0].transform.position;

        //objetoAMover.transform.DOMove(posRecorrido[1].transform.position, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        objetoAMover.transform.DOMove(posRecorrido[1].transform.position, time).OnComplete(MoveToPos1);
    }

    public void MoveToPos1()
    {
        sprite.SetActive(false);
        objetoAMover.transform.DOMove(posRecorrido[0].transform.position, time/2f).OnComplete(MoveToPos2);
    }
    
    public void MoveToPos2()
    {
        sprite.SetActive(true);
        objetoAMover.transform.DOMove(posRecorrido[1].transform.position, time).OnComplete(MoveToPos1);
    }
}
