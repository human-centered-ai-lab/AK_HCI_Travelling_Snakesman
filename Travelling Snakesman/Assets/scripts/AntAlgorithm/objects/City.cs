/****************************************************
 * IML ACO implementation for TSP 
 * More information: http://hci-kdd.org/project/iml/
 * Author: Andrej Mueller
 * Year: 2017
 *****************************************************/

/* City represents a city in the TSP */

public class City
{
    public City(int xPosition, int yPosition, int id)
    {
        Id = id;
        XPosition = xPosition;
        YPosition = yPosition;
    }

    public int Id { get; private set; }

    public int XPosition { get; set; }

    public int YPosition { get; set; }
}