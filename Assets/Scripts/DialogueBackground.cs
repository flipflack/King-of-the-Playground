using UnityEngine;

// Shows the current NPC's blurred background behind the characters.
// GameManager finds this in the scene (or creates one) and calls SetBackground.
[RequireComponent(typeof(SpriteRenderer))]
public class DialogueBackground : MonoBehaviour
{
    [Tooltip("Scale the background so it always covers the whole camera view.")]
    [SerializeField] private bool fitToCamera = true;

    [Tooltip("Sorting order for the background. Keep it lower than the characters.")]
    [SerializeField] private int sortingOrder = -100;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = sortingOrder;
    }

    public void SetBackground(Sprite sprite)
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();

        sr.sprite = sprite;
        sr.enabled = sprite != null;
        if (sprite != null && fitToCamera) FitToCamera();
    }

    private void FitToCamera()
    {
        Camera cam = Camera.main;
        if (cam == null || !cam.orthographic || sr.sprite == null) return;

        float viewHeight = cam.orthographicSize * 2f;
        float viewWidth = viewHeight * cam.aspect;
        Vector2 spriteSize = sr.sprite.bounds.size;

        // "cover" scaling: fill the screen, crop the overflow
        float scale = Mathf.Max(viewWidth / spriteSize.x, viewHeight / spriteSize.y);
        transform.localScale = new Vector3(scale, scale, 1f);

        // centre the sprite on the camera no matter where its pivot is
        Vector3 offset = Vector3.Scale(sr.sprite.bounds.center, transform.localScale);
        Vector3 camPos = cam.transform.position;
        transform.position = new Vector3(camPos.x - offset.x, camPos.y - offset.y, transform.position.z);
    }
}
