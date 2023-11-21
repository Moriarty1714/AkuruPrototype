using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class DynamicColliderAdjuster : MonoBehaviour
{
    public RectTransform smallSquare;

    public BoxCollider2D upLeftCollider;
    public BoxCollider2D upRightCollider;
    public BoxCollider2D downLeftCollider;
    public BoxCollider2D downRightCollider;

    private RectTransform canvasRectTransform;

    void Start()
    {
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        AdjustColliders();
    }

    void Update()
    {
        AdjustColliders();
    }

    void AdjustColliders()
    {
        // Obtén la escala del canvas para manejar la resolución de la pantalla correctamente
        float canvasScale = canvasRectTransform.localScale.x;

        // Calcular el tamaño del canvas en el espacio del mundo
        Vector2 canvasSize = new Vector2(
            canvasRectTransform.sizeDelta.x * canvasScale,
            canvasRectTransform.sizeDelta.y * canvasScale
        );

        // Calcular la posición y tamaño del smallSquare en el espacio del mundo
        Vector2 squareSize = new Vector2(
            smallSquare.rect.width * canvasScale,
            smallSquare.rect.height * canvasScale
        );

        Vector2 squarePos = new Vector2(
            smallSquare.anchoredPosition.x * canvasScale,
            smallSquare.anchoredPosition.y * canvasScale
        );

        // Asegúrate de que el pivote del smallSquare esté en el centro
        if (smallSquare.pivot != new Vector2(0.5f, 0.5f))
        {
            Debug.LogWarning("El pivote de smallSquare debe estar centrado.");
        }

        // Ajustar los colliders para que rodeen el smallSquare
        // Los colliders se posicionan en el espacio local respecto al centro del canvas

        // Collider superior izquierdo
        upLeftCollider.size = new Vector2(squarePos.x, (canvasSize.y - squarePos.y) / 2);
        upLeftCollider.offset = new Vector2((-canvasSize.x + squareSize.x) / 4, squarePos.y + squareSize.y / 2);

        // Collider superior derecho
        upRightCollider.size = new Vector2((canvasSize.x - squarePos.x - squareSize.x) / 2, (canvasSize.y - squarePos.y) / 2);
        upRightCollider.offset = new Vector2(squarePos.x + (3 * squareSize.x) / 4, squarePos.y + squareSize.y / 2);

        // Collider inferior izquierdo
        downLeftCollider.size = new Vector2(squarePos.x, (canvasSize.y + squarePos.y - squareSize.y) / 2);
        downLeftCollider.offset = new Vector2((-canvasSize.x + squareSize.x) / 4, (-canvasSize.y + squareSize.y) / 4);

        // Collider inferior derecho
        downRightCollider.size = new Vector2((canvasSize.x - squarePos.x - squareSize.x) / 2, (canvasSize.y + squarePos.y - squareSize.y) / 2);
        downRightCollider.offset = new Vector2(squarePos.x + (3 * squareSize.x) / 4, (-canvasSize.y + squareSize.y) / 4);
    }
}
