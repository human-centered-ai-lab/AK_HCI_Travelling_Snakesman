/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* AntAlgorithmSimple is a wrapper class for the Ant Algorithm 
   -> use this script in your Unity scene */

using System.Collections.Generic;
using UnityEngine;

namespace AntAlgorithm
{
    public enum Mode
    {
        Normal
    }

    public class AntAlgorithmSimple : MonoBehaviour
    { 
        // influence of pheromone for decision
        [SerializeField]
        private int alpha = 1;
        // influence of distance for decision
        [SerializeField]
        private int beta = 2;
        // pheromone decrease factor
        [SerializeField]
        private double rho = 0.07;
        // pheromone increase factor
        [SerializeField]
        private double q = 100;

        private List<City> cities;
        // Ant interactions
        private AntInteraction _antInteraction;

        [SerializeField]
        private int numOfAnts = 25;

        [SerializeField]
        private int firstCity;

        //placeholder
        [SerializeField]
        private Mode mode;

        // output - updating after every algorithm iteration
        private List<int> _bestTour;

        public double BestTourLength { get; private set; }

        //helper
        private int _algStep = 1;

        // inits step of the algorithm
        // usage: use it once for initialization
        public void Init()
        {
            _antInteraction = new AntInteraction(alpha, beta, rho, q, numOfAnts, cities, firstCity);
            _bestTour = new List<int>();
            BestTourLength = double.MaxValue;
            CheckBestTour();
            _algStep = 1;
        }

        // iteration step of the algorithm - calculates a complete tour for every ant
        // usage: you can use it several times
        public void Iteration()
        {
            _antInteraction.UpdateAnts();
            _antInteraction.UpdatePheromones();
            CheckBestTour();
        }

        // all ants are moving one city ahead. If the routes are completed, a new iteration is starting.
        // usage: small part of the iteration. you should use it several times to complete an iteration
        public void Step()
        {
            if (_antInteraction.UpdateAntsStepwise(_algStep))
            {
                _algStep = 1;
                _antInteraction.UpdatePheromones();
                CheckBestTour();
            }
            else
            {
                _algStep++;
            }
        }

        // checks best tour so far
        private void CheckBestTour()
        {
            Ant bestAnt = _antInteraction.FindBestAnt();
            double tourLengthTemp = bestAnt.GetTourLength();

            if (tourLengthTemp < BestTourLength)
            {
                BestTourLength = tourLengthTemp;
                _bestTour = new List<int>(bestAnt.Tour);
            }
        }

        // debug output for best tour
        public void PrintBestTour(string context)
        {
            string str = "";
            foreach (int cityId in _bestTour)
            {
                str += cityId + " ";
            }
            Debug.Log("[" + context + "] Best Dist: " + BestTourLength + " Tour: " + str);
        }

        // usage: set cities before the initialization
        public void SetCities(List<City> cityList)
        {
            cities = cityList;
        }

        // after the initialization you can modify each ant
        public Ant GetAnt(int i)
        {
            return _antInteraction.GetAnts()[i];
        }

        // after the initialization you can modify the pheromones
        public Pheromones Pheromones
        {
            get
            {
                return _antInteraction.GetPheromones();
            }
        }

        public double GetTourLength()
        {
            return BestTourLength;
        }

        public List<int> GetTour()
        {
            return _bestTour;
        }
    }
}
