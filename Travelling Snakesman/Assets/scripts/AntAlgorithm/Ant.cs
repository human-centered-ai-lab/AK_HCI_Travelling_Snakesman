/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* Ant represents an ant in the Ant Algorihtm */

using System.Collections.Generic;
using UnityEngine;

namespace AntAlgorithm
{
    public class Ant
    {
        private readonly int _id;
        // every ant has an own tour
        private List<int> _tour;
        private double _tourLength;
        // if an ant is currently "on the road" you need to know the visited cities
        private readonly bool[] _visited;

        private readonly Distances _distances;

        public Ant(int id, int numOfCities, int firstCity, Distances distances)
        {
            _id = id;
            _tour = new List<int>();
            _visited = new bool[numOfCities];
            _distances = distances;

            // generate a random tour after initialization
            GenerateRandomTour(numOfCities, firstCity);
            CalculateTourLength();
        }

        // Generates a random tour by shuffeling the cities
        private void GenerateRandomTour(int numOfCities, int start)
        {
            for (int i = 0; i < numOfCities; i++)
                _tour.Add(i);

            //shuffle
            for (int i = 0; i < numOfCities; i++)
            {
                int random = Random.Range(i, numOfCities);
                int tmpC1 = _tour[random];
                _tour[random] = _tour[i];
                _tour[i] = tmpC1;
            }

            int target = -1;

            // search for the index of the target city
            for (int i = 0; i < _tour.Count; i++)
            {
                if (_tour[i] == start)
                {
                    target = i;
                }
            }

            int tmpC2 = _tour[0];
            _tour[0] = _tour[target];
            _tour[target] = tmpC2;
            _tour.Add(_tour[0]);
        }

        // Determines, if there is a path between two cities on the ants tour
        public bool IsEdge(int cityAId, int cityBId)
        {
            for (int i = 0; i < _tour.Count - 1; i++)
            {
                if (_tour[i] == cityAId && _tour[i + 1] == cityBId)
                    return true;
                if (_tour[i + 1] == cityAId && _tour[i] == cityBId)
                    return true;
            }
            return false;
        }

        //Calculates the tour length of the current ant tour
        public double CalculateTourLength()
        {
            _tourLength = 0;

            for (int i = 0; i < _tour.Count - 1; i++)
            {
                _tourLength += _distances.getDistance(_tour[i], _tour[i + 1]);
            }

            return _tourLength;
        }

        public new string ToString
        {
            get
            {
                string str = "";
                str += "ANT " + _id + ": Tour length: " + _tourLength + " CityOrder: ";
                foreach (int cityId in _tour)
                {
                    str += cityId + " ";
                }
                return str;
            }
        }

        public double GetTourLength()
        {
            return _tourLength;
        }

        public void SetCityVisited(int i)
        {
            _visited[i] = true;
        }

        public bool IsCityVisited(int i)
        {
            return _visited[i];
        }

        public void UnvisitAllCities()
        {
            for (int i = 0; i < _visited.Length; i++)
                _visited[i] = false;
        }

        public void ClearTour()
        {
            _tour.Clear();
        }

        public void AddCityToTour(int i)
        {
            _tour.Add(i);
        }


        public List<int> Tour
        {
            get
            {
                return _tour;
            }

            set
            {
                _tour = value;
            }
        }

        public int GetCityOfTour(int i)
        {
            return _tour[i];
        }
    }
}
