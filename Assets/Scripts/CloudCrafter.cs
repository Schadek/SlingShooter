using UnityEngine;
using System.Collections;

public class CloudCrafter : MonoBehaviour {

	public int numClouds = 40;
	public GameObject[] cloudPrefabs;	
	public Vector3 cloudPosMin;			
	public Vector3 cloudPosMax;			
	public float cloudScaleMin = 1.0f;	
	public float cloudScaleMax = 5.0f;	
	public float cloudSpeedMult = 0.5f; 

	private GameObject[] cloudInstances;

	void Awake() {
		cloudInstances = new GameObject[numClouds];

		GameObject cloud;
		for (int i=0; i < cloudInstances.Length; i++) 
        {
			int prefabNum = Random.Range(0, cloudPrefabs.Length);
			cloud = Instantiate(cloudPrefabs[prefabNum]) as GameObject;
			Vector3 cPos = Vector3.zero;
			cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
			cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
			float scaleFactor = Random.value;
			float scaleValue = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleFactor);
			cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleFactor);
			cPos.z = 100 - 90 * scaleFactor;

			cloud.transform.position = cPos;
			cloud.transform.localScale = Vector3.one * scaleValue;
            cloud.transform.parent = transform;
			cloudInstances[i] = cloud;
		}
	}

	void Update() {
		foreach(GameObject cloud in cloudInstances) {
			Vector3 cPos = cloud.transform.position;
			float scaleValue = cloud.transform.localScale.x;

			cPos.x -= scaleValue * Time.deltaTime * cloudSpeedMult;

			if(cPos.x < cloudPosMin.x){
				cPos.x = cloudPosMax.x;
			}
			cloud.transform.position = cPos;
		}
	
	}

}
