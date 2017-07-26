using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxAntInteraction : AntInteraction
{
    private double pBest;

    public MinMaxAntInteraction(int alpha, int beta, double rho, int numOfAnts, List<City> cities, double pBest) : base(alpha, beta, rho, numOfAnts, cities)
    {
        this.pBest = pBest;
        pheromoneTrailInitialValue = 1.0f / (rho * Distances.CalculateNNHeuristic());

        Pheromones = new Pheromones(cities.Count, pheromoneTrailInitialValue, pBest);
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

    // the core of the as algorithm: what city should the  ant select next
    private int DecisionRule(int currCityIndex, int antIndex)
    {
        CalculateProbs(currCityIndex, antIndex);
        return ExplorationDecision();
    }

    public override bool UpdateAntsStepwise(int citiesSoFar)
    {
        throw new NotImplementedException();
    }

    public override void UpdatePheromones()
    {
        int bestAntIndex = FindBestAnt().Id;

        EvaporatePheromones();

        double increaseFactor = (1.0 / Ants[bestAntIndex].TourLength);

        //Debug.Log("Best Ant: " + bestAntIndex + " Increase Factor: "+ increaseFactor);
        //Debug.Log("Tour " + Ants[bestAntIndex].ToString);

        DepositPheromones(bestAntIndex, increaseFactor);
        Pheromones.UpdateTrailLimits(Ants[bestAntIndex].TourLength, rho, pBest);
        Pheromones.CheckPheromoneTrailLimits();

       // Debug.Log("Pheromones: " + Pheromones.ToString);


        FinishIteration();
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
}
