using UnityEngine;

public class FollowMouse : MonoBehaviour
{
	public float Speed = 1.5f;
	private Vector3 _target;

	void Start()
    {
		_target = transform.position;
	}

	void Update()
    {
		//don't rotate main camera
		Camera.main.transform.rotation = Quaternion.identity;


        if (AntAlgorithmManager.Instance.IsGameFinished)
        {
            Speed = 0;
            return;
        }
    }

    void FixedUpdate()
    {
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _target.z = transform.position.z;

        //follow mouse
        transform.position = Vector3.MoveTowards(transform.position, _target, Speed * Time.deltaTime);

        float angle = Mathf.Atan2(_target.y - Camera.main.transform.position.y, _target.x - Camera.main.transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}