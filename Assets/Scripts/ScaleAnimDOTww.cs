using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScaleAnimDOTww : MonoBehaviour
{
    private float time =0.5f;

    public bool invert = false;

    public GameObject[] afectedObj;

    private Vector2[] normalScale;

    void Start()
    {
        normalScale = new Vector2[afectedObj.Length];

        DOTween.Init();
        for (int i = 0; i < afectedObj.Length; i++)
        {
            normalScale[i] = afectedObj[i].transform.localScale;
            afectedObj[i].transform.localScale = new Vector2(0, 0);
        }

        StartCoroutine(WaitScaleDownAndScaleUp());
    }
    public void ScaleUP()
    {
        for (int i = 0; i < afectedObj.Length; i++)
        {
            afectedObj[i].transform.GetComponent<RectTransform>().DOScale(normalScale[i], time);
        }
    }
    public void ScaleDown()
    {
        for (int i = 0; i < afectedObj.Length; i++)
        {
            afectedObj[i].transform.DOScale(0, time).OnComplete(Desactivate);
        }
    }
    void Desactivate()
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator WaitScaleDownAndScaleUp() {
        yield return new WaitForSeconds(time);

        ScaleUP();
    }
}
