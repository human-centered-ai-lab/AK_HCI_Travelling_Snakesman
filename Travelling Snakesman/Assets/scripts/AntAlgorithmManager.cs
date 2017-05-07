using System.Collections.Generic;
using AntAlgorithm;
using util;

public class AntAlgorithmManager : Singleton<AntAlgorithmManager>
{
    protected AntAlgorithmManager()
    {
    }

    private AntAlgorithmSimple _antAlgorithm;
    private const string TspFileToUse = "berlin52.tsp";

    public void Start() {
        Cities = TSPImporter.ImportTsp(TspFileToUse);
        _antAlgorithm = transform.GetOrAddComponent<AntAlgorithmSimple>();
        _antAlgorithm.SetCities(Cities);
        _antAlgorithm.Init();
    }

    public List<City> Cities { get; private set; }

    public void RunXIterations(int numIter)
    {
        for (int i = 0; i < numIter; ++i)
        {
            _antAlgorithm.Iteration();
        }
    }
}