/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* AntInteraction represents the interaction with Ants */

using System.Collections.Generic;
using UnityEngine;

namespace AntAlgorithm
{
    public class AntInteraction
    {
        private const int NoValidNextCity = -1;
        private string _errorMessage;

        private readonly int _alpha;
        private readonly int _beta;
        private readonly double _rho;
        private readonly double _q;

        private readonly int _numOfAnts;
        private List<Ant> _ants;
        private readonly List<City> _cities;

        private readonly Pheromones _pheromones;
        private readonly Distances _distances;
        private readonly ChoiceInfo _choiceInfo;
        private readonly int _startCity;

        //helper flags
        private bool _tourComplete;

        public AntInteraction(int alpha, int beta, double rho, double q, int numOfAnts, List<City> cities, int firstCity)
        {
            _cities = cities;
            _alpha = alpha;
            _beta = beta;
            _rho = rho;
            _q = q;
            _numOfAnts = numOfAnts;
            _startCity = firstCity;

            _distances = new Distances(cities);
            InitAnts();
            _pheromones = new Pheromones(cities.Count);
            _choiceInfo = new ChoiceInfo(cities.Count);
            _choiceInfo.updateChoiceInfo(_pheromones, _distances, alpha, beta);
            Debug.Log("Choices: " + _choiceInfo.ToString);
        }

        //init the ants with random tours
        private void InitAnts()
        {
            _ants = new List<Ant>();
            for (int i = 0; i < _numOfAnts; i++)
            {
                _ants.Add(new Ant(i, _cities.Count, 0, _distances));
            }
        }

        //update the tours of ants by considering the pheromones
        public void UpdateAnts()
        {
            InitAntUpdate();

            for (int i = 1; i < _cities.Count; i++)
            {
                if (!MoveAnts(i))
                {
                    Debug.LogError("No valid next city!" + _errorMessage);
                }
            }
            CompleteTours();
        }

        //the init step of the ant update
        private void InitAntUpdate()
        {
            foreach (var ant in _ants)
            {
                ant.ClearTour();
                ant.UnvisitAllCities();
            }

            foreach (var ant in _ants)
            {
                int start = _startCity;//Random.Range(0, cities.Count);
                ant.AddCityToTour(_cities[start].getId());
                ant.SetCityVisited(_cities[start].getId());
            }
        }

        // simply adds the first city to the end of a tour
        private void CompleteTours()
        {
            foreach (var ant in _ants)
            {
                ant.AddCityToTour(ant.GetCityOfTour(0));
                ant.CalculateTourLength();
            }

            _tourComplete = true;
        }

        // updates an ant stepwise every city
        public bool UpdateAntsStepwise(int citiesSoFar)
        {
            if (_tourComplete) return _tourComplete;
            if (citiesSoFar == 1)
            {
                InitAntUpdate();
            }
            else
            {
                if (!MoveAnts(citiesSoFar - 1))
                {
                    CompleteTours();
                }
            }
            return _tourComplete;
        }

        // moves all ants one city ahead. returns false, if no city is available
        private bool MoveAnts(int currentCityPos)
        {
            for (int k = 0; k < _ants.Count; k++)
            {
                int nextCityIndex = AsDecisionRule(currentCityPos, k);
                if (nextCityIndex == NoValidNextCity)
                    return false;

                _ants[k].AddCityToTour(nextCityIndex);
                _ants[k].SetCityVisited(nextCityIndex);
            }
            return true;
        }

        // the core of the ant algorithm: what city should the ant select next
        private int AsDecisionRule(int currCityIndex, int antIndex)
        {
            var selectionProbability = new double[_cities.Count];
            double sumProbabilities = 0.0;
            int currentCity = _ants[antIndex].GetCityOfTour(currCityIndex - 1);

            for (int i = 0; i < selectionProbability.Length; i++)
            {
                if (_ants[antIndex].IsCityVisited(i))
                {
                    selectionProbability[i] = 0.0;
                }
                else
                {
                    selectionProbability[i] = _choiceInfo.getChoice(currentCity, i);
                    sumProbabilities += selectionProbability[i];
                }
            }

            var probs = new double[_cities.Count];
            for (int i = 0; i < probs.Length; i++)
                probs[i] = selectionProbability[i] / sumProbabilities;

            var cumulativeProbs = new double[selectionProbability.Length + 1];

            // calculate the comulative probabilities
            for (int i = 0; i < selectionProbability.Length; i++)
                cumulativeProbs[i + 1] = cumulativeProbs[i] + probs[i];

            //just to make sure
            cumulativeProbs[cumulativeProbs.Length-1] = 1.0f;

            double p = Random.value;

            for (int i = 0; i < cumulativeProbs.Length - 1; i++)
                if (p >= cumulativeProbs[i] && p <= cumulativeProbs[i + 1])
                    return _cities[i].getId();
            _errorMessage = "Error: p=" + p + " ant=" + antIndex + " city=" + currCityIndex;
            return NoValidNextCity;
        }

        //updates the pheromones
        public void UpdatePheromones()
        {
            double decreaseFactor = 1.0 - _rho;

            for (int i = 0; i < _cities.Count; i++)
            {
                for (int j = i + 1; j < _cities.Count; j++)
                {
                    _pheromones.DecreasePheromone(i, j, decreaseFactor);
                    _pheromones.DecreasePheromone(j, i, decreaseFactor);
                }
            }
            foreach (var ant in _ants)
            {
                double increaseFactor = (1.0 / ant.GetTourLength()) * _q;

                for (int i = 0; i < _cities.Count; i++)
                {
                    int j = ant.GetCityOfTour(i);
                    int l = ant.GetCityOfTour(i + 1);

                    _pheromones.IncreasePheromone(j, l, increaseFactor);
                    _pheromones.IncreasePheromone(l, j, increaseFactor);
                }
            }

            _choiceInfo.updateChoiceInfo(_pheromones, _distances, _alpha, _beta);
            _tourComplete = false;

            //Debug.Log("Choices: " + choiceInfo.ToString);
            //Debug.Log("UPDATE PHEROMONE" + pheromones.ToString);
        }

        // finds the ant with the best tour
        public Ant FindBestAnt()
        {
            double minTourLength = _ants[0].GetTourLength();
            int minTourLengthIndex = 0;
            for (int i = 1; i < _ants.Count; i++)
            {
                if (_ants[i].GetTourLength() < minTourLength)
                {
                    minTourLength = _ants[i].GetTourLength();
                    minTourLengthIndex = i;
                }
            }
            return _ants[minTourLengthIndex];
        }

        public List<Ant> GetAnts()
        {
            return _ants;
        }

        public Pheromones GetPheromones()
        {
            return _pheromones;
        }

        public Distances GetDistances()
        {
            return _distances;
        }
    }
}

