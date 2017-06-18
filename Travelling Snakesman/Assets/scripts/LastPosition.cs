using UnityEngine;

public class LastPosition : MonoBehaviour
{
    public Vector3 OldPosition { get; set; }

    // Use this for initialization
	void Start ()
    {
		InvokeRepeating("SetOldPosition", 0.0f, 0.35f);
	}
	
	void SetOldPosition()
    {
		OldPosition = transform.position;
	}
}
