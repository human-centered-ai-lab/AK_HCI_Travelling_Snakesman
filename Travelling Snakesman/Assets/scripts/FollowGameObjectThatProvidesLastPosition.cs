using UnityEngine;

public class FollowGameObjectThatProvidesLastPosition : MonoBehaviour {
	public GameObject objectToFollow;
	
	// Update is called once per frame
	void Update ()
    {
		LastPosition ancestor = objectToFollow.GetComponent<LastPosition>();

		//dirty hack. needed because we don't know when last pos of ancestor was called the last time
		if (Vector3.Distance(objectToFollow.transform.position, ancestor.OldPosition) < Vector3.kEpsilon) {
			return;
		}
		transform.position = ancestor.OldPosition;
	}
}
