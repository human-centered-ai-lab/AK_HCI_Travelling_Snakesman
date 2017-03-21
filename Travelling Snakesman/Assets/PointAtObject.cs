using UnityEngine;
using System.Collections;

public class PointAtObject : MonoBehaviour {

	public GameObject objectToPointAt;
	private Vector3 target;

	// Update is called once per frame
	void Update () {

		//if object to point at is already destroyed, also destroy this object
		if (objectToPointAt == null) {
			Destroy (gameObject);
		}
			
		target = objectToPointAt.transform.position;

		//show arrow only if object to point at is outside of camera view
		Vector3 worldToViewpointPort = Camera.main.WorldToViewportPoint(target);

		//Debug.Log("worldToViewpointPort: " + worldToViewpointPort);

		if(worldToViewpointPort.x > 0 && worldToViewpointPort.x < 1 &&
		   worldToViewpointPort.y > 0 && worldToViewpointPort.y < 1) {
			GetComponent<Renderer>().enabled = false;	
			return;
		}

		GetComponent<Renderer>().enabled = true;
	
		float angle = Mathf.Atan2(target.y - Camera.main.transform.position.y, target.x - Camera.main.transform.position.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}