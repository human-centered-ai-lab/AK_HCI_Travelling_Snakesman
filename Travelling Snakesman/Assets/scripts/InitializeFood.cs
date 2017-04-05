using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitializeFood : MonoBehaviour {

	public GameObject foodPrefab;
	public GameObject arrowPrefab;
	public int maxXandYValuesForFood = 30;
	public string tspFileToUse = "berlin52.tsp";

	private List<City> cities;
	private AntAlgorithms.AntAlgorithmSimple aas;

	// Use this for initialization
	void Start () {

		cities = TSPImporter.importTsp (tspFileToUse);
		aas = GetComponent<AntAlgorithms.AntAlgorithmSimple>();
		aas.setCities(cities);

		//get min and max values of x and y coordinates, so that we can calculate normalized values
		int minXPosition = int.MaxValue;
		int maxXPosition = int.MinValue;
		int minYPosition = int.MaxValue;
		int maxYPosition = int.MinValue;

		foreach (City city in cities) {
			minXPosition = city.getXPosition() < minXPosition ? city.getXPosition() : minXPosition;
			maxXPosition = city.getXPosition() > maxXPosition ? city.getXPosition() : maxXPosition;
			minYPosition = city.getYPosition() < minYPosition ? city.getYPosition() : minYPosition;
			maxYPosition = city.getYPosition() > maxYPosition ? city.getYPosition() : maxYPosition;
		}

		int absoluteMaxX = Mathf.Max (Mathf.Abs (minXPosition), Mathf.Abs (maxXPosition));
		int absoluteMaxY = Mathf.Max (Mathf.Abs (minYPosition), Mathf.Abs (maxYPosition));
		int absoluteMax = Mathf.Max (absoluteMaxX, absoluteMaxY);

		Debug.Log ("number of cities: " + cities.Count);
		Debug.Log ("X min/max: " + minXPosition + "/" + maxXPosition);
		Debug.Log ("Y min/max: " + minYPosition + "/" + maxYPosition);

		Debug.Log ("absoluteMaxX: " + absoluteMaxX);
		Debug.Log ("absoluteMaxY: " + absoluteMaxY);
		Debug.Log ("absolute max: " + absoluteMax);

		//initialize food objects with normalized values
		foreach (City city in cities) {
			float xPos = ((float) city.getXPosition () / (float) absoluteMaxX) * maxXandYValuesForFood;
			float yPos = ((float) city.getYPosition () / (float) absoluteMaxY) * maxXandYValuesForFood;
			Vector3 pos = new Vector3(xPos, yPos,0);

			Debug.Log ("initialize food: id: " + city.getId ().ToString () + ", pos: " + pos + "  / original pos: " + city.getXPosition() + "/" + city.getYPosition());

			GameObject foodGameObject = (GameObject) Instantiate(foodPrefab, pos, Quaternion.identity);
			foodGameObject.name = city.getId().ToString();
			//foodGameObject.transform.localScale *= Random.Range (0.8f, 2.5f); //TODO: use real values!

			GameObject arrowThatPointsToFood = (GameObject) Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
			arrowThatPointsToFood.transform.parent = Camera.main.transform;
			arrowThatPointsToFood.GetComponent<PointAtObject> ().objectToPointAt = foodGameObject;
		}


		//aas.init();
		//for (int i = 0; i < 3000; i++) {
		//	aas.iteration ();
		//}

		//aas.printBestTour(tspFileToUse);
	}
	
	// Update is called once per frame
	//void Update () {
	//}
}
