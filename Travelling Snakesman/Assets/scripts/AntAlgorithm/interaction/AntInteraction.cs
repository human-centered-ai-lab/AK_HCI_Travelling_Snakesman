/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* AntInteraction represents the interaction with Ants */

using System;
using System.Collections.Generic;

public abstract class AntInteraction
{
    protected static int noValidNextCity = -1;

    protected int alpha;
    protected int beta;
    protected double rho;
    protected double pheromoneTrailInitialValue;

    protected int numOfAnts;
    protected List<City> cities;

    protected ChoiceInfo choiceInfo;

    //helper
    protected double[] selectionProbability;
    protected double sumProbabilities;
    protected int bestProbIndex;
    protected double[] cumulativeProbs;
    protected double[] probs;
    protected double bestGlobalTourLength;
    protected int pheromoneIncreaseFactorParameter = 100;

    //helper flags
    protected bool tourComplete = false;

    protected Random random = new Random();

    public AntInteraction(int alpha, int beta, double rho, int numOfAnts, List<City> cities)
    {
        this.cities = cities;
        this.alpha = alpha;
        this.beta = beta;
        this.rho = rho;
        this.numOfAnts = numOfAnts;

        //calculates distances
        Distances = new Distances(cities);

        // init ants with random start point
        InitAnts(true);

        selectionProbability = new double[cities.Count];
        cumulativeProbs = new double[selectionProbability.Length + 1];
        probs = new double[cities.Count];
    }

    //update the tours of ants by considering the pheromones
    public abstract void UpdateAnts();

    // updates an ant stepwise every city
    public abstract bool UpdateAntsStepwise(int citiesSoFar);

    public abstract void UpdatePheromones();

    //calculation of the probabilities 
    protected void CalculateProbs(int currCityIndex, int antIndex)
    {
        sumProbabilities = 0.0;

        int currentCity = Ants[antIndex].GetCityOfTour(currCityIndex - 1);

        for (int i = 0; i < selectionProbability.Length; i++)
        {
            if (Ants[antIndex].IsCityVisited(i))
            {
                selectionProbability[i] = 0.0;
            }
            else
            {
                selectionProbability[i] = choiceInfo.GetChoice(currentCity, i);
                sumProbabilities += selectionProbability[i];
            }
        }

        double probTemp = 0.0;

        bestProbIndex = -1;

        for (int i = 0; i < probs.Length; i++)
        {
            probs[i] = selectionProbability[i] / sumProbabilities;

            if (probs[i] > probTemp)
            {
                bestProbIndex = i;
                probTemp = probs[i];
            }
        }
    }

    // explore new edges by adding randomness
    protected int ExplorationDecision()
    {
        // calculate the cumulative probabilities
        for (int i = 0; i < selectionProbability.Length; i++)
        {
            cumulativeProbs[i + 1] = cumulativeProbs[i] + probs[i];
        }
        cumulativeProbs[cumulativeProbs.Length - 1] = 1.0f;
        double p = random.NextDouble();

        for (int i = 0; i < cumulativeProbs.Length - 1; i++)
        {
            if (p >= cumulativeProbs[i] && p <= cumulativeProbs[i + 1])
            {
                return cities[i].Id;
            }
        }
        return noValidNextCity;
    }

    //updates the pheromones for MMAS
    public void GlobalPheromoneUpdateMinMax()
    {
        int bestAntIndex = FindBestAnt().Id;

        EvaporatePheromones();

        double increaseFactor = (1.0 / Ants[bestAntIndex].TourLength) * rho;

        DepositPheromones(bestAntIndex, increaseFactor);

        FinishIteration();
    }

    // finds the ant with the best tour
    public Ant FindBestAnt()
    {
        double minTourLength = Ants[0].TourLength;
        int minTourLengthIndex = 0;

        for (int i = 1; i < Ants.Count; i++)
        {
            if (Ants[i].TourLength < minTourLength)
            {
                minTourLength = Ants[i].TourLength;
                minTourLengthIndex = i;
            }
        }

        return Ants[minTourLengthIndex];
    }

    //the init step of the ant update
    protected void InitAntUpdate()
    {
        bool[] placed = new bool[cities.Count];

        foreach (Ant ant in Ants)
        {
            ant.ClearTour();
            ant.UnvisitAllCities();
        }

        foreach (Ant ant in Ants)
        {
            int r = random.Next(0, cities.Count);
            while (placed[r] && !(numOfAnts > cities.Count))
                r = random.Next(0, cities.Count);
            placed[r] = true;
            ant.AddCityToTour(cities[r].Id);
            ant.SetCityVisited(cities[r].Id);
        }
    }

    // simply adds the first city to the end of a tour
    protected void CompleteTours()
    {
        foreach (Ant ant in Ants)
        {
            ant.AddCityToTour(ant.GetCityOfTour(0));
            ant.CalculateTourLength();
        }
        tourComplete = true;
    }

    //init the ants with random tours
    protected void InitAnts(bool randomPlacement)
    {
        Ants = new List<Ant>();
        bool[] placed = new bool[cities.Count];

        for (int i = 0; i < numOfAnts; i++)
        {
            if (!randomPlacement)
            {
                Ants.Add(new Ant(i, cities.Count, 0, Distances));
            }
            else
            {
                int r = random.Next(0, cities.Count);
                while (placed[r] && !(numOfAnts > cities.Count))
                {
                    r = random.Next(0, cities.Count);
                }
                Ants.Add(new Ant(i, cities.Count, r, Distances));
                placed[r] = true;
            }
        }
    }

    // evaporation of pheromones
    protected void EvaporatePheromones()
    {
        double decreaseFactor = 1.0 - rho;

        for (int i = 0; i < cities.Count; i++)
        {
            for (int j = i + 1; j < cities.Count; j++)
            {
                Pheromones.DecreasePheromoneAs(i, j, decreaseFactor);
                Pheromones.DecreasePheromoneAs(j, i, decreaseFactor);
            }
        }
    }

    // evaporation of pheromones
    protected void EvaporatePheromones(int antIndex)
    {
        double decreaseFactor = 1.0 - rho;

        for (int i = 0; i < cities.Count; i++)
        {
            int j = Ants[antIndex].GetCityOfTour(i);
            int l = Ants[antIndex].GetCityOfTour(i + 1);

            Pheromones.DecreasePheromoneAs(j, l, decreaseFactor);
            Pheromones.DecreasePheromoneAs(l, j, decreaseFactor);
        }
    }

    // deposit procedure of pheromones
    protected void DepositPheromones(int antIndex, double increaseFactor)
    {
        for (int i = 0; i < cities.Count; i++)
        {
            int j = Ants[antIndex].GetCityOfTour(i);
            int l = Ants[antIndex].GetCityOfTour(i + 1);

            Pheromones.IncreasePheromoneAs(j, l, increaseFactor);
            Pheromones.IncreasePheromoneAs(l, j, increaseFactor);
        }
    }

    // deposit procedure of pheromones
    protected void FinishIteration()
    {
        choiceInfo.UpdateChoiceInfo(Pheromones, Distances, alpha, beta);
        tourComplete = false;
    }

    public List<Ant> Ants { get; protected set; }

    public Pheromones Pheromones { get; protected set; }

    public Distances Distances { get; protected set; }
}
