using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AlterateSprite : MonoBehaviour
{
    public float scale, time;

    public bool invert = false;

    public GameObject[] afectedObj;

    public SpriteRenderer spr;

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        DOTween.Init();
        ScaleUP();
    }
    public void ScaleUP()
    {
        for (int i = 0; i < afectedObj.Length; i++)
        {
            afectedObj[i].transform.DOScale(new Vector3(afectedObj[i].gameObject.transform.localScale.x + scale,
                afectedObj[i].gameObject.transform.localScale.y + scale,
                afectedObj[i].gameObject.transform.localScale.z + scale), 1).OnComplete(ScaleDown);

            //spr.color = spr.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }
    public void ScaleDown()
    {
        for (int i = 0; i < afectedObj.Length; i++)
        {
            afectedObj[i].transform.DOScale(new Vector3(
                afectedObj[i].gameObject.transform.localScale.x - scale,
                afectedObj[i].gameObject.transform.localScale.y - scale,
                afectedObj[i].gameObject.transform.localScale.z - scale), 1).OnComplete(ScaleUP);

            //spr.color = spr.color = new Color(1f, 1f, 1f, 1f);
        }
    }
    void Desactivate()
    {
        this.gameObject.SetActive(false);
    }
}
