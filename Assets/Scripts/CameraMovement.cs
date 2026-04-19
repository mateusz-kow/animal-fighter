using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    
    private float minX, maxX, minY, maxY;

    public void SetBounds(float mapSize)
    {
        Camera cam = GetComponent<Camera>();
        
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        float worldMin = -0.5f;
        float worldMax = mapSize - 0.5f;

        minX = worldMin + camHalfWidth;
        maxX = worldMax - camHalfWidth;
        minY = worldMin + camHalfHeight;
        maxY = worldMax - camHalfHeight;

        if (maxX < minX) minX = maxX = (worldMax + worldMin) / 2f;
        if (maxY < minY) minY = maxY = (worldMax + worldMin) / 2f;
    }

    void LateUpdate()
    {
        if (!target) return;

        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        Vector3 smoothed = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        
        transform.position = new Vector3(
            Mathf.Clamp(smoothed.x, minX, maxX),
            Mathf.Clamp(smoothed.y, minY, maxY),
            transform.position.z
        );
    }
}