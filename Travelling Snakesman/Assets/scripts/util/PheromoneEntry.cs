
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneEntry : MonoBehaviour
{
    public int HighestValueIndex { get; set; }
    public int PreviousChosenValueIndex { get; set; }
    public int Iteration { get; set; }
    public string Pheromones { get; set; }

    public PheromoneEntry(int highestValueIndex, int previousChosenValueIndex, int iteration, double[] pheromones)
    {
        this.HighestValueIndex = highestValueIndex;
        this.PreviousChosenValueIndex = previousChosenValueIndex;
        this.Iteration = iteration;
        this.Pheromones = ConvertPheromonesToString(pheromones);
    }
    private string ConvertPheromonesToString(double[] pheromones)
    {
        string text = "";
        for(int i = 0; i < pheromones.Length; i++)
        {
            text += PreviousChosenValueIndex + "-" + i + ":" + pheromones[i] + "  ";
        }
        return text;
    }
}

