using UnityEngine;
using System.Collections;

public class InitializeFood : MonoBehaviour {

	public GameObject foodPrefab;
	public GameObject arrowPrefab;

	// Use this for initialization
	void Start () {

		//create food
		//TODO: currently dummy data only
		for (int i = 0; i < 30; i++) {
			Vector3 pos = new Vector3(Random.Range(-15.0f, 15.0f), Random.Range(-15.0f, 15.0f),0); //TODO: use real values
			GameObject foodGameObject = (GameObject) Instantiate(foodPrefab, pos, Quaternion.identity);
			foodGameObject.name = "food_" + i;
			foodGameObject.transform.localScale *= Random.Range (0.8f, 2.5f); //TODO: use real values

			GameObject arrowThatPointsToFood = (GameObject) Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
			arrowThatPointsToFood.transform.parent = Camera.main.transform;
			arrowThatPointsToFood.GetComponent<PointAtObject> ().objectToPointAt = foodGameObject;
		}

	}
	
	// Update is called once per frame
	//void Update () {
	//}
}
