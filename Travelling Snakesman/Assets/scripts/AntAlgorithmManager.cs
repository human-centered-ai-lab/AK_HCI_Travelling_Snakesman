using System.Collections.Generic;
using AntAlgorithm;
using util;
using UnityEngine;

public class AntAlgorithmManager : Singleton<AntAlgorithmManager>
{
    [SerializeField]
    private uint GameBoardSize = 30;

    protected AntAlgorithmManager() {}

    private AntAlgorithmSimple _antAlgorithm;
    private const string TspFileToUse = "berlin52.tsp";

    public void Start()
    {
        Cities = TSPImporter.ImportTsp(TspFileToUse);
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

    public void RegisterFood(int id)
    {

    }
}