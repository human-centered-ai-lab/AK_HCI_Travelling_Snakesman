using System.Collections;
using System.Collections.Generic;
using AntAlgorithm.tools;
using util;
using UnityEngine;
using UnityEngine.UI;

namespace gui
{
    public class LoadHighScores : MonoBehaviour
    {
        [SerializeField] private Text highScoreText;
        // Use this for initialization
        void Start ()
        {
            print("Waiting for High Scores...");
            var scores = HighScoreHandler.GetScores();
            scores.Sort((e1, e2) => e1.Score.CompareTo(e2.Score));

            var text = "";

            for(int i = 0; i < scores.Count; i++)
            {
                var highScoreEntry = scores[i];
                text += string.Format("{0}\t{1}\t\t{2}\n", (i+1), highScoreEntry.Name.PadLeft(20), highScoreEntry.Score);
            }
            text = text.TrimEnd('\n');
            highScoreText.text = text;
        }        
    }
}
