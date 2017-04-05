/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* AntInteraction represents the interaction with Ants */

using System.Collections.Generic;
using UnityEngine;

public class AntInteraction
{
    private static int noValidNextCity = -1;
    private string errorMessage;

    private int alpha;
    private int beta;
    private double rho;
    private double q;

    private int numOfAnts;
    private List<Ant> ants;
    private List<City> cities;

    private Pheromones pheromones;
    private Distances distances;
    private ChoiceInfo choiceInfo;
    private int startCity;

    //helper flags
    private bool tourComplete = false;

    public AntInteraction(int alpha, int beta, double rho, double q, int numOfAnts, List<City> cities, int firstCity)
    {
        this.cities = cities;
        this.alpha = alpha;
        this.beta = beta;
        this.rho = rho;
        this.q = q;
        this.numOfAnts = numOfAnts;
        this.startCity = firstCity;

        distances = new Distances(cities);
        initAnts();
        pheromones = new Pheromones(cities.Count);
        choiceInfo = new ChoiceInfo(cities.Count);
        choiceInfo.updateChoiceInfo(pheromones, distances, alpha, beta);
        Debug.Log("Choices: " + choiceInfo.ToString);
    }

    //init the ants with random tours
    private void initAnts()
    {
        ants = new List<Ant>();
        for (int i = 0; i < numOfAnts; i++)
        {
            ants.Add(new Ant(i, cities.Count, 0, distances));
        }
    }

    //update the tours of ants by considering the pheromones
    public void updateAnts()
    {
        initAntUpdate();

        for (int i = 1; i < cities.Count; i++)
        {
            if (!moveAnts(i))
                Debug.LogError("No valid next city!" + errorMessage);
        }

        completeTours();
    }

    //the init step of the ant update
    private void initAntUpdate()
    {
        for (int k = 0; k < ants.Count; k++)
        {
            ants[k].clearTour();
            ants[k].unvisitAllCities();
        }

        for (int k = 0; k < ants.Count; k++)
        {
            int start = startCity;//Random.Range(0, cities.Count);
            ants[k].addCityToTour(cities[start].getId());
            ants[k].setCityVisited(cities[start].getId());
        }
    }

    // simply adds the first city to the end of a tour
    private void completeTours()
    {
        for (int k = 0; k < ants.Count; k++)
        {
            ants[k].addCityToTour(ants[k].getCityOfTour(0));
            ants[k].calculateTourLength();
        }

        tourComplete = true;
    }

    // updates an ant stepwise every city
    public bool updateAntsStepwise(int citiesSoFar)
    {
        if (!tourComplete)
        {
            if (citiesSoFar == 1)
                initAntUpdate();
            else
            {
                if (!moveAnts(citiesSoFar - 1))
                {
                    completeTours();
                }
            }
        }
        return tourComplete;
    }

    // moves all ants one city ahead. returns false, if no city is available
    private bool moveAnts(int currentCityPos)
    {
        for (int k = 0; k < ants.Count; k++)
        {
            int nextCityIndex = asDecisionRule(currentCityPos, k);
            if (nextCityIndex == noValidNextCity)
                return false;

            ants[k].addCityToTour(nextCityIndex);
            ants[k].setCityVisited(nextCityIndex);
        }
        return true;
    }

    // the core of the ant algorithm: what city should the ant select next
    private int asDecisionRule(int currCityIndex, int antIndex)
    {
        double[] selectionProbability = new double[cities.Count];
        double sumProbabilities = 0.0;
        int currentCity = ants[antIndex].getCityOfTour(currCityIndex - 1);

        for (int i = 0; i < selectionProbability.Length; i++)
        {
            if (ants[antIndex].isCityVisited(i))
            {
                selectionProbability[i] = 0.0;
            }
            else
            {
                selectionProbability[i] = choiceInfo.getChoice(currentCity, i);
                sumProbabilities += selectionProbability[i];
            }
        }

        double[] probs = new double[cities.Count];
        for (int i = 0; i < probs.Length; i++)
            probs[i] = selectionProbability[i] / sumProbabilities;

        double[] cumulativeProbs = new double[selectionProbability.Length + 1];

        // calculate the comulative probabilities
        for (int i = 0; i < selectionProbability.Length; i++)
            cumulativeProbs[i + 1] = cumulativeProbs[i] + probs[i];

        //just to make sure
        cumulativeProbs[cumulativeProbs.Length-1] = 1.0f;

        double p = Random.value;

        for (int i = 0; i < cumulativeProbs.Length - 1; i++)
            if (p >= cumulativeProbs[i] && p <= cumulativeProbs[i + 1])
                return cities[i].getId();
        errorMessage = "Error: p=" + p + " ant=" + antIndex + " city=" + currCityIndex;
        return noValidNextCity;
    }

    //updates the pheromones
    public void updatePheromones()
    {
        double decreaseFactor = 1.0 - rho;
        double increaseFactor = 0;

        for (int i = 0; i < cities.Count; i++)
        {
            for (int j = i + 1; j < cities.Count; j++)
            {
                pheromones.decreasePheromone(i, j, decreaseFactor);
                pheromones.decreasePheromone(j, i, decreaseFactor);
            }
        }
        for (int k = 0; k < ants.Count; k++)
        {
            increaseFactor = (1.0 / ants[k].getTourLength()) * q;

            for (int i = 0; i < cities.Count; i++)
            {
                int j = ants[k].getCityOfTour(i);
                int l = ants[k].getCityOfTour(i + 1);

                pheromones.increasePheromone(j, l, increaseFactor);
                pheromones.increasePheromone(l, j, increaseFactor);
            }
        }

        choiceInfo.updateChoiceInfo(pheromones, distances, alpha, beta);
        tourComplete = false;

        //Debug.Log("Choices: " + choiceInfo.ToString);
        //Debug.Log("UPDATE PHEROMONE" + pheromones.ToString);
    }

    // finds the ant with the best tour
    public Ant findBestAnt()
    {
        double minTourLength;
        int minTourLengthIndex;

        minTourLength = ants[0].getTourLength();
        minTourLengthIndex = 0;
        for (int i = 1; i < ants.Count; i++)
        {
            if (ants[i].getTourLength() < minTourLength)
            {
                minTourLength = ants[i].getTourLength();
                minTourLengthIndex = i;
            }
        }
        return ants[minTourLengthIndex];
    }

    public List<Ant> getAnts()
    {
        return ants;
    }

    public Pheromones getPheromones()
    {
        return pheromones;
    }

    public Distances getDistances()
    {
        return distances;
    }
}

