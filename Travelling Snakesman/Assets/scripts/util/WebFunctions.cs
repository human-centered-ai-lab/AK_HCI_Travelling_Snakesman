using System.Collections.Generic;
using UnityEngine;

namespace util
{
    public class WebFunctions
    {
        public static WWW Get(string url)
        {
            WWW www = new WWW(url);

            WaitForSeconds w;
            while (!www.isDone)
                w = new WaitForSeconds(0.1f);

            return www;
        }

        public static WWW Post(string url, Dictionary<string, string> post = null)
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

    }
}
