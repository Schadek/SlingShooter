using UnityEngine;
using System.Collections;

public class BuildModeCamera : MonoBehaviour 
{
    public float maxX;
    public float minX;
    [Space(10)]
    public float marginSide;
    public float distancePerFrame;

    private void FixedUpdate()
    {
        if (transform.position.x + Input.GetAxis("Horizontal") * 0.15f < maxX && transform.position.x + Input.GetAxis("Horizontal") * 0.15f > minX)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal") * 0.15f, 0, 0);
        }

        if (Input.mousePosition.x >= Screen.width - marginSide && transform.position.x + distancePerFrame < maxX)
        {
            transform.position += new Vector3(distancePerFrame, 0, 0);
        }

        if (Input.mousePosition.x <= marginSide && transform.position.x - distancePerFrame > minX)
        {
            transform.position -= new Vector3(distancePerFrame, 0, 0);
        }
    }
}
