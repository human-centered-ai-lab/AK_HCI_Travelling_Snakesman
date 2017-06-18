using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using util;

namespace AntAlgorithm.tools
{
    public class HighScoreEntry
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Score { get; private set; }

        public string Comment { get; private set; }

        private HighScoreEntry()
        {
            Id = -1;
            Name = null;
            Score = -1;
            Comment = null;
        }

        public static HighScoreEntry Create(string line)
        {
            var entry = new HighScoreEntry();
            foreach (var kvPair in line.Split(new []{" - "}, StringSplitOptions.RemoveEmptyEntries))
            {
                var splitPair = kvPair.Split(':');
                if(splitPair.Length != 2)
                    continue;
                var key = splitPair[0].Trim().ToLower();
                var value = splitPair[1].Trim();
                switch (key)
                {
                    case "id":
                        entry.Id = int.Parse(value);
                        break;
                    case "name":
                        entry.Name = value;
                        break;
                    case "score":
                        entry.Score = int.Parse(value);
                        break;
                    case "comment":
                        entry.Comment = value.Trim();
                        break;
                }
            }
            return entry.IsInitialized() ? entry : null;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, Score);
        }

        public bool IsInitialized()
        {
            return Id != -1
                   && Name != null
                   && Score != -1;
        }
    }


    public class HighScoreHandler : MonoBehaviour
    {
        private const string SecretKey = "rLdZyTAJeynUh6JDR8Sut8Yj1sLXIPWO";
        private const string AddScoreURL = "http://www.andrejmueller.com/highscoresIML/addscore.php?";
        private const string HighscoreURL = "http://www.andrejmueller.com/highscoresIML/getscores.php?";
        public static bool ReadHighScoresFinished { private set; get; }
        public static List<HighScoreEntry> Result = new List<HighScoreEntry>();

        public static void PostScores(string userName,
            int score,
            string comment,
            string tsp = AntAlgorithmManager.TspFileName,
            int algorithm = 1,
            string game = AntAlgorithmManager.GameName)
        {
            string url = AddScoreURL.TrimEnd('?');
            var postValues = new Dictionary<string, string>();
            postValues["name"] = userName;
            postValues["score"] = score.ToString();
            postValues["tsp"] = tsp;
            postValues["hash"] = Hash(SecretKey);
            postValues["algorithm"] = algorithm.ToString();
            postValues["game"] = game;
            postValues["comment"] = comment;
            var hsPost = WebFunctions.Post(url, postValues);

            print("HSPOST " + hsPost.url);

            if (!string.IsNullOrEmpty(hsPost.error))
            {
                print("There was an error posting the high score: " + hsPost.error);
            }
        }

        public static IEnumerator PostScoresAsync(string userName, 
                                                  int score,
                                                  string comment,
                                                  string tsp = AntAlgorithmManager.TspFileName,
                                                  int algorithm = 1,
                                                  string game = AntAlgorithmManager.GameName)
        {
            string url = AddScoreURL
                             + "name=" + WWW.EscapeURL(userName)
                             + "&score=" + score
                             + "&tsp=" + WWW.EscapeURL(tsp)
                             + "&hash=" + WWW.EscapeURL(Hash(SecretKey))
                             + "&algorithm=" + algorithm
                             + "&game="+ WWW.EscapeURL(game)
                             + "&comment=" + WWW.EscapeURL(comment);
            print(url);
            WWW hsPost = new WWW(url);
            yield return hsPost;
            print("HSPOST " + hsPost.url);

            if (!string.IsNullOrEmpty(hsPost.error))
            {
                print("There was an error posting the high score: " + hsPost.error);
            }
        }

        public static List<HighScoreEntry> GetScores(string tspName = AntAlgorithmManager.TspFileName,
            string gameName = AntAlgorithmManager.GameName,
            int numberOfEntries = AntAlgorithmManager.NumHighScoreEntries)
        {
            Debug.Log("Retrieving High Scores...");
            ReadHighScoresFinished = false;
            Result.Clear();
            var url = HighscoreURL
                      + "tsp=" + WWW.EscapeURL(tspName)
                      + "&num=" + numberOfEntries
                      + "&game=" + WWW.EscapeURL(gameName);
            print(url);
            WWW hsGet = WebFunctions.Get(url);

            if (!string.IsNullOrEmpty(hsGet.error))
            {
                print("There was an error getting the high score: " + hsGet.error);
            }
            else
            {
                foreach (var line in hsGet.text.Split(new[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var entry = HighScoreEntry.Create(line);
                    if(entry != null)
                        Result.Add(entry);
                }
            }
            ReadHighScoresFinished = true;
            return Result;
        }

        public static IEnumerator GetScoresAsync(string tspName = AntAlgorithmManager.TspFileName,
                                string gameName = AntAlgorithmManager.GameName, 
                                int numberOfEntries = AntAlgorithmManager.NumHighScoreEntries)
        {
            Debug.Log("Retrieving High Scores...");
            ReadHighScoresFinished = false;
            var url = HighscoreURL
                      + "tsp=" + WWW.EscapeURL(tspName)
                      + "&num=" + numberOfEntries
                      + "&game=" + gameName;
            print(url);
            WWW hsGet = new WWW(url);
            yield return hsGet;

            if (!string.IsNullOrEmpty(hsGet.error))
            {
                print("There was an error getting the high score: " + hsGet.error);
            }
            else
            {
                print(hsGet.text);
                foreach (var line in hsGet.text.Split(new []{"<br>"}, StringSplitOptions.RemoveEmptyEntries))
                {
                    var entry = HighScoreEntry.Create(line);
                    Result.Add(entry);
                }
            }
            ReadHighScoresFinished = true;
            yield return Result;
        }

        private static string Hash(string password)
        {
            return BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(password)))
                .Replace("-", string.Empty).ToUpper();
        }
    }
}
