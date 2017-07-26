using System.Collections;
using System.Collections.Generic;
using AntAlgorithms;
using UnityEngine;
using System;

public class ACSAntInteraction : AntInteraction
{
    private double acsQ0;
    private double tau0;

    public ACSAntInteraction(int alpha, int beta, double rho, int numOfAnts, List<City> cities, double acsQ0) : base( alpha, beta, rho, numOfAnts, cities)
    {
        this.acsQ0 = acsQ0;
        tau0 = 1.0f / (cities.Count * Distances.CalculateNNHeuristic());
        pheromoneTrailInitialValue = this.tau0;

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
    private bool MoveAnts(int currentCityAmount)
    {
        for (int k = 0; k < Ants.Count; k++)
        {
            int nextCityIndex = DecisionRule(currentCityAmount, k);
            if (nextCityIndex == noValidNextCity)
            {
                return false;
            }

            Ants[k].AddCityToTour(nextCityIndex);
            Ants[k].SetCityVisited(nextCityIndex);

            // local update for pheromones
            LocalPheromoneUpdate(k, currentCityAmount);
        }

        return true;
    }


    // the core of the acs algorithm: what city should the  ant select next
    private int DecisionRule(int currCityIndex, int antIndex)
    {
        double q = random.NextDouble();

        CalculateProbs(currCityIndex, antIndex);

        if (q <= acsQ0)
        {
            return cities[bestProbIndex].Id;
        }

        return ExplorationDecision();
    }

    //updates the pheromones for ACS
    public override void UpdatePheromones()
    {
        int bestAntIndex = FindBestAnt().Id;

        EvaporatePheromones();

        double increaseFactor = (1.0 / Ants[bestAntIndex].TourLength) * rho;

        DepositPheromones(bestAntIndex, increaseFactor);

        FinishIteration();
    }

    // the local pheromone update is done after each step (each ant one city step)
    private void LocalPheromoneUpdate(int antIndex, int currentCityAmount)
    {
        int prevCity = Ants[antIndex].GetCityOfTour(currentCityAmount - 1);
        int currentCity = Ants[antIndex].GetCityOfTour(currentCityAmount);

        Pheromones.SetPheromone(prevCity, currentCity, ((1.0f - rho) * Pheromones.GetPheromone(prevCity, currentCity)) + (rho * tau0));
        Pheromones.SetPheromone(currentCity, prevCity, Pheromones.GetPheromone(prevCity, currentCity));
        choiceInfo.SetChoice(prevCity, currentCity, Math.Pow(Pheromones.GetPheromone(prevCity, currentCity), alpha) *
                                   Math.Pow((1.0 / Distances.GetDistance(prevCity, currentCity)), beta));
        choiceInfo.SetChoice(currentCity, prevCity, choiceInfo.GetChoice(prevCity, currentCity));
    }


}
