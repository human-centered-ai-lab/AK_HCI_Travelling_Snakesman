using UnityEngine;
using System.Collections;

public class LastPosition : MonoBehaviour {

	public Vector3 oldPosition;

	// Use this for initialization
	void Start () {
		InvokeRepeating("setOldPosition", 0.0f, 0.5f);
	}
	
	void setOldPosition() {
		oldPosition = transform.position;
	}
}
