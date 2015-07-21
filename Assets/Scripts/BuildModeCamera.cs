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
        if (Input.GetAxis("Horizontal") > 0 && transform.position.x + distancePerFrame < maxX)
        {
            transform.position += new Vector3(distancePerFrame, 0, 0);
        }

        if (Input.GetAxis("Horizontal") < 0 && transform.position.x - distancePerFrame > minX)
        {
            transform.position -= new Vector3(distancePerFrame, 0, 0);
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
