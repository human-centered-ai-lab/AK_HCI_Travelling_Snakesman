using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringOperations {

    public static int GetTimeFromString(string time, bool withMillis)
    {
        if (!withMillis)
        {
            string[] timeStrings = time.Split(':');
            return (Int32.Parse(timeStrings[0]) * 60 + Int32.Parse(timeStrings[1]));
        }else
        {
            return 0;
        }
    }
    public static string GetStringFromTime(float time, bool withMillis)
    {
        if (!withMillis)
        {
            var minutes = Mathf.Floor(time / 60);
            var seconds = Mathf.Floor(time % 60);

            return string.Format("{0}:{1}", minutes.ToString("0"), seconds.ToString("00"));
        }
        else
        {
            return "";
        }
    }
}
