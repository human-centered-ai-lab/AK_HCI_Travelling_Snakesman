/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* Ant represents an ant in the Ant Algorihtm */

using System;
using System.Collections.Generic;

public class Ant
{
    // every ant has it's own tour
    // if an ant is currently "on the road" you need to know the visited cities
    private bool[] visited;

    private Distances distances;
    private Random random;

    public Ant(int id, int numOfCities, int firstCity, Distances distances)
    {
        random = new Random();
        Id = id;
        Tour = new List<int>();
        visited = new bool[numOfCities];
        this.distances = distances;

        // generate a random tour after initialization
        GenerateRandomTour(numOfCities, firstCity);
        CalculateTourLength();
    }

    // Generates a random tour by shuffeling the cities
    private void GenerateRandomTour(int numOfCities, int start)
    {
        for (int i = 0; i < numOfCities; i++)
        {
            Tour.Add(i);
        }

        //shuffle
        for (int i = 0; i < numOfCities; i++)
        {
            int rand = random.Next(i, numOfCities);
            int tmpC1 = Tour[rand];
            Tour[rand] = Tour[i];
            Tour[i] = tmpC1;
        }

        int target = -1;

        // search for the index of the target city
        for (int i = 0; i < Tour.Count; i++)
        {
            if (Tour[i] == start)
            {
                target = i;
            }
        }

        int tmpC2 = Tour[0];
        Tour[0] = Tour[target];
        Tour[target] = tmpC2;
        Tour.Add(Tour[0]);
    }

    // Determines, if there is a path between two cities on the ants tour
    public bool IsEdge(int cityAID, int cityBID)
    {
        for (int i = 0; i < Tour.Count - 1; i++)
        {
            if (Tour[i] == cityAID && Tour[i + 1] == cityBID)
            {
                return true;
            }
            if (Tour[i + 1] == cityAID && Tour[i] == cityBID)
            {
                return true;
            }
        }

        return false;
    }

    //Calculates the tour length of the current ant tour
    public double CalculateTourLength()
    {
        TourLength = 0;

        for (int i = 0; i < Tour.Count - 1; i++)
        {
            TourLength += distances.GetDistance(Tour[i], Tour[i + 1]);
        }

        return TourLength;
    }

    public new string ToString
    {
        get
        {
            string str = "";
            str += "ANT " + Id + ": Tour length: " + TourLength + " CityOrder: ";
            foreach (int cityIndex in Tour)
            {
                str += cityIndex + " ";
            }
            str += " visited: ";
            for (int i = 0; i < visited.Length; i++)
            {
                str += visited[i] + " ";
            }
            return str;
        }
    }

    public double TourLength { get; private set; }

    public void SetCityVisited(int i)
    {
        visited[i] = true;
    }

    public bool IsCityVisited(int i)
    {
        return visited[i];
    }

    public void UnvisitAllCities()
    {
        for (int i = 0; i < visited.Length; i++)
        {
            visited[i] = false;
        }
    }

    public void ClearTour()
    {
        Tour.Clear();
    }

    public void AddCityToTour(int i)
    {
        Tour.Add(i);
    }

    public List<int> Tour { get; set; }

    public int GetCityOfTour(int i)
    {
        return Tour[i];
    }

    public int Id { get; private set; }
}
