/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* ChoiceInfo represents the choices to select a certain path with respect to pheromones and distances */

using System;

public class ChoiceInfo
{
    // a matrix representing the choice of selecting a path
    private double[][] choiceInfo;
    private int size;

    public ChoiceInfo(int numOfCities)
    {
        this.size = numOfCities;
        choiceInfo = new double[numOfCities][];
        for (int i = 0; i < numOfCities; i++)
            choiceInfo[i] = new double[numOfCities];
    }

    // choice update
    public void updateChoiceInfo(Pheromones pheromones, Distances distances, int alpha, int beta)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = i + 1; j < size; j++)
            {
                choiceInfo[i][j] = Math.Pow(pheromones.getPheromone(i, j), alpha) *
                                   Math.Pow((1.0 / distances.getDistance(i, j)), beta);
                choiceInfo[j][i] = choiceInfo[i][j];
            }
        }
    }

    public new string ToString
    {
        get
        {
            string str = "";
            for (int i = 0; i < choiceInfo.Length; i++)
            {
                str += "\n";
                for (int j = 0; j < choiceInfo[i].Length; j++)
                    str += choiceInfo[i][j] + " ";
            }
            return str;
        }
    }

    public double getChoice(int cityA, int cityB)
    {
        return choiceInfo[cityA][cityB];
    }
}
