using UnityEngine;
using System.Collections;

// This is needed for using Lists
using System.Collections.Generic;

public class ProjectileLine : MonoBehaviour {
	// Fields in Inspector pane
	public float minDist = 0.1f;

	// Dynamic fields
	private LineRenderer line;
	private GameObject _poi;
	private Vector3 lastPoint;
	private int pointsCount;

	void Awake() {
		// Get a reference to the LineRenderer
		line = GetComponent<LineRenderer>();
		line.material = new Material(Shader.Find("Mobile/Particles/Additive"));
		line.SetColors(Color.white, Color.white);
        line.SetWidth(0.2f, 0.2f);
		pointsCount = 0;
		// Disable until its needed
		line.enabled = false;

        Clear();
	}

	void FixedUpdate() 
    {
		if (!SceneInformation.Instance.currentProjectile) 
        {
            return;
		}

		AddPoint();
        if (SceneInformation.Instance.currentProjectile.IsSleeping())
        {
            SceneInformation.Instance.currentProjectile = null;
		}
	}

	public void AddPoint(){
        Vector3 pt = SceneInformation.Instance.currentProjectile.transform.position;
		// If the point isnt far enough from the last one, do nothing
		if(pointsCount > 0 && (pt - lastPoint).magnitude < minDist) {
			return;
		}

		if(pointsCount == 0){
			// If its the launch point (first)
			line.SetVertexCount(1);
			line.SetPosition(0, pt);
			//line.SetPosition(1, pt);
			pointsCount += 1;
			line.enabled = true;
		} else {
			// Not the first point
			pointsCount++;
			line.SetVertexCount(pointsCount);
			line.SetPosition(pointsCount - 1, pt);
		}

		lastPoint = pt;
	}

	public void Clear(){
		pointsCount = 0;
		line.SetVertexCount(0);
	}
}
