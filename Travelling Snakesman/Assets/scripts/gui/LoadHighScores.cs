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
            LoadHighscore();
        }

        public void LoadHighscore()
        {
            Dropdown tspDropdown = GameObject.Find("TspDropdown").GetComponent<Dropdown>();
            int menuIndex = tspDropdown.GetComponent<Dropdown>().value;
            List<Dropdown.OptionData> menuOptions = tspDropdown.GetComponent<Dropdown>().options;
            string tspName = menuOptions[menuIndex].text;
            PlayerPrefs.SetString("TspName", tspName);

#if UNITY_STANDALONE_WIN
            HighScoreHandler h = new HighScoreHandler();
            List<HighScoreEntry> scores = h.GetScores();
            setScores(scores);
#endif


#if UNITY_WEBGL

            HighScoreHandler h = new HighScoreHandler();
            StartCoroutine(h.ScoresWebGL());
            StartCoroutine(checkScores(h));
#endif           
        }

        IEnumerator checkScores( HighScoreHandler h )
        {
            while (!h.ReadHighScoresFinished )
                yield return new WaitForSeconds(0.1f);
            setScores(h.Result);
        }

        void setScores( List<HighScoreEntry> scores )
        {
            //scores.Sort((e1, e2) => e1.Score.CompareTo(e2.Score));
       
            var text = "";

            for (int i = 0; i < scores.Count; i++)
            {
                
                var highScoreEntry = scores[i];
                text += string.Format("{0}\t{1}\t\t{2}\n", (i + 1), highScoreEntry.Name, highScoreEntry.Score);
            }
            text = text.TrimEnd('\n');
            highScoreText.text = text;
        }
    }
}
