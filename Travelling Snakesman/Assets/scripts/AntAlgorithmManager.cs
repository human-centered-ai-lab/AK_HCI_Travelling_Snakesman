using System;
using System.Collections.Generic;
using System.Linq;
using AntAlgorithm.tools;
using util;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using AntAlgorithms;

public class AntAlgorithmManager : Singleton<AntAlgorithmManager>
{
    [SerializeField] private uint GameBoardSize = 25;

    protected AntAlgorithmManager()
    {
        //PlayerPrefs.DeleteAll();
        _initializationFinished = false;
        Cities = new List<City>();
        _userTour = new List<int>();
        _userTourCities = new List<City>();
    }

    private AntAlgorithmChooser _antAlgorithmChooser;
    private AntAlgorithms.AntAlgorithm _antAlgorithm;

    public const string GameName = "TravellingSnakesman";
    public const int NumHighScoreEntries = 50;

    private string TspFileToUse;
    private string TspFileName;
    private GameObject[] _remainingFood;
    private readonly List<int> _userTour;
    private readonly List<City> _userTourCities;
    private bool _initializationFinished;
    private Vector3 _nextBestFoodPosition;

    public List<City> Cities { get; private set; }

    public double BestTourLength { get { return _antAlgorithm.TourLength; } }
    public List<int> BestTour { get { return _antAlgorithm.BestTour; } }
    public int BestItertation { get { return _antAlgorithm.BestIteration; } }

    public int BestAlgorithmIteration { get; private set; }
    public double BestAlgorithmLength { get; private set; }
    public string BestAlgorithmTour { get; private set; }

    public bool IsGameFinished
    {
        get { return _initializationFinished && GameObject.FindGameObjectsWithTag("Food").Length == 0; }
    }

    private void Start()
    {
        _initializationFinished = false;
        Debug.Log(string.Format("!Start called on {0}!", GetHashCode()));
        Debug.Log("--- FIND EDITION ---");
        InitTSP();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainGameScreen")
        {
            return;
        }
        InitTSP();
    }

    public void InitTSP()
    {
        TspFileName = PlayerPrefs.GetString("TspName");
        TspFileToUse = TspFileName + ".tsp";

    #if UNITY_STANDALONE_WIN
        Debug.Log("Stand Alone Windows");
        Cities = TSPImporter.ImportTsp(TspFileToUse);
        Init();
    #endif

    #if UNITY_WEBGL || UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        TSPImporter tsp = new TSPImporter();
        Debug.Log("WebGL or Mobile");
        StartCoroutine(tsp.importTspFromWebWebGL(TspFileToUse));
        StartCoroutine(InitWebGL(tsp));
        Cities = tsp.Cities;
    #endif


    }

    private IEnumerator InitWebGL(TSPImporter tsp)
    {
        while (!tsp.loadingComplete)
            yield return new WaitForSeconds(0.1f);
        Init();
    }


    public void Update()
    {
        if (IsGameFinished)
        {
            _initializationFinished = false;
        }
    }

    public void Init()
    {
        Debug.Log(string.Format("!RUNNING INIT on {0}!", GetHashCode()));
        if (_userTour.Count != 0)
        {
            _userTour.Clear();
            Debug.Log("\tUser tour cleared.");
        }
        if (_userTourCities.Count != 0)
        {
            _userTourCities.Clear();
            Debug.Log("\tUser tour cities cleared.");
        }
        if (Cities.Count == 0)
        {
            Debug.Log("\tNo Cities loaded.");
        }

        _remainingFood = new GameObject[Cities.Count];
        _antAlgorithmChooser = new AntAlgorithmChooser(Mode.MinMaxAntSystem, 1, 5, 0.02, 51, -1, 0.05);
        _antAlgorithm = _antAlgorithmChooser.Algorithm;
        //_antAlgorithm = transform.GetOrAddComponent<AntAlgorithm>();
        _antAlgorithm.Cities = (Cities);
        _antAlgorithm.Init();
        FoodController.InitializeFoodPositions(GameBoardSize);

        _nextBestFoodPosition = new Vector3(0, 0, 0); // init
        RunXIterations(Cities.Count * 5);
        PrintBestTour("algo best tour");
        BestAlgorithmLength = BestTourLength;
        BestAlgorithmTour = TourToString(BestTour);
        BestAlgorithmIteration = BestItertation;
        _initializationFinished = true;
        _antAlgorithm.Init();
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
        _antAlgorithm.PrintBestTour(str, 1);
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
        Debug.Log(string.Format("!UnregisterEatenFood on {0}!", GetHashCode()));
        _userTour.Add(id);
        _userTourCities.Add(Cities[id]);

        _remainingFood[id] = null;
        var pheromones = _antAlgorithm.Pheromones.GetPheromones(id);
        var max = GetRemainingMaximum(pheromones);
        var min = GetRemainingMinimum(pheromones);

        //Debug.Log(string.Format("PHEROMONES - Min: {0} - Max: {1}", min, max));

        for (int i = 0; i < pheromones.Length; i++)
        {
            if(_remainingFood[i] == null)
                continue;
			_remainingFood[i].GetComponent<FoodController>().Redye(GetRedyeFactor(min, pheromones[i], max));
            
            // set nextBestFoodPosition because of maximum of pheromones
            if (Math.Abs(max - pheromones[i]) < 1e-6 * max)
            {
                _nextBestFoodPosition = _remainingFood[i].transform.position;
            }
        }
        //Debug.Log(_antAlgorithm.Pheromones.ToString);

        UpdatePheromones();
        RunXIterations(5);
    }

    private void UpdatePheromones()
    {
        if (_userTour.Count < 2) return;
        var lastIdx = _userTour.Count - 1;
        var cityA = _userTour[lastIdx];
        var cityB = _userTour[lastIdx - 1];
        _antAlgorithm.Pheromones.IncreasePheromoneAs(cityA, cityB, _antAlgorithm.Pheromones.GetPheromone(cityA, cityB));
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

            Vector2 city1Pos = new Vector2(city1.XPosition, city1.YPosition);
            Vector2 city2Pos = new Vector2(city2.XPosition, city2.YPosition);

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

            Vector2 city1Pos = new Vector2(city1.XPosition, city1.YPosition);
            Vector2 city2Pos = new Vector2(city2.XPosition, city2.YPosition);

            distance += Vector2.Distance(city1Pos, city2Pos);
        }

        return distance;
    }

    public string TourToString(List<int> tour)
    {
        string tourString = "";
        for (int i = 0; i < tour.Count; i++)
        {
            if(i == (tour.Count - 1))
                tourString += tour[i];
            else
                tourString += tour[i] + "-";
        }

        return tourString;
    }

    #endregion
}