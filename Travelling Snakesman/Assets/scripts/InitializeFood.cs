using UnityEngine;

public class InitializeFood : MonoBehaviour {

	public GameObject FoodPrefab;
	public GameObject ArrowPrefab;
	public int MaxXandYValuesForFood = 30;

	// Use this for initialization
	void Start () {
        //get min and max values of x and y coordinates, so that we can calculate normalized values
        int minXPosition = int.MaxValue;
		int maxXPosition = int.MinValue;
		int minYPosition = int.MaxValue;
		int maxYPosition = int.MinValue;

	    var cities = AntAlgorithmManager.Instance.Cities;
        foreach (var city in cities) {
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
		foreach (var city in cities) {
			float xPos = ((float) city.getXPosition () / (float) absoluteMaxX) * MaxXandYValuesForFood;
			float yPos = ((float) city.getYPosition () / (float) absoluteMaxY) * MaxXandYValuesForFood;
			Vector3 pos = new Vector3(xPos, yPos,0);

			Debug.Log ("initialize food: id: " + city.getId () + ", pos: " + pos + "  / original pos: " + city.getXPosition() + "/" + city.getYPosition());

			GameObject foodGameObject = Instantiate(FoodPrefab, pos, Quaternion.identity);
			foodGameObject.name = city.getId().ToString();
			//foodGameObject.transform.localScale *= Random.Range (0.8f, 2.5f); //TODO: use real values!

			GameObject arrowThatPointsToFood = Instantiate(ArrowPrefab, Vector3.zero, Quaternion.identity);
			arrowThatPointsToFood.transform.parent = Camera.main.transform;
			arrowThatPointsToFood.GetComponent<PointAtObject> ().objectToPointAt = foodGameObject;
		}
	}
}
