using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LayersController : MonoBehaviour
{
    [SerializeField] public Canvas canvas;
    [SerializeField] public RectTransform parentSceneLayerRT;
    [SerializeField] public LayoutElement[] botLayoutButtons;
    public float timeBounce = 0.25f;
    public float sceneLayerOffset = 300f;
    
    private IEnumerator layerTransitionCor;

    // Start is called before the first frame update
    void Start()
    {
        ChangeLayerByButton(1);
    }

    // Update is called once per frame
    private void Update()
    {
            
    }

    //private void OnMouseEnter()
    //{
    //    //Movement
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        initTouchPos = Input.mousePosition.x;

    //    }
    //    else if (Input.GetMouseButton(0))
    //    {
    //        float dragPos = Input.mousePosition.x;
    //        parentSceneLayerRT.localPosition = new Vector2((initLayerPos.x - (initTouchPos - dragPos)), parentSceneLayerRT.localPosition.y);
    //    }
    //}

    public void ChangeLayerByButton(int index) 
    {
        if (layerTransitionCor != null) StopCoroutine(layerTransitionCor);


        if (index == 0)
        {
            layerTransitionCor = ScrollLayer(sceneLayerOffset, timeBounce);
            StartCoroutine(layerTransitionCor);

            //View
            botLayoutButtons[0].preferredWidth = 150;
            botLayoutButtons[1].preferredWidth = 100;
            botLayoutButtons[2].preferredWidth = 100;
        }
        else if (index == 1)
        {
            layerTransitionCor = ScrollLayer(0, timeBounce);
            StartCoroutine(layerTransitionCor);

            //View
            botLayoutButtons[0].preferredWidth = 100;
            botLayoutButtons[1].preferredWidth = 150;
            botLayoutButtons[2].preferredWidth = 100;
        }
        else if (index == 2)
        {
            layerTransitionCor = ScrollLayer(-sceneLayerOffset, timeBounce);
            StartCoroutine(layerTransitionCor);

            //View
            botLayoutButtons[0].preferredWidth = 100;
            botLayoutButtons[1].preferredWidth = 100;
            botLayoutButtons[2].preferredWidth = 150;
        }
        else 
        {
            Debug.Log("Layer doesn't exist!");
        }
    }

    IEnumerator ScrollLayer(float position, float time)
    {
        Vector2 currentPos = parentSceneLayerRT.localPosition;
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / time; //Tiepo de deslizamiento
            parentSceneLayerRT.localPosition = Vector3.Lerp(currentPos, new Vector3(position, parentSceneLayerRT.localPosition.y), t);
            yield return null;
        }
    }
}

