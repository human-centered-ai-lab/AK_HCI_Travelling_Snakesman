/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* AntAlgorithmController is the wrapper script. 
 * Drag it into your Unity Scene:
 * usage: AntAlgorithmChooser aac = new AntAlgorithmChooser(mode, alpha, beta, rho, q, numOfAnts, firstCity, acsQ0) - acsQ0 only matters if the algorithm is ACS
          AntAlgorithm aa = aac.getAlgorithm(); 
   "An  investigation  of  some  properties of an Ant algorithm" - 1992 
    */

using System.Reflection;
[assembly: AssemblyVersion("0.1")]

namespace AntAlgorithms
{
    public enum Mode
    {
        AntSystem,
        AntColonySystem,
        MinMaxAntSystem
    }

    public class AntAlgorithmChooser
    {
        public static double PHERDEFAULTINITVALUE = 0.1;

        public AntAlgorithmChooser(Mode mode, int alpha, int beta, double q, int numOfAnts, double acsQ0, double pBest)
        {
            // TODO: intelligent switch due to parameters
            switch (mode)
            {
                case Mode.AntSystem:
                    Algorithm = new ASAlgorithm(alpha, beta, q, numOfAnts);
                    break;
                case Mode.AntColonySystem:
                    Algorithm = new ACSAlgorithm(alpha, beta, q, numOfAnts, acsQ0);
                    break;
                case Mode.MinMaxAntSystem:
                    Algorithm = new MinMaxAlgorithm(alpha, beta, q, numOfAnts, pBest);
                    break;
            }
        }

        public AntAlgorithmChooser(int alpha, int beta, double q, int numOfAnts)
        {
            Algorithm = new ASAlgorithm(alpha, beta, q, numOfAnts);
        }

        public AntAlgorithmChooser(int alpha, int beta, double q, int numOfAnts,  double acsQ0)
        {
            Algorithm = new ACSAlgorithm(alpha, beta, q, numOfAnts, acsQ0);
        }

        public AntAlgorithm Algorithm { get; private set; }
    }
}
