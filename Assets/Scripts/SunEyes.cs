using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SunEyes : MonoBehaviour
{
    public float range = 1f;
    [Space(15)]
    public Sprite flinchedEye;
    public Sprite openedEye;
    public Image eye;
    [Space(15)]
    public bool flipEye;

    private Camera mainCam;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
        mainCam = Camera.main;
    }

    private void FixedUpdate()
    {
        Vector3 direction = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        direction.z = 0;

        transform.position = originalPosition + direction.normalized;
    }
}
