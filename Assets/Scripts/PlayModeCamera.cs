using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayModeCamera : MonoBehaviour 
{
    public float maxX;
    public float minX;
    [Space(10)]
    public float marginSide;
    public float distancePerFrame;
    [Space(10)]
    public Image cameraSprite;

    private bool followProjectile = false;
    private Camera mainCam;

    private void Start()
    {
        mainCam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (followProjectile)
        {
            if (SceneInformation.Instance.currentProjectile)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(SceneInformation.Instance.currentProjectile.transform.position.x, transform.position.y, transform.position.z), 0.1f);
                return;
            }
            else if (SceneInformation.Instance.lastFireSlingshot)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(SceneInformation.Instance.lastFireSlingshot.position.x, transform.position.y, transform.position.z), 0.1f);
                return;
            }
        }

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

    public void ToggleCameraMode()
    {
        followProjectile = !followProjectile;

        if (followProjectile)
        {
            cameraSprite.color = Color.white;
        }
        else
        {
            cameraSprite.color = Color.gray;
        }
    }
}
