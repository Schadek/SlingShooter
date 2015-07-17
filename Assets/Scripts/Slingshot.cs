using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour
{

    // Fields set in the Unity Inspector pane
    public GameObject prefabProjectile;
    public float velocityMult = 20f;

    // Fields set dynamically
    private GameObject launchPoint;
    private Vector3 launchPos;
    private GameObject projectile;
    private bool aimingMode;

    void Awake()
    {
        Transform launchPointTrans = transform.FindChild("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

    void OnMouseEnter()
    {
        //print ("Enter");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        //print ("Exit");
        if (!aimingMode)
            launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        //print ("Down");

        // Player pressed mouse while over Slingshot
        aimingMode = true;

        // Instantiate a projectile
        projectile = Instantiate(prefabProjectile) as GameObject;

        // Start it at launch position
        projectile.transform.position = launchPos;

        // Set it to kinematic for now
        projectile.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    void Update()
    {
        // If the Slingshot is not in aiming mode, don't run this code
        if (!aimingMode) return;

        // Get the current mouse position in 2D screen coordinates
        Vector3 mousePos = Input.mousePosition;
        // Convert the mouse position to 3D world coordinates
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos);

        // Find the delta from launch position to 3D mouse position
        Vector3 mouseDelta = mousePos3D - launchPos;

        // Limit mouseDelta to the radius of the Slingshot SphereCollider
        float maxMagnitude = GetComponent<CircleCollider2D>().radius;
        mouseDelta = Vector3.ClampMagnitude(mouseDelta, maxMagnitude);

        // Now move the projectile to this new position
        projectile.transform.position = launchPos + mouseDelta;

        if (Input.GetMouseButtonUp(0))
        {
            // The mouse has been released
            aimingMode = false;
            // Fire off the projectile with given velocity
            projectile.GetComponent<Rigidbody2D>().isKinematic = false;
            projectile.GetComponent<Rigidbody2D>().velocity = -mouseDelta * velocityMult;

            SceneInformation.Instance.allObjects.Add(projectile);

            // Set the Followcam's target to our projectile

            // Set the reference to the projectile to null as early as possible
            projectile = null;
        }

    }
}
