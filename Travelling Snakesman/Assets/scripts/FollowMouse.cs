using UnityEngine;

public class FollowMouse : MonoBehaviour
{
	public float speed = 1.5f;
	private Vector3 target;

	void Start ()
    {
		target = transform.position;
	}

	void Update ()
    {
		//don't rotate main camera
		Camera.main.transform.rotation = Quaternion.identity;


        if (AntAlgorithmManager.Instance.IsGameFinished)
        {
            speed = 0;
            return;
        }
    }

    void FixedUpdate()
    {
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = transform.position.z;

        //follow mouse
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        float angle = Mathf.Atan2(target.y - Camera.main.transform.position.y, target.x - Camera.main.transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}