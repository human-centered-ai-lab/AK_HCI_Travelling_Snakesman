using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASAntInteraction : AntInteraction
{
    public ASAntInteraction(int alpha, int beta, double rho, int numOfAnts, List<City> cities) : base(alpha, beta, rho, numOfAnts, cities)
    {
        pheromoneTrailInitialValue = numOfAnts / Distances.CalculateNNHeuristic();
        Pheromones = new Pheromones(cities.Count, pheromoneTrailInitialValue);
        Pheromones.Init();

        choiceInfo = new ChoiceInfo(cities.Count);
        choiceInfo.UpdateChoiceInfo(Pheromones, Distances, alpha, beta);
    }

    public override void UpdateAnts()
    {
        InitAntUpdate();
        bool moveValid = true;

        for (int i = 1; i < cities.Count; i++)
        {

            moveValid = MoveAnts(i);

            if (!moveValid)
                throw new Exception("No valid next city!");
        }
        CompleteTours();
    }
    // updates an ant stepwise every city
    public override bool UpdateAntsStepwise(int citiesSoFar)
    {
        bool lastCity = false;

        if (!tourComplete)
        {
            if (citiesSoFar == 1)
                InitAntUpdate();
            else
            {
                lastCity = !MoveAnts(citiesSoFar - 1);
            }
            if (lastCity)
                CompleteTours();
        }

        return tourComplete;
    }
    // moves all ants one city ahead. returns false, if no city is available
    private bool MoveAnts(int currentCityPos)
    {
        for (int k = 0; k < Ants.Count; k++)
        {
            int nextCityIndex = DecisionRule(currentCityPos, k);
            if (nextCityIndex == noValidNextCity)
            {
                return false;
            }

            Ants[k].AddCityToTour(nextCityIndex);
            Ants[k].SetCityVisited(nextCityIndex);
        }
        return true;
    }

    // the core of the as algorithm: what city should the  ant select next
    private int DecisionRule(int currCityIndex, int antIndex)
    {
        CalculateProbs(currCityIndex, antIndex);
        return ExplorationDecision();
    }

    //updates the pheromones
    public override void UpdatePheromones()
    {
        EvaporatePheromones();

        for (int k = 0; k < Ants.Count; k++)
        {
            double increaseFactor = (1.0 / Ants[k].TourLength) * pheromoneIncreaseFactorParameter;
            DepositPheromones(k, increaseFactor);
        }

        FinishIteration();
    }
}
