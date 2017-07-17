using AntAlgorithm.tools;
using UnityEngine;
using UnityEngine.UI;

public class FollowMouse : MonoBehaviour
{
    private bool showLine = false; 

	public float Speed = 1.5f;
	private Vector3 _target;

    private LineRenderer lineRenderer;
    public Material material;
    bool written;

    void Start()
    {
		_target = transform.position;

        lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        //lineRenderer.material.color = UnityEngine.Color.white;

        // Assigns a material named "Assets/Resources/DEV_Orange" to the object.
        //Material newMat = Resources.Load("Materials/lineColor") as Material;
        //Debug.Log(newMat);
        //lineRenderer.material = newMat;
    }

    void Update()
    {
		//don't rotate main camera
		Camera.main.transform.rotation = Quaternion.identity;

        if (AntAlgorithmManager.Instance.IsGameFinished)
        {
            Speed = 0;
            if(!written)
            {
                written = true;

                double userDistance = AntAlgorithmManager.Instance.CalcOverallUserDistance();
				Debug.Log("[user distance: " + userDistance);
                AntAlgorithmManager.Instance.PrintBestTour("user best tour: ");

				GameObject bestTourUserText = GameObject.Find ("distance_text_1");
                bestTourUserText.GetComponent<Text> ().text = ((int)AntAlgorithmManager.Instance.BestTourLength).ToString();

                GameObject bestTourText = GameObject.Find("distance_text_2");
                bestTourText.GetComponent<Text>().text = ((int)AntAlgorithmManager.Instance.BestAlgorithmLength).ToString();

                float improved = (((float)AntAlgorithmManager.Instance.BestAlgorithmLength) 
                    - ((float)AntAlgorithmManager.Instance.BestTourLength)) / ((float)AntAlgorithmManager.Instance.BestAlgorithmLength) * 100;
                Debug.Log("improved: " + improved);

                GameObject improvedText = GameObject.Find("distance_text_3");
                improvedText.GetComponent<Text>().text = improved.ToString("0.00") + " %";

                GameObject gameEndedCanvas = GameObject.Find ("GameEndedCanvas");
				gameEndedCanvas.GetComponent<Canvas> ().enabled = true;

				GameObject gameCanvas = GameObject.Find ("Canvas");
				gameCanvas.GetComponent<Canvas> ().enabled = false;

                StartCoroutine(HighScoreHandler.PostScoresAsync(PlayerPrefs.GetString ("PlayerName"), (int) userDistance, "best tour user: " + (int)AntAlgorithmManager.Instance.BestTourLength + " # best tour algo: " + (int)AntAlgorithmManager.Instance.BestAlgorithmLength, PlayerPrefs.GetString("TspName")));
            }

            if (showLine)
            {
                lineRenderer.SetPosition(1, transform.position); // snakehead position
            }
            return;
        }

        if (showLine)
        {
            // set Positions of line
            lineRenderer.SetPosition(0, transform.position); // snakehead position
            lineRenderer.SetPosition(1, AntAlgorithmManager.Instance.GetNextPosition()); // "optimal" next position
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