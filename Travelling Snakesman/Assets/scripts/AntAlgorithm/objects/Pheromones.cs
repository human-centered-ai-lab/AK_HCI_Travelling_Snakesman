/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* Pheromone represents the pheromones between cities */

using System;
using util;

public class Pheromones
{
    // initialization factor for pheromones
    private double initPheromoneValue;
    // Matrix of pheromones between city x and city y
    private double[,] _pheromones;
    private int numOfCities;
    // for mmas
    private double trailMin;
    private double trailMax;

    public Pheromones(int numOfCities, double initPheromoneValue)
    {
        this.numOfCities = numOfCities;
        this.initPheromoneValue = initPheromoneValue;
    }

    public Pheromones(int numOfCities, double initPheromoneValue, double pBest)
    {
        this.numOfCities = numOfCities;
        this.initPheromoneValue = initPheromoneValue;

        trailMax = initPheromoneValue;
        trailMin = (trailMax * (1.0 - Math.Pow(pBest, numOfCities))) / (((numOfCities / 2) - 1.0) * Math.Pow(pBest, numOfCities));
    }

    // init of pheromones
    public void Init()
    {
        _pheromones = new double[numOfCities, numOfCities];
        for (int i = 0; i < _pheromones.GetLength(0); i++)
        {
            for (int j = 0; j < _pheromones.GetLength(1); j++)
            {
                _pheromones[i, j] = initPheromoneValue;
            }
        }
    }

    public new string ToString
    {
        get
        {
            string str = "";
            for (int i = 0; i < _pheromones.GetLength(0); i++)
            {
                str += "\n";
                for (int j = 0; j < _pheromones.GetLength(1); j++)
                {
                    str += _pheromones[i, j] + " ";
                }
            }
            return str;
        }
    }

    public void CheckPheromoneTrailLimits()
    {
        for (int i = 0; i < numOfCities; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (_pheromones[i, j] < trailMin)
                {
                    _pheromones[i, j] = trailMin;
                    _pheromones[j, i] = trailMin;
                }
                else if (_pheromones[i, j] > trailMax)
                {
                    _pheromones[i, j] = trailMax;
                    _pheromones[j, i] = trailMax;
                }
            }
        }
    }

    public void UpdateTrailLimits(double optimalLength, double rho, double pBest)
    {
        trailMax = 1.0/ (rho * optimalLength);
        trailMin = (trailMax * (1.0 - Math.Pow(pBest, 1.0 / numOfCities))) / (((numOfCities / 2) - 1.0) * Math.Pow(pBest, 1.0 / numOfCities));
    }
    // decrease the pheromone value between 2 particular cities by one ant 
    public void DecreasePheromoneAs(int cityAId, int cityBId, double decreaseValue)
    {
        _pheromones[cityAId, cityBId] = decreaseValue * _pheromones[cityAId, cityBId];
    }

    // decrease the pheromone value between 2 particular cities by one ant 
    public void IncreasePheromoneAs(int cityAId, int cityBId, double increaseValue)
    {
        _pheromones[cityAId, cityBId] = _pheromones[cityAId, cityBId] + increaseValue;
    }

    public void SetPheromone(int cityAId, int cityBId, double value)
    {
        _pheromones[cityAId, cityBId] = value;
    }

    public double GetPheromone(int cityAId, int cityBId)
    {
        return _pheromones[cityAId, cityBId];
    }

    public double[] GetPheromones(int cityId)
    {
        return _pheromones.GetRow(cityId);
    }
}
