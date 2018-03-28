using AntAlgorithm.tools;
using gui;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FollowMouse : MonoBehaviour
{
    private bool showLine = false;
    private bool timeSet = false;

    public float Speed = 1.5f;
    private float tmpTime;

    private Vector3 _target;
    private bool escPressed = false;

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
        GameObject timer = GameObject.Find("Timer");
        float timeFloat = timer.GetComponent<TimerDisplayController>().Time;
        GameObject countdown = GameObject.Find("Countdown");
        if (timeFloat < 1.0f && !timeSet)
        {
            countdown.GetComponent<Text>().text = 3 + "";
            return;
        }
        else if (timeFloat < 2.0f && !timeSet)
        {
            countdown.GetComponent<Text>().text = 2 + "";
            return;
        }
        else if (timeFloat < 3.0f && !timeSet)
        {
            countdown.GetComponent<Text>().text = 1 + "";
            return;
        }
        else if (timeFloat > 3.0f && !timeSet)
        {
            timer.GetComponent<TimerDisplayController>().Time = 0;
            timeSet = true;
            return;
        }
        countdown.GetComponent<Text>().text = "";

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
                int timeInMillis = StringOperations.GetTimeFromString(time.GetComponent<Text>().text);

                GameObject score = GameObject.Find("ScoreValueText");
                score.GetComponent<Text>().text = StringOperations.GetStringFromTime(timeInMillis);

                GameObject gameEndedCanvas = GameObject.Find("GameEndedCanvas");
                gameEndedCanvas.GetComponent<Canvas>().enabled = true;

                GameObject gameCanvas = GameObject.Find("Canvas");
                gameCanvas.GetComponent<Canvas>().enabled = false;

                StartCoroutine(HighScoreHandler.PostScoresAsync(PlayerPrefs.GetString("PlayerName"), PlayerPrefs.GetString("TspName"), bestAlgorithmDistance, bestAlgorithIteration, bestAlgoritmTour, bestUserDistance, bestUserIteration, bestUserTour, timeInMillis));
            }

            if (showLine)
            {
                lineRenderer.SetPosition(1, transform.position); // snakehead position
            }
            return;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (escPressed)
            {
                ResumeSpeed();
                escPressed = false;

            }
            else
            {

                escPressed = true;
                tmpTime = timer.GetComponent<TimerDisplayController>().Time;

                Speed = 0;
                GameObject gameEndedCanvas = GameObject.Find("QuitGameCanvas");
                gameEndedCanvas.GetComponent<Canvas>().enabled = true;

                GameObject gameCanvas = GameObject.Find("Canvas");
                gameCanvas.GetComponent<Canvas>().enabled = false;

                GameObject resumeGameButton = GameObject.Find("ResumeGameButton");
                resumeGameButton.GetComponent<Button>().onClick.AddListener(ResumeSpeed);
            }
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

    private void ResumeSpeed()
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