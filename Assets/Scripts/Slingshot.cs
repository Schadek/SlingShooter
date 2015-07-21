using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Slingshot : MonoBehaviour
{

    // Fields set in the Unity Inspector pane
    public GameObject lineRendererPrefab;
    public float velocityMult = 20f;
    public List<GameObject> birds = new List<GameObject>();

    // Fields set dynamically
    private GameObject launchPoint;
    private GameObject[] queueBirds = new GameObject[0];

    private Vector3 launchPos;
    private GameObject projectile;
    private bool aimingMode;
    private Transform birdQueue;

    void Awake()
    {
        Transform launchPointTrans = transform.FindChild("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;

        birdQueue = GetComponent<DroppingBirds>().birdQueue;
    }

    private void Start()
    {
        UpdateQueue();
    }

    void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        if (!aimingMode)
            launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        // Player pressed mouse while over Slingshot
        if (birds.Count == 0)
            return;

        aimingMode = true;

        if (SceneInformation.Instance.currentProjectile)
        {
            SceneInformation.Instance.currentProjectile = null;
        }

        // Instantiate a projectile
        projectile = Instantiate(birds[0]) as GameObject;
        SceneInformation.Instance.allObjects.Add(projectile);

        //Destroy the placeholder launchpoint bird
        Destroy(queueBirds[0]);
        SceneInformation.Instance.allObjects.Remove(queueBirds[0]);
        queueBirds[0] = null;

        //////////////////////////////////////////////////////////////////
        //Manipulate the projectile
        Transform tmpLineRend = Instantiate(lineRendererPrefab).transform;
        tmpLineRend.SetParent(projectile.transform);
        tmpLineRend.localPosition = Vector3.zero;

        SceneInformation.Instance.lastFireSlingshot = transform;
        //////////////////////////////////////////////////////////////////

        // Start it at launch position
        projectile.transform.position = launchPos;

        // Set it to kinematic for now
        projectile.GetComponent<Rigidbody2D>().isKinematic = true;

        //Trigger OnGrab 'event'
        projectile.GetComponent<Bird>().OnGrab();
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
            projectile.GetComponent<Bird>().OnRelease();

            Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
            
            //We adjust the mass of the bird according to its bird inheritor
            projectileBody.mass = birds[0].GetComponent<Bird>().mass;

            //We remove the bird from both the global and the local list
            SceneInformation.Instance.birds.Remove(birds[0]);
            birds.RemoveAt(0);
            //If there is a line renderer in the scene, delete it
            if (SceneInformation.Instance.currentLineRenderer)
            {
                Destroy(SceneInformation.Instance.currentLineRenderer.GetComponent<ProjectileLine>());
                Destroy(SceneInformation.Instance.currentLineRenderer);
            }

            // The mouse has been released
            aimingMode = false;
            // Fire off the projectile with given velocity
            projectileBody.isKinematic = false;
            projectileBody.velocity = -mouseDelta * velocityMult;

            SceneInformation.Instance.currentProjectile = projectileBody;
            SceneInformation.Instance.currentLineRenderer = projectile.GetComponentInChildren<LineRenderer>();

            // Set the reference to the projectile to null as early as possible
            projectile = null;

            UpdateQueue();
        }
    }

    private void UpdateQueue()
    {
        for (int i = 0; i < queueBirds.Length; i++)
        {
            SceneInformation.Instance.allObjects.Remove(queueBirds[i]);

            if (queueBirds[i] != null)
            {
                Destroy(queueBirds[i]);
            }
        }

        queueBirds = new GameObject[birds.Count];

        if (queueBirds.Length > 0)
        {
            queueBirds[0] = new GameObject("LaunchPointBird");
            queueBirds[0].transform.position = launchPoint.transform.position;
            SpriteRenderer tmpRenderer = queueBirds[0].AddComponent<SpriteRenderer>();
            tmpRenderer.sprite = birds[0].GetComponent<BirdInfo>().eyesOpen;
            tmpRenderer.sortingOrder = 1;

            //Add it to the sceneObjects so it gets deleted when the player wins
            SceneInformation.Instance.allObjects.Add(queueBirds[0]);

            //Now that we added the special position of the first bird we generically adjust the other ones if there are any
            for (int i = 1; i < birds.Count; i++)
            {
                queueBirds[i] = new GameObject("QueueBird Nr. " + (i + 1));
                SpriteRenderer tmpRend = queueBirds[i].AddComponent<SpriteRenderer>();
                tmpRend.sprite = birds[i].GetComponent<BirdInfo>().eyesOpen;
                tmpRend.sortingOrder = 1;

                queueBirds[i].transform.position = birdQueue.position + new Vector3(0, tmpRend.bounds.extents.y, 0);
                if (i > 1)
                {
                    float prevBoundsX = queueBirds[i - 1].GetComponent<SpriteRenderer>().bounds.extents.x;
                    queueBirds[i].transform.position = new Vector3((queueBirds[i - 1].transform.position.x) -prevBoundsX * 1.5f - tmpRend.bounds.extents.x, tmpRend.bounds.extents.y * 0.67f, 0);
                }

                SceneInformation.Instance.allObjects.Add(queueBirds[i]);
            }
        }
    }
}
