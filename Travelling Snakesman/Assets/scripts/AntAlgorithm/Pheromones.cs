/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* Pheromone represents the pheromones between cities */

using util;

namespace AntAlgorithm
{
    public class Pheromones
    {
        // initialization factor for pheromones
        public static double InitPheromoneValue = 10;
        // Matrix of pheromones between city x and city y
        private double[,] _pheromones;

        public Pheromones(int numOfCities)
        {
            InitPheromones(numOfCities);
        }

        // init of pheromones
        private void InitPheromones(int numOfCities)
        {
            _pheromones = new double[numOfCities, numOfCities];
            for (int i = 0; i < _pheromones.GetLength(0); i++)
            {
                for (int j = 0; j < _pheromones.GetLength(1); j++)
                {
                    _pheromones[i, j] = InitPheromoneValue;
                }
            }
        }

        public new string ToString
        {
            get
            {
                string str = "";
                for (int i = 0; i < _pheromones.GetLength(0); i++)
                {
                    str += "\n";
                    for (int j = 0; j < _pheromones.GetLength(1); j++)
                    {
                        str += _pheromones[i, j] + " ";
                    }
                }
                return str;
            }
        }

        // decrease the pheromone value between 2 particular cities by one ant 
        public void DecreasePheromone(int cityAId, int cityBId, double decreaseFactor)
        {
            _pheromones[cityAId, cityBId] = decreaseFactor * _pheromones[cityAId, cityBId];
        }

        // decrease the pheromone value between 2 particular cities by one ant 
        public void IncreasePheromone(int cityAId, int cityBId, double increaseValue)
        {
            _pheromones[cityAId, cityBId] = _pheromones[cityAId, cityBId] + increaseValue;
        }

        public double GetPheromone(int cityAId, int cityBId)
        {
            return _pheromones[cityAId, cityBId];
        }

        public double[] GetPheromones(int cityId)
        {
            return _pheromones.GetRow(cityId);
        }
    }
}
