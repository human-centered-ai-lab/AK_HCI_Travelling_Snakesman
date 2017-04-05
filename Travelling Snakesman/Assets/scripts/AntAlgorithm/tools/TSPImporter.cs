/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* TSPImporter is a simple importer for .tsp files (http://www.iwr.uni-heidelberg.de/groups/comopt/software/TSPLIB95/) */

using UnityEngine;
using System.Collections.Generic;
using System;

public static class TSPImporter
{

    private static string tspLibFolderName = "tspLib";
    private static string pointSection = "NODE_COORD_SECTION";

    public static List<City> importTsp(string fileName)
    {
        string line;
        int count = 0;
        List<City> cities = new List<City>();
        System.IO.StreamReader file =
           new System.IO.StreamReader(Application.dataPath + "/" + tspLibFolderName + "/" + fileName);
        while ((line = file.ReadLine()) != null)
        {
            if (line == pointSection)
                break;
        }
        while ((line = file.ReadLine()) != null)
        {
            char[] delimiterChars = { ' ', ':', '\t' };
            string[] cityParameter = line.Split(delimiterChars);
            if (cityParameter.Length == 3)
            {
                cities.Add(new City(Int32.Parse(cityParameter[1]), Int32.Parse(cityParameter[2]), count, "city " + count, new GameObject()));
            }
            count++;
        }
        file.Close();
        return cities;
    }

}
