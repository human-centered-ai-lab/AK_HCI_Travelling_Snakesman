using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    public class WebFunctions
    {
        public bool highscoreLoaded = false;
        public bool highscoreUploaded = false;

        public static WWW Get(string url)
        {
            WWW www = new WWW(url);

            WaitForSeconds w;
            while (!www.isDone)
                w = new WaitForSeconds(0.1f);

            return www;
        }

        public IEnumerator GetWebGL(string url)
        {
            highscoreLoaded = false;
            WWW www = new WWW(url);

            yield return www;
            highscoreLoaded = true;
        }


        public static WWW Post(string url, Dictionary<string, string> post)
        {
            WWWForm form = new WWWForm();
            if (post != null)
            {
                foreach (var pair in post)
                    form.AddField(pair.Key, pair.Value);
            }

            WWW www = new WWW(url, form);

            WaitForSeconds w;
            while (!www.isDone)
                w = new WaitForSeconds(0.1f);
            return www;
        }



        public IEnumerator PostWebGL(string url, Dictionary<string, string> post)
        {
            highscoreUploaded = false;
            WWWForm form = new WWWForm();
            if (post != null)
            {
                foreach (var pair in post)
                    form.AddField(pair.Key, pair.Value);
            }

            WWW www = new WWW(url, form);

            yield return www;

            Debug.Log("HSPOST " + www.url);

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("There was an error posting the high score: " + www.error);
            }

            highscoreUploaded = true;
        }

    }
}
