using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomRotation : MonoBehaviour
{
    [Space(15)]
    [Header("[Associar] Object a ser escalonado")]
    [SerializeField] private GameObject objeto;

    [Space(15)]
    [Header("[Definir] Zoom ")]
    [SerializeField] private float zoomOutMin = 0;
    [SerializeField] private float zoomInMax = 10;
    [Space(15)]
    [Header("[Definir] Speed Zoom ")]
    [SerializeField] private float zoomSpeedMobile = 1;
    [SerializeField] private float zoomSpeedPC = 100;


    [Space(15)]
    [Header("[Definir] Speed Rotacao  ")]
    [SerializeField] private float rotateSpeed = 0.2f;



    private bool zooming, blockZoom;
    private float pointer_x = 0;
    private float pointer_y = 0;

    private void Awake()
    {
        if (objeto == null)
        {
            objeto = transform.gameObject;
        }

    }
    private void Update()
    {

        if (Input.touchCount == 2 && blockZoom)
        {
            if (zooming == false)
                zooming = true;

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrePos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrePos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrePos - touchOnePrePos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            ZoomImage(-difference * (0.001f * zoomSpeedMobile));
        }
        else
        {
            zooming = false;
        }

        ZoomImage(Input.GetAxis("Mouse ScrollWheel") * (0.001f * zoomSpeedPC));
    }



    private void OnMouseDrag()
    {
        blockZoom = true;
        if (zooming == false)
        {
#if UNITY_EDITOR
            pointer_x = Input.GetAxis("Mouse X");
            pointer_y = Input.GetAxis("Mouse Y");
#else
        Touch screenTouch = Input.GetTouch(0);
        pointer_x = screenTouch.deltaPosition.x;
        pointer_y = screenTouch.deltaPosition.y;
#endif

            if (objeto != null)
            {
                objeto.transform.Rotate(0f, -pointer_x * rotateSpeed, 0f, Space.World);
            }
        }
    }
    private void OnMouseUp()
    {
        blockZoom = false;
    }

    private void ZoomImage(float increment)
    {
        if (objeto != null)
        {
            objeto.transform.localScale = new Vector3(
         Mathf.Clamp(objeto.transform.localScale.x - increment, zoomOutMin, zoomInMax),
         Mathf.Clamp(objeto.transform.localScale.y - increment, zoomOutMin, zoomInMax),
         Mathf.Clamp(objeto.transform.localScale.z - increment, zoomOutMin, zoomInMax)
         );
        }
    }


}
