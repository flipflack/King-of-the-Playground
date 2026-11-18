using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float dampTime = 0f;
    [SerializeField] private Vector3 followOffset = Vector3.zero;

    private Vector3 velocity = Vector3.zero;
    private Vector3 cameraOffset = new Vector3(0,0,-10f);

    private float fixedY;

    //Variables to have Camera be locked (X min and max bound) Not b
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private float manualMinX = -10f;
    [SerializeField] private float manualMaxX = 10f;
    private float minBoundX;
    private float maxBoundX;

    private Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        fixedY = transform.position.y;
        if (background != null)
        {
            Bounds b = background.bounds;
            minBoundX = b.min.x;
            maxBoundX = b.max.x;
        }
        else
        {
            minBoundX = manualMinX;
            maxBoundX = manualMaxX;
        }
    }

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 targetDest = target.position + followOffset + cameraOffset;
        targetDest.y = fixedY;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float minX = minBoundX + halfWidth;
        float maxX = maxBoundX - halfWidth;

        if (minX <= maxX)
        {
            targetDest.x = Mathf.Clamp(targetDest.x, minX, maxX);
        }
        else
        {
            // Background is narrower than the screen, so center the camera
            targetDest.x = (minBoundX + maxBoundX) * 0.5f;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetDest, ref velocity, dampTime);

}

}
