/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* ChoiceInfo represents the choices to select a certain path with respect to pheromones and distances */

using System;

namespace AntAlgorithm
{
    public class ChoiceInfo
    {
        // a matrix representing the choice of selecting a path
        private double[,] _choiceInfo;
        private readonly int _size;

        public ChoiceInfo(int numOfCities)
        {
            _size = numOfCities;
            _choiceInfo = new double[numOfCities, numOfCities];
        }

        // choice update
        public void UpdateChoiceInfo(Pheromones pheromones, Distances distances, int alpha, int beta)
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = i + 1; j < _size; j++)
                {
                    _choiceInfo[i, j] = Math.Pow(pheromones.GetPheromone(i, j), alpha) *
                                       Math.Pow((1.0 / distances.getDistance(i, j)), beta);
                    _choiceInfo[j, i] = _choiceInfo[i, j];
                }
            }
        }

        public new string ToString
        {
            get
            {
                string str = "";
                for (int i = 0; i < _choiceInfo.GetLength(0); i++)
                {
                    str += "\n";
                    for (int j = 0; j < _choiceInfo.GetLength(1); j++)
                    {
                        str += _choiceInfo[i, j] + " ";
                    }
                }
                return str;
            }
        }

        public double GetChoice(int cityA, int cityB)
        {
            return _choiceInfo[cityA, cityB];
        }
    }
}
