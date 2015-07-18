using UnityEngine;
using System.Collections;

public class StarOrbitor : MonoBehaviour 
{
    public Transform pivot;
    public Vector3 rotator = new Vector3(0, 0, 1f);
    public float speed = 0.01f;

    private Vector3 pivotXY;

    private void Start()
    {
        pivotXY = pivot.position;
        pivotXY.z = 0f;
    }

    private void FixedUpdate()
    {
        transform.RotateAround(pivotXY, rotator, speed);
    }
}
