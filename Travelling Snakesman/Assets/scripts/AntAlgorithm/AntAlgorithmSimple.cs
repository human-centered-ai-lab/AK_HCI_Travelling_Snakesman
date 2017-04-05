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

namespace AntAlgorithms
{
    public enum Mode
    {
        normal
    }

    public class AntAlgorithmSimple : MonoBehaviour
    {
        // influence of pheromone for decision
        public int alpha = 1;
        // influence of distance for decision
        public int beta = 2;
        // pheromone decrease factor
        public double rho = 0.07;
        // pheromone increase factor
        public double q = 100;

        private List<City> cities;
        // Ant interactions
        private AntInteraction antin;

        public int numOfAnts;
        public int firstCity;

        //placeholder
        public Mode mode;

        // output - updateing after every algorithm iteration
        private double tourLength;
        private List<int> bestTour;

        //helper
        private int algStep = 1;

        // inits step of the algorithm
        // usage: use it once for initialization
        public void init()
        {
            antin = new AntInteraction(alpha, beta, rho, q, numOfAnts, cities, firstCity);
            bestTour = new List<int>();
            tourLength = double.MaxValue;
            checkBestTour();
            algStep = 1;
        }

        // iteration step of the algorithm - calculates a complete tour for every ant
        // usage: you can use it several times
        public void iteration()
        {
            antin.updateAnts();
            antin.updatePheromones();
            checkBestTour();
        }

        // all ants are moving one city ahead. If the routes are completed, a new iteration is starting.
        // usage: small part of the iteration. you should use it several times to complete an iteration
        public void step()
        {
            if (antin.updateAntsStepwise(algStep))
            {
                algStep = 1;
                antin.updatePheromones();
                checkBestTour();
            }
            else
            {
                algStep++;
            }
        }

        // checks best tour so far
        private bool checkBestTour()
        {
            Ant bestAnt = antin.findBestAnt();
            double tourLengthTemp = bestAnt.getTourLength();

            if (tourLengthTemp < tourLength)
            {
                tourLength = tourLengthTemp;
                bestTour.Clear();
                for (int i = 0; i < bestAnt.getTour().Count; i++)
                    bestTour.Add(bestAnt.getTour()[i]);
                return true;
            }
            return false;
        }

        // debug output for best tour
        public void printBestTour(string context)
        {
            string str = "";
            for (int i = 0; i < bestTour.Count; i++)
                str += bestTour[i] + " ";
            Debug.Log("[" + context + "] Best Dist: " + tourLength + " Tour: " + str);
        }

        // usage: set cities before the initialization
        public void setCities(List<City> cities)
        {
            this.cities = cities;
        }

        // after the initialization you can modify each ant
        public Ant getAnt(int i)
        {
            return antin.getAnts()[i];
        }

        // after the initialization you can modify the pheromones
        public Pheromones getPheromones()
        {
            return antin.getPheromones();
        }

        public double getTourLength()
        {
            return tourLength;
        }

        public List<int> getTour()
        {
            return bestTour;
        }
    }
}
