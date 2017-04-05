using UnityEngine;
using System.Collections;

public class FollowGameObjectThatProvidesLastPosition : MonoBehaviour {

	public GameObject objectToFollow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = 

		LastPosition ancestor = (LastPosition)objectToFollow.GetComponent (typeof(LastPosition));

		//dirty hack. needed because we don't know when last pos of ancestor was called the last time
		if (Vector3.Distance(objectToFollow.transform.position, ancestor.oldPosition) < Vector3.kEpsilon) {
			return;
		}
			
		transform.position = ancestor.oldPosition;
	}
}
