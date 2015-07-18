using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

    public Vector3 rotationVector;

    private void Update()
    {
        transform.Rotate(rotationVector * Time.deltaTime);
    }
}
