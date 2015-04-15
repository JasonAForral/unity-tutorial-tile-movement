using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public Transform dolly;

    float zoomSpeed = 50f;
    float zoomLevel = 1f;
    float panSpeed = 1f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float scroller = Input.GetAxis("Mouse ScrollWheel");
        //transform.Translate(Vector3.forward * scroller);
        float camHorizontal = Input.GetAxis("Horizontal");
        float camVertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(camHorizontal, 0.0f, camVertical) * panSpeed);
        //transform.Translate(Vector3.right * camHorizontal + Vector3.forward * camVertical);
        if ((2f > zoomLevel && 0 < scroller) || (0f < zoomLevel && 0 > scroller))
        {
            dolly.Translate(Vector3.forward * scroller * zoomSpeed);
            zoomLevel += scroller;
            panSpeed = 1.5f - (zoomLevel / 2f);
            panSpeed = Mathf.Round(panSpeed * 10) / 10;

        }
    }
}
