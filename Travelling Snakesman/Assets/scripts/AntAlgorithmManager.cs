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
    }

    public List<City> Cities { get; private set; }

    public void RunXIterations(int numIter)
    {
        for (var i = 0; i < numIter; ++i)
        {
            _antAlgorithm.Iteration();
        }
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
        _remainingFood[id] = null;
        var connectedPheromones = _antAlgorithm.Pheromones.GetPheromones(id);
        var pheromoneMaximum = connectedPheromones.Max();

        for (int idx = 0; idx < connectedPheromones.Length; idx++)
        {
            if(_remainingFood[idx] == null)
                continue;
            var scaleFactor = 1 + (float)(connectedPheromones[idx] / pheromoneMaximum) * MaximumEnlargementFactor;
            _remainingFood[idx].GetComponent<FoodController>().Rescale(scaleFactor);
        }
        UpdatePheromones();
        RunXIterations(10);
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