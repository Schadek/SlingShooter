using UnityEngine;
using System.Collections;

public class StringUp : MonoBehaviour 
{
    Transform tr;
    LineRenderer lineRen;

    void Start()
    {
        tr = transform;
        lineRen = GetComponent<LineRenderer>();
        lineRen.SetVertexCount(2);
        lineRen.SetColors(Color.white, Color.white);
        lineRen.SetWidth(0.5f, 0.5f);
    }
    void Update()
    {
        lineRen.SetPosition(0, tr.position);
        lineRen.SetPosition(1, tr.position + Vector3.up * 50);
    }
}
