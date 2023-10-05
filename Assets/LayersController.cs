using UnityEngine;

public class LayersController : MonoBehaviour
{
    [SerializeField] public Canvas canvas;
    [SerializeField] public RectTransform parentSceneLayerRT;
    private Vector2[] sceneLayersPosition;

    private float limit;

    private Vector2 initLayerPos;
    private float initTouchPos;
    public float fMovingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        limit = Screen.width / 2;
        initLayerPos = parentSceneLayerRT.localPosition;

        RectTransform[] sceneLayersRT = parentSceneLayerRT.gameObject.GetComponentsInChildren<RectTransform>();
        sceneLayersPosition = new Vector2[sceneLayersRT.Length];

        for (int i = 0; i < sceneLayersRT.Length; i++)
        {
            sceneLayersPosition[i] = sceneLayersRT[i].anchoredPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        if (Input.GetMouseButtonDown(0))
        {
            initTouchPos = Input.mousePosition.x;

        }
        else if (Input.GetMouseButton(0))
        {
            float dragPos = Input.mousePosition.x;
            parentSceneLayerRT.localPosition = new Vector2((initLayerPos.x - (initTouchPos - dragPos)), parentSceneLayerRT.localPosition.y);
        }
        else
        {
            parentSceneLayerRT.localPosition = Vector2.MoveTowards(transform.position, initLayerPos, fMovingSpeed);
        }
    }
    
}
