using UnityEngine;
using System.Collections;

public abstract class Bird : MonoBehaviour 
{
    public float mass;
    public Rigidbody2D rBody;

    public abstract void OnGrab();
    public abstract void OnRelease();
    public abstract void OnTap();
}
