/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* Ant represents an ant in the Ant Algorihtm */

using System.Collections.Generic;
using UnityEngine;

public class Ant
{
    private int ID;
    // every ant has an own tour
    private List<int> tour;
    private double tourLength;
    // if an ant is currently "on the road" you need to know the visited cities
    private bool[] visited;

    private Distances distances;

    public Ant(int ID, int numOfCities, int firstCity, Distances distances)
    {
        this.ID = ID;
        tour = new List<int>();
        visited = new bool[numOfCities];
        this.distances = distances;

        // generate a random tour after initialization
        generateRandomTour(numOfCities, firstCity);
        calculateTourLength();
    }

    // Generates a random tour by shuffeling the cities
    private void generateRandomTour(int numOfCities, int start)
    {
        for (int i = 0; i < numOfCities; i++)
            tour.Add(i);

        //shuffle
        for (int i = 0; i < numOfCities; i++)
        {
            int random = Random.Range(i, numOfCities);
            int tmpC1 = tour[random];
            tour[random] = tour[i];
            tour[i] = tmpC1;
        }

        int target = -1;

        // search for the index of the target city
        for (int i = 0; i < tour.Count; i++)
        {
            if (tour[i] == start)
            {
                target = i;
            }
        }

        int tmpC2 = tour[0];
        tour[0] = tour[target];
        tour[target] = tmpC2;
        tour.Add(tour[0]);
    }

    // Determines, if there is a path between two cities on the ants tour
    public bool isEdge(int cityAID, int cityBID)
    {
        for (int i = 0; i < tour.Count - 1; i++)
        {
            if (tour[i] == cityAID && tour[i + 1] == cityBID)
                return true;
            else if (tour[i + 1] == cityAID && tour[i] == cityBID)
                return true;
        }

        return false;
    }

    //Calculates the tour length of the current ant tour
    public double calculateTourLength()
    {
        tourLength = 0;

        for (int i = 0; i < tour.Count - 1; i++)
        {
            tourLength += distances.getDistance(tour[i], tour[i + 1]);
        }

        return tourLength;
    }

    public new string ToString
    {
        get
        {
            string str = "";
            str += "ANT " + ID + ": Tour length: " + tourLength + " CityOrder: ";
            for (int i = 0; i < tour.Count; i++)
                str += tour[i] + " ";
            return str;
        }
    }

    public double getTourLength()
    {
        return tourLength;
    }

    public void setCityVisited(int i)
    {
        visited[i] = true;
    }

    public bool isCityVisited(int i)
    {
        if (visited[i])
            return true;
        return false;
    }

    public void unvisitAllCities()
    {
        for (int i = 0; i < visited.Length; i++)
            visited[i] = false;
    }

    public void clearTour()
    {
        this.tour.Clear();
    }

    public void addCityToTour(int i)
    {
        tour.Add(i);
    }

    public List<int> getTour()
    {
        return tour;
    }

    public void setTour(List<int> tour)
    {
        this.tour = tour;
    }

    public int getCityOfTour(int i)
    {
        return tour[i];
    }
}
