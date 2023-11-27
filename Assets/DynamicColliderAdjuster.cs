using UnityEngine;
public class DynamicColliderAdjuster : MonoBehaviour
{
    public Camera camera;
    public Transform smallSquare;



    private Transform tutorialBlackFilter;
    private Vector2 centerFilter;

    void Start()
    {
        FitToScreen(tutorialBlackFilter, camera);

        //AdjustColliders();
    }

    void Update()
    {

    }

    public static void FitToScreen(Transform transform, Camera camera)
    {
        if (camera.orthographic)
        {
            float screenHeightInWorld = camera.orthographicSize * 2;
            float screenWidthInWorld = screenHeightInWorld * camera.aspect;

            // Aplica el nuevo localScale al transform
            transform.localScale = new Vector3(screenWidthInWorld, screenHeightInWorld, 1); ;
        }
        else {

            Debug.LogError("Camera must be ortograhic!");
        }
    }
}


