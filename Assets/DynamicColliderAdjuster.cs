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

    void Start()
    {
        tutorialBlackFilter = GetComponent<Transform>();

        FitToScreen(tutorialBlackFilter, camera);

        //AdjustColliders();
    }

    void Update()
    {
        AdjustColliders();
    }

    void AdjustColliders()
    {
        // Tamaño de la pantalla en unidades del mundo
        Vector2 screenSize = new Vector2(camera.orthographicSize * camera.aspect * 2, camera.orthographicSize * 2);

        // Posición y escala del cuadrado blanco
        Vector2 holePosition = (Vector2)smallSquare.position;
        Vector2 holeSize = smallSquare.localScale;

        // Calcular el tamaño del collider 'upLeft'
        Vector2 upLeftSize = new Vector2(Mathf.Abs((screenSize.x / 2) - (holePosition.x - holeSize.x / 2)), Mathf.Abs((screenSize.y / 2) - (holePosition.y + holeSize.y / 2)));

        // Calcular el desplazamiento del collider 'upLeft'
        Vector2 upLeftOffset = new Vector2((-screenSize.x / 2) + (upLeftSize.x / 2), (screenSize.y / 2) - (upLeftSize.y / 2));

        // Aplicar tamaño y desplazamiento al collider 'upLeft'
        upLeftCollider.size = upLeftSize;
        upLeftCollider.offset = upLeftOffset + new Vector2((holeSize.x / 2), -(holeSize.y / 2));
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
