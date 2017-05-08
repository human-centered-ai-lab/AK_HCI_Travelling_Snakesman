using System.Collections.Generic;
using System.Linq;
using AntAlgorithm;
using AntAlgorithm.tools;
using util;
using UnityEngine;

public class AntAlgorithmManager : Singleton<AntAlgorithmManager>
{
    [SerializeField] private uint GameBoardSize = 30;
    [SerializeField] private float MaximumEnlargementFactor = 0.5f;

    protected AntAlgorithmManager() {}

    private AntAlgorithmSimple _antAlgorithm;
    private const string TspFileToUse = "berlin52.tsp";
    private GameObject[] _remainingFood;
    private List<int> _userTour;
    public GameObject linez;
    private LineRenderer lineRenderer;
    public Material material;

    public bool IsGameFinished
    {
        get { return GameObject.FindGameObjectsWithTag("Food").Length == 0; }
    }

    public void Start()
    {
        Cities = TSPImporter.ImportTsp(TspFileToUse);
        _remainingFood = new GameObject[Cities.Count];
        _antAlgorithm = transform.GetOrAddComponent<AntAlgorithmSimple>();
        _antAlgorithm.SetCities(Cities);
        _antAlgorithm.Init();
        _userTour = new List<int>();
        FoodController.InitializeFoodPositions(GameBoardSize);

        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Assigns a material named "Assets/Resources/DEV_Orange" to the object.
        Material newMat = Resources.Load("lineColor") as Material;
        lineRenderer.material = newMat;


    }

    public List<City> Cities { get; private set; }

    public void RunXIterations(int numIter)
    {
        for (var i = 0; i < numIter; ++i)
        {
            _antAlgorithm.Iteration();
        }
        _antAlgorithm.PrintBestTour("After Run");
    }

    public void RegisterFood(int id, GameObject go)
    {
        Debug.Assert(_remainingFood[id] == null);
        _remainingFood[id] = go;
        Debug.Assert(_remainingFood[id] != null);
    }

    public void UnregisterEatenFood(int id)
    {
        _userTour.Add(id);
        FoodController rf_eaten;
        rf_eaten = _remainingFood[id].GetComponent<FoodController>();
        Vector3 from = rf_eaten.getPosition();

        _remainingFood[id] = null;
        var connectedPheromones = _antAlgorithm.Pheromones.GetPheromones(id);
        var pheromoneMaximum = connectedPheromones.Max();
        Vector3 to = new Vector3(0,0,0);

        for (int idx = 0; idx < connectedPheromones.Length; idx++)
        {
            if(_remainingFood[idx] == null)
                continue;
            var scaleFactor = 1 + (float)(connectedPheromones[idx] / pheromoneMaximum) * MaximumEnlargementFactor;
            FoodController rf = _remainingFood[idx].GetComponent<FoodController>();
            rf.Rescale(scaleFactor);
            if (pheromoneMaximum == connectedPheromones[idx])
            {
                to = rf.getPosition();
            }
        }


       

        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
        //lineRenderer.material = material;
        lineRenderer.SetWidth(0.15f, 0.15f);
        UpdatePheromones();
        RunXIterations(5);
    }

    private void UpdatePheromones()
    {
        if (_userTour.Count < 2) return;
        var lastIdx = _userTour.Count - 1;
        var cityA = _userTour[lastIdx];
        var cityB = _userTour[lastIdx - 1];
        _antAlgorithm.Pheromones.IncreasePheromone(cityA, cityB, _antAlgorithm.Pheromones.GetPheromone(cityA, cityB));
    }

}