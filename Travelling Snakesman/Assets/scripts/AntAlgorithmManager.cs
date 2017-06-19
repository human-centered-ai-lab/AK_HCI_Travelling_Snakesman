using System;
using System.Collections.Generic;
using System.Linq;
using AntAlgorithm;
using AntAlgorithm.tools;
using util;
using UnityEngine;

public class AntAlgorithmManager : Singleton<AntAlgorithmManager>
{
    [SerializeField] private uint GameBoardSize = 30;

    protected AntAlgorithmManager() {
	}

    private AntAlgorithmSimple _antAlgorithm;

    public const string GameName = "TravellingSnakesman";
    public const string TspFileName = "berlin52";
    public const int NumHighScoreEntries = 5;

    private const string TspFileToUse = TspFileName + ".tsp";
    private GameObject[] _remainingFood;
    private List<int> _userTour;
    private List<City> _userTourCities;
    private Vector3 _nextBestFoodPosition;

    public List<City> Cities { get; private set; }

    public double BestTourLength { get { return _antAlgorithm.BestTourLength; } }

    public double BestAlgorithmLength { get; private set; }

    public bool IsGameFinished
    {
        get { return GameObject.FindGameObjectsWithTag("Food").Length == 0; }
    }

    public void Start()
    {
        Debug.Log("!Start called!");
        Debug.Log("--- FIND EDITION ---");
        
    #if UNITY_STANDALONE_WIN
        Debug.Log("Stand Alone Windows");
        Cities = TSPImporter.ImportTsp(TspFileToUse);
    #endif
        
    #if UNITY_WEBGL
        Debug.Log("WebGL");
        TSPImporter tsp = new TSPImporter();
        tsp.ImportFromWeb(TspFileToUse);
        Cities = tsp.Cities;
    #endif
    Init();
    }

    public void Awake()
	{
	    DontDestroyOnLoad(transform.gameObject);
        Debug.Log ("!Awake called!");
	}	

    public void Init()
    {
        Debug.Log("!RUNNING INIT!");
        Debug.Log("Number of Cities: " + Cities.Count);
        if (_userTour != null)
        {
            _userTour.Clear();
            Debug.Log("\tUser tour cleared.");
        }
        if (_userTourCities != null)
        {
            _userTourCities.Clear();
            Debug.Log("\tUser tour cities cleared.");
        }

        _remainingFood = new GameObject[Cities.Count];
        _antAlgorithm = transform.GetOrAddComponent<AntAlgorithmSimple>();
        _antAlgorithm.SetCities(Cities);
        _antAlgorithm.Init();
        _userTour = new List<int>();
        _userTourCities = new List<City>();
        FoodController.InitializeFoodPositions(GameBoardSize);

        _nextBestFoodPosition = new Vector3(0, 0, 0); // init
        RunXIterations(52*5);
        PrintBestTour("algo best tour: ");
        BestAlgorithmLength = BestTourLength;
    }

    public void RunXIterations(int numIter)
    {
        for (var i = 0; i < numIter; ++i)
        {
            _antAlgorithm.Iteration();
        }
        //_antAlgorithm.PrintBestTour("After Run");
    }

    public void PrintBestTour(string str)
    {
        _antAlgorithm.PrintBestTour(str);
    }

    public void RegisterFood(int id, GameObject go)
    {
        Debug.Assert(_remainingFood[id] == null);
        _remainingFood[id] = go;
        Debug.Assert(_remainingFood[id] != null);
    }

    public Vector3 GetNextPosition()
    {
        return _nextBestFoodPosition;
    }

    public void UnregisterEatenFood(int id)
    {
        _userTour.Add(id);
        _userTourCities.Add(Cities[id]);

        _remainingFood[id] = null;
        var pheromones = _antAlgorithm.Pheromones.GetPheromones(id);
        var max = GetRemainingMaximum(pheromones);
        var min = GetRemainingMinimum(pheromones);

        //Debug.Log(string.Format("PHEROMONES - Min: {0} - Max: {1}", min, max));

        for (int idx = 0; idx < pheromones.Length; idx++)
        {
            if(_remainingFood[idx] == null)
                continue;
			_remainingFood[idx].GetComponent<FoodController>().Redye(GetRedyeFactor(min, pheromones[idx], max));
            
            // set nextBestFoodPosition because of maximum of pheromones
            if (Math.Abs(max - pheromones[idx]) < 1e-6 * max)
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
		//_antAlgorithm.Pheromones.SetPheromone(cityA, cityB, 0.7);
    }

    #region Helper Methods

    //returns value between 0 (for min value) and 1 (for max value)
	private float GetRedyeFactor(double min, double value, double max)
	{
		double divisor = max - min;
		if (Math.Abs(divisor) < 1e-6 * min) 
        {
			divisor = 0.001;
		}
		return (float)( (value - min) / divisor );
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

    public double CalcOverallUserDistance()
    {
		Debug.Log ("CalcOverallUserDistance. number of cities: " + _userTourCities.Count);

        double distance = 0;
        
        for(int i = 0; i < _userTourCities.Count - 1; i++)
        {
            City city1 = _userTourCities[i];
            City city2 = _userTourCities[i + 1];

            Vector2 city1Pos = new Vector2(city1.getXPosition(), city1.getYPosition());
            Vector2 city2Pos = new Vector2(city2.getXPosition(), city2.getYPosition());

            distance += Vector2.Distance(city1Pos, city2Pos);
        }

        return distance;
    }

    public double CalcOverallDistance(List<City> cities)
    {
        double distance = 0;
        
		Debug.Log("CalcOverallDistance. #cities: " + _userTourCities.Count);

		for(int i = 0; i < cities.Count - 1; i++)
        {
            City city1 = cities[i];
            City city2 = cities[i + 1];

            Vector2 city1Pos = new Vector2(city1.getXPosition(), city1.getYPosition());
            Vector2 city2Pos = new Vector2(city2.getXPosition(), city2.getYPosition());

            distance += Vector2.Distance(city1Pos, city2Pos);
        }

        return distance;
    }

    #endregion
}