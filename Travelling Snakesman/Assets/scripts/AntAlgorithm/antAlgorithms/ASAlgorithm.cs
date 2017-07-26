/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* ASAlgorithm represents the AS ant algorithm implementation:
   "An  investigation  of  some  properties of an Ant algorithm" - 1992*/

using System.Collections.Generic;

namespace AntAlgorithms
{
    public class ASAlgorithm : AntAlgorithm
    {
        public ASAlgorithm(int alpha, int beta, double q, int numOfAnts)
        {
            this.alpha = alpha;
            this.beta = beta;
            this.q = q;
            this.numOfAnts = numOfAnts;
        }

        public override void Init()
        {
            antin = new ASAntInteraction(alpha, beta, q, numOfAnts, Cities);
            BestTour = new List<int>();
            TourLength = double.MaxValue;
            CheckBestTour();
            algStep = 1;
        }

        public override void Iteration()
        {
            antin.UpdateAnts();
            antin.UpdatePheromones();
            CheckBestTour();
        }

        public override void Step()
        {
            if (antin.UpdateAntsStepwise(algStep))
            {
                algStep = 1;
                antin.UpdatePheromones();
                CheckBestTour();
            }
            else
            {
                algStep++;
            }
        }
    }
}
