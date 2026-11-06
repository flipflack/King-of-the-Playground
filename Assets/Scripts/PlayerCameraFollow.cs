using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float dampTime = 0f;
    [SerializeField] private Vector3 followOffset = Vector3.zero;

    private Vector3 velocity = Vector3.zero;
    private Vector3 cameraOffset = new Vector3(0,0,-10f);

    private void Update()
    {
        if (target)
        {
        Vector3 targetDest = target.position + followOffset + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetDest, ref velocity, dampTime);
        }

    }

}
