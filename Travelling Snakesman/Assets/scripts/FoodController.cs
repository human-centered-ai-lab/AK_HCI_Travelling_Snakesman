using System.Collections.Generic;
using AntAlgorithm;
using util;
using UnityEngine;
using gui;

public class FoodController : MonoBehaviour
{
    private static GameObject _foodPrefab;
    private static GameObject _arrowPrefab;

    public int Id { get; private set; }

	public static void InitializeFoodPositions(uint gameboardSize, bool rotateGameBoard = true)
    {
        //get min and max values of x and y coordinates, so that we can calculate normalized values
        int minX, maxX, minY, maxY;

        var cities = AntAlgorithmManager.Instance.Cities;

        GetExtremeCityCoordinates(cities, out minX, out maxX, out minY, out maxY);

        int absoluteMaxX = Mathf.Max(Mathf.Abs(minX), Mathf.Abs(maxX));
        int absoluteMaxY = Mathf.Max(Mathf.Abs(minY), Mathf.Abs(maxY));
		int angleToRotate = rotateGameBoard ? Random.Range (0, 359) : 0;
		// Debug.Log ("Game board is rotated, so that player is not bored when playing with same town more often. Angle to rotate: " + angleToRotate);

        //initialize food objects with normalized values
        foreach (var city in cities)
        {
            float xPos = (city.XPosition / (float)absoluteMaxX) * gameboardSize;
            float yPos = (city.YPosition / (float)absoluteMaxY) * gameboardSize;
			var pos = Quaternion.Euler (0, 0, angleToRotate) * new Vector3(xPos, yPos, 0);

            Create(pos, city.Id);
        }
    }

    public static FoodController Create(Vector3 position, int id)
    {
        if (_foodPrefab == null)
        {
            _foodPrefab = Resources.Load("Prefabs/FoodPrefab") as GameObject;
        }
        if (_arrowPrefab == null)
            _arrowPrefab = Resources.Load("Prefabs/ArrowPointAtObject") as GameObject;

        var foodGameObject = Instantiate(_foodPrefab, position, Quaternion.identity);
		foodGameObject.name = "foodGameObject_" + id;
        var controller = foodGameObject.transform.GetOrAddComponent<FoodController>();
        controller.Id = id;

        var arrowThatPointsToFood = Instantiate(_arrowPrefab, Vector3.zero, Quaternion.identity);
        arrowThatPointsToFood.transform.parent = Camera.main.transform;
        arrowThatPointsToFood.GetComponent<PointAtObject>().objectToPointAt = foodGameObject;

        AntAlgorithmManager.Instance.RegisterFood(id, foodGameObject);
        return controller;
    }

	public void Redye(float redyeFactor = 1.0f)
	{
		if (redyeFactor < 0.4f) //no item should be completely black
		{
			redyeFactor = 0.4f;
		}

		gameObject.GetComponent<SpriteRenderer>().color = new Color (redyeFactor, redyeFactor, redyeFactor);
	}
    
    public void OnDestroy()
    {
        AntAlgorithmManager.Instance.UnregisterEatenFood(Id);
    }

    #region Helper Methods

    private static void GetExtremeCityCoordinates(IEnumerable<City> cities, out int minX, out int maxX, out int minY, out int maxY)
    {
        minX = minY = int.MaxValue;
        maxX = maxY = int.MinValue;

        foreach (var city in cities)
        {
            var x = city.XPosition;
            var y = city.YPosition;

            if (x < minX)
                minX = x;
            if (x > maxX)
                maxX = x;
            if (y < minY)
                minY = y;
            if (y > maxY)
                maxY = y;
        }
    }
    #endregion
}
