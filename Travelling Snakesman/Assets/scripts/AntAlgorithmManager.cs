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
    private GameObject[] remainingFood;

    public bool IsGameFinished
    {
        get { return GameObject.FindGameObjectsWithTag("Food").Length == 0; }
    }

    public void Start()
    {
        Cities = TSPImporter.ImportTsp(TspFileToUse);
        remainingFood = new GameObject[Cities.Count];
        _antAlgorithm = transform.GetOrAddComponent<AntAlgorithmSimple>();
        _antAlgorithm.SetCities(Cities);
        _antAlgorithm.Init();

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
        Debug.Assert(remainingFood[id] == null);
        remainingFood[id] = go;
        Debug.Assert(remainingFood[id] != null);
    }

    public void UnregisterEatenFood(int id)
    {
        remainingFood[id] = null;
        var connectedPheromones = _antAlgorithm.Pheromones.GetPheromones(id);
        var pheromoneMaximum = connectedPheromones.Max();

        for (int idx = 0; idx < connectedPheromones.Length; idx++)
        {
            if(remainingFood[idx] == null)
                continue;

            var scaleFactor = 1 + (float)(connectedPheromones[idx] / pheromoneMaximum) * MaximumEnlargementFactor;
            remainingFood[idx].GetComponent<FoodController>().Rescale(scaleFactor);
        }
        RunXIterations(10);
    }
}