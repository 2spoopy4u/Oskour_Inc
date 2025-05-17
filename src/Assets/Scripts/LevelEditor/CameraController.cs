using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 5f;
    public float zoomStep = 0.1f;
    public float zoomLerpSpeed = 10f;
    public float minZoom = 2f;
    public float maxZoom = 20f;

    private Camera cam;
    private float targetZoom;

    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            float scroll = Input.mouseScrollDelta.y;
            if (scroll != 0f)
            {
                // 1. Position monde sous la souris avant zoom
                Vector3 mouseWorldBeforeZoom = cam.ScreenToWorldPoint(Input.mousePosition);

                // 2. Modifier le targetZoom
                float zoomChange = -scroll * cam.orthographicSize * zoomStep;
                targetZoom = Mathf.Clamp(targetZoom + zoomChange, minZoom, maxZoom);

                // 3. Appliquer immédiatement le zoom (sans Lerp)
                cam.orthographicSize = targetZoom;

                // 4. Position monde sous la souris après zoom
                Vector3 mouseWorldAfterZoom = cam.ScreenToWorldPoint(Input.mousePosition);

                // 5. Décalage entre les deux, on déplace la caméra dans l’autre sens
                Vector3 zoomOffset = mouseWorldBeforeZoom - mouseWorldAfterZoom;
                transform.position += zoomOffset;
            }

            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.unscaledDeltaTime * zoomLerpSpeed);
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 move = new Vector3(-Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"), 0);
            transform.Translate(move * panSpeed * Time.unscaledDeltaTime * cam.orthographicSize, Space.World);
        }
    }
}