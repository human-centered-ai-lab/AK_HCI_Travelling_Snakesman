using AntAlgorithm.tools;
using gui;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FollowMouse : MonoBehaviour
{
    private bool showLine = false;

    public float Speed = 1.5f;
    private float tmpTime;

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
            if (!written)
            {
                written = true;

                //double userDistance = AntAlgorithmManager.Instance.CalcOverallUserDistance();
                double bestAlgorithmDistance = AntAlgorithmManager.Instance.BestAlgorithmLength;
                int bestAlgorithIteration = AntAlgorithmManager.Instance.BestAlgorithmIteration;
                string bestAlgoritmTour = AntAlgorithmManager.Instance.BestAlgorithmTour;

                double bestUserDistance = AntAlgorithmManager.Instance.BestTourLength;
                int bestUserIteration = AntAlgorithmManager.Instance.BestItertation;
                string bestUserTour = AntAlgorithmManager.Instance.TourToString(AntAlgorithmManager.Instance.BestTour);

                //Debug.Log("[user distance: " + userDistance);
                //AntAlgorithmManager.Instance.PrintBestTour("user best tour: ");
                GameObject time = GameObject.Find("Timer");
                int timeInSeconds = StringOperations.GetTimeFromString(time.GetComponent<Text>().text, false);

                GameObject score = GameObject.Find("ScoreValueText");
                score.GetComponent<Text>().text = timeInSeconds + "";

                GameObject gameEndedCanvas = GameObject.Find("GameEndedCanvas");
                gameEndedCanvas.GetComponent<Canvas>().enabled = true;

                GameObject gameCanvas = GameObject.Find("Canvas");
                gameCanvas.GetComponent<Canvas>().enabled = false;

                StartCoroutine(HighScoreHandler.PostScoresAsync(PlayerPrefs.GetString("PlayerName"), PlayerPrefs.GetString("TspName"), bestAlgorithmDistance, bestAlgorithIteration, bestAlgoritmTour, bestUserDistance, bestUserIteration, bestUserTour, timeInSeconds));
            }

            if (showLine)
            {
                lineRenderer.SetPosition(1, transform.position); // snakehead position
            }
            return;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            GameObject timer = GameObject.Find("Timer");
            tmpTime = timer.GetComponent<TimerDisplayController>().Time;

            Speed = 0;
            GameObject gameEndedCanvas = GameObject.Find("QuitGameCanvas");
            gameEndedCanvas.GetComponent<Canvas>().enabled = true;

            GameObject gameCanvas = GameObject.Find("Canvas");
            gameCanvas.GetComponent<Canvas>().enabled = false;

            GameObject resumeGameButton = GameObject.Find("ResumeGameButton");
            resumeGameButton.GetComponent<Button>().onClick.AddListener(resumeSpeed);


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

    private void resumeSpeed()
    {
        Speed = 3f;

        GameObject timer = GameObject.Find("Timer");
        timer.GetComponent<TimerDisplayController>().Time = tmpTime;

        GameObject gameCanvas = GameObject.Find("Canvas");
        gameCanvas.GetComponent<Canvas>().enabled = true;

        GameObject gameEndedCanvas = GameObject.Find("QuitGameCanvas");
        gameEndedCanvas.GetComponent<Canvas>().enabled = false;
    }
}