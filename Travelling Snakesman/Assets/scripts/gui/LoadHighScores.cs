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
        private const string WEEKLY_MODE_TEXT = "WEEKLY";
        private const string ALLTIME_MODE_TEXT = "ALLTIME";
        private const string DAILY_MODE_TEXT = "TODAY";

        [SerializeField]
        private Text remainingHighscoreText1;
        [SerializeField]
        private Text remainingHighscoreText2;
        [SerializeField]
        private Text remainingHighscoreText3;
        [SerializeField]
        private Text firstPlaceText;
        [SerializeField]
        private Text secondPlaceText;
        [SerializeField]
        private Text thirdPlaceText;
        [SerializeField]
        private Text firstPlaceScore;
        [SerializeField]
        private Text secondPlaceScore;
        [SerializeField]
        private Text thirdPlaceScore;
        [SerializeField]
        private Button leftButton;
        [SerializeField]
        private Button rightButton;
        [SerializeField]
        private Text modeText;
        [SerializeField]
        private HighScoreHandler highScoreHandler;

        // Use this for initialization
        void Start()
        {
            //print("Waiting for High Scores...");
            PlayerPrefs.SetString("TimeSpan", ALLTIME_MODE_TEXT);
            modeText.text = ALLTIME_MODE_TEXT;
            rightButton.GetComponent<Button>().onClick.AddListener(ChangeModeRight);
            leftButton.GetComponent<Button>().onClick.AddListener(ChangeModeLeft);
            LoadHighscore();
        }
        public void ChangeModeRight()
        {
            if (modeText.text.Equals(ALLTIME_MODE_TEXT))
            {
                modeText.text = WEEKLY_MODE_TEXT;
                PlayerPrefs.SetString("TimeSpan", WEEKLY_MODE_TEXT);
            }
            else if (modeText.text.Equals(WEEKLY_MODE_TEXT))
            {
                modeText.text = DAILY_MODE_TEXT;
                PlayerPrefs.SetString("TimeSpan", DAILY_MODE_TEXT);
            }
            else if (modeText.text.Equals(DAILY_MODE_TEXT))
            {
                modeText.text = ALLTIME_MODE_TEXT;
                PlayerPrefs.SetString("TimeSpan", ALLTIME_MODE_TEXT);
            }
            LoadHighscore();
        }
        public void ChangeModeLeft()
        {
            if (modeText.text == ALLTIME_MODE_TEXT)
            {
                modeText.text = DAILY_MODE_TEXT;
                PlayerPrefs.SetString("TimeSpan", DAILY_MODE_TEXT);
            }
            else if (modeText.text == DAILY_MODE_TEXT)
            {
                modeText.text = WEEKLY_MODE_TEXT;
                PlayerPrefs.SetString("TimeSpan", WEEKLY_MODE_TEXT);
            }
            else if (modeText.text == WEEKLY_MODE_TEXT)
            {
                modeText.text = ALLTIME_MODE_TEXT;
                PlayerPrefs.SetString("TimeSpan", ALLTIME_MODE_TEXT);
            }
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
            List<HighScoreEntry> scores = highScoreHandler.GetScores(PlayerPrefs.GetString("TspName"), HighScoreHandler.ORDER_TYPE_ASC);
            setScores(scores);
#endif


#if UNITY_WEBGL || UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

            StartCoroutine(highScoreHandler.ScoresWebGL());
            StartCoroutine(checkScores(highScoreHandler));
#endif
        }

        IEnumerator checkScores(HighScoreHandler h)
        {
            while (!h.ReadHighScoresFinished)
                yield return new WaitForSeconds(0.1f);
            setScores(h.Result);
        }

        void setScores(List<HighScoreEntry> scores)
        {
            scores.Sort((e1, e2) => e1.Time.CompareTo(e2.Time));

            var place = "";
            var text = "";
            var score = "";

            int size = scores.Count;

            for (int i = 0; i < 3; i++)
            {
                if (scores.Count > i)
                {
                    text = scores[i].Name;
                    score = StringOperations.GetStringFromTime(scores[i].Time);
                }
                if (i == 0)
                {
                    firstPlaceText.text = text;
                    firstPlaceScore.text = score;
                }
                if (i == 1)
                {
                    secondPlaceText.text = text;
                    secondPlaceScore.text = score;
                }
                if (i == 2)
                {
                    thirdPlaceText.text = text;
                    thirdPlaceScore.text = score;
                }
                text = "";
                score = "";
            }

            for (int i = 3; i < scores.Count; i++)
            {
                var highScoreEntry = scores[i];
                place += (i + 1) + "\n";
                text += highScoreEntry.Name + "\n";
                score += StringOperations.GetStringFromTime(scores[i].Time) + "\n";
            }
            remainingHighscoreText1.text = place;
            remainingHighscoreText2.text = text;
            remainingHighscoreText3.text = score;

            text = text.TrimEnd('\n');
        }
    }
}
