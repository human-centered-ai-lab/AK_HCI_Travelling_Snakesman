using AntAlgorithm.tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieChartController : MonoBehaviour
{
    public List<float> values;
    public Color[] wedgeColors;
    public Image wedgePrefab;
    public Text headline;

    void Start()
    {
        values = new List<float>();
        for (int i = 0; i < 3; i++)
            values.Add(0f);
        headline.text = "Statistics for " + PlayerPrefs.GetString("TspName");
        HighScoreHandler hsh = new HighScoreHandler();

#if UNITY_STANDALONE_WIN
            List<HighScoreEntry> scores = hsh.GetScores(PlayerPrefs.GetString("TspName"), HighScoreHandler.ORDER_TYPE_ASC, AntAlgorithmManager.GameName,-1);
        HandleScores(scores);
                MakeGraph();

#endif


#if UNITY_WEBGL || UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

        StartCoroutine(hsh.ScoresWebGL());
        StartCoroutine(checkScores(hsh));
#endif
    }
    IEnumerator checkScores(HighScoreHandler h)
    {
        while (!h.ReadHighScoresFinished)
            yield return new WaitForSeconds(0.1f);
        HandleScores(h.Result);
        MakeGraph();

    }
    void HandleScores(List<HighScoreEntry> scores)
    {
        for (int i = 0; i < scores.Count; i++)
        {
            if (scores[i].UserScore < scores[i].AlgoScore)
            {
                values[0] += 1;
            }
            else if (scores[i].UserScore > scores[i].AlgoScore)
            {
                values[2] += 1;
            }
            else
            {
                values[1] += 1;
            }

        }
    }
    void MakeGraph()
    {
        float total = 0f;
        float zRotation = 0f;
        for (int i = 0; i < values.Count; i++)
            total += values[i];
        for (int i = 0; i < values.Count; i++)
        {
            Image newWedge = Instantiate(wedgePrefab) as Image;
            newWedge.transform.SetParent(transform, false);

            newWedge.color = wedgeColors[i];
            newWedge.fillAmount = values[i] / total;
            newWedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= newWedge.fillAmount * 360f;
        }
    }
    void Update()
    {

    }
}
