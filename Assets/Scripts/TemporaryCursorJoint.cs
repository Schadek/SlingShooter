using UnityEngine;
using System.Collections;

public class TemporaryCursorJoint : MonoBehaviour 
{
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        Vector3 newPosition;
        newPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        transform.position = newPosition;
    }
}
