using UnityEngine;
public class DynamicColliderAdjuster : MonoBehaviour
{
    public Camera camera;
    public Transform smallSquare;

    public BoxCollider2D upLeftCollider;
    public BoxCollider2D upRightCollider;
    public BoxCollider2D downLeftCollider;
    public BoxCollider2D downRightCollider;

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


