/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* TSPImporter is a simple importer for .tsp files (http://www.iwr.uni-heidelberg.de/groups/comopt/software/TSPLIB95/) */

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AntAlgorithm.tools
{
    // ReSharper disable once InconsistentNaming
    public static class TSPImporter
    {
        private const string TspLibFolderName = "tspLib";
        private const string PointSection = "NODE_COORD_SECTION";

        public static List<City> ImportTsp(string fileName)
        {
            string line;
            int count = 0;
            List<City> cities = new List<City>();

            var file = new StreamReader(Application.dataPath + Path.AltDirectorySeparatorChar + TspLibFolderName +
                                        Path.AltDirectorySeparatorChar + fileName);

            while ((line = file.ReadLine()) != null)
            {
                if (line == PointSection)
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
}
