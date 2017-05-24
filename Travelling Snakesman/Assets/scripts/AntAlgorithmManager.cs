using System.Collections.Generic;
using System.Linq;
using AntAlgorithm;
using AntAlgorithm.tools;
using util;
using UnityEngine;

public class AntAlgorithmManager : Singleton<AntAlgorithmManager>
{
    public enum ScalingMethod
    {
        Max,
        MinMax
    }

    [SerializeField] private uint GameBoardSize = 30;
    [SerializeField] private float MaximumEnlargementFactor = 0.5f;
    [SerializeField] private ScalingMethod Scaling;

    protected AntAlgorithmManager() {}

    private AntAlgorithmSimple _antAlgorithm;
    private const string TspFileToUse = "berlin52.tsp";
    private GameObject[] _remainingFood;
    private List<int> _userTour;

    private Vector3 _nextBestFoodPosition;

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

        _nextBestFoodPosition = new Vector3(0, 0, 0); // init
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

    public Vector3 getNextPosition()
    {
        return _nextBestFoodPosition;
    }

    public void UnregisterEatenFood(int id)
    {
        _userTour.Add(id);
        Vector3 from = _remainingFood[id].transform.position;

        _remainingFood[id] = null;
        var pheromones = _antAlgorithm.Pheromones.GetPheromones(id);
        var max = GetRemainingMaximum(pheromones);
        var min = GetRemainingMinimum(pheromones);

        //Debug.Log(string.Format("PHEROMONES - Min: {0} - Max: {1}", min, max));

        for (int idx = 0; idx < pheromones.Length; idx++)
        {
            if(_remainingFood[idx] == null)
                continue;
            //_remainingFood[idx].GetComponent<FoodController>().Rescale(GetScalingFactor(min, pheromones[idx], max));
			_remainingFood[idx].GetComponent<FoodController>().Redye(GetRedyeFactor(min, pheromones[idx], max));
            
            // set nextBestFoodPosition because of maximum of pheromones
            if (max == pheromones[idx])
            {
                _nextBestFoodPosition = _remainingFood[idx].transform.position;
            }
        }

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

    #region Helper Methods
    private float GetScalingFactor(double min, double value, double max)
    {
        switch (Scaling)
        {
            case ScalingMethod.Max:
                return 1 + (float)(value / max) * MaximumEnlargementFactor;
            case ScalingMethod.MinMax:
                return 1 + (float)((max - value) / (max - min)) * MaximumEnlargementFactor;
            default:
                return 1;
        }
    }

	//returns value between 0 (for min value) and 1 (for max value)
	private float GetRedyeFactor(double min, double value, double max)
	{
		return (float)( (value - min) / (max - min) );
	}

    private double GetRemainingMaximum(double[] arr)
    {
        var tmp = (double[])arr.Clone();

        foreach (var visitedCityIdx in _userTour)
        {
            tmp[visitedCityIdx] = double.MinValue;
        }
        return tmp.Max();
    }

    private double GetRemainingMinimum(double[] arr)
    {
        var tmp = (double[])arr.Clone();

        foreach (var visitedCityIdx in _userTour)
        {
            tmp[visitedCityIdx] = double.MaxValue;
        }
        return tmp.Min();
    }

    #endregion
}