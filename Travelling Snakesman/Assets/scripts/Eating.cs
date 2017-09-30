using gui;
using UnityEngine;

public class Eating : MonoBehaviour
{
    public int maxSnakeLength = 15;
    public GameObject snakeBodyPrefab;
    private int currentSnakeLength = 3;
    private GameObject lastSnakeBodyPart;
    private AudioSource audioSource;
    private int _foodEated;

    private RangeDisplayController _rangeDisplayController;
    private CounterDisplayController _counterDisplayController;

    void Start()
    {
        lastSnakeBodyPart = GameObject.Find("snake_body_2");
        audioSource = GetComponent<AudioSource>();
        _rangeDisplayController = GameObject.FindGameObjectWithTag("RangeDisplay").GetComponent<RangeDisplayController>();
        _counterDisplayController = GameObject.FindGameObjectWithTag("CounterDisplay").GetComponent<CounterDisplayController>();
        _foodEated = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Food"))
        {
            return;
        }

        _foodEated++;

        Vector3 realCityCoordinates = getRealCityCoordinatesOfFoodGameObject(other);

        //_rangeDisplayController.UpdateRange(other.gameObject.transform.position); //gameObject coordinates

        _counterDisplayController.UpdateRange(_foodEated);

        Destroy(other.gameObject);

        audioSource.Play();
        if (currentSnakeLength >= maxSnakeLength)
        {
            return;
        }

        //create new snake body part like defined prefab
        Vector3 pos = new Vector3(-100.0f, -100.0f, 0);
        GameObject newSnakeBodyPart = Instantiate(snakeBodyPrefab, pos, Quaternion.identity);
        newSnakeBodyPart.name = "snake_body_" + currentSnakeLength;
        newSnakeBodyPart.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
        newSnakeBodyPart.GetComponent<SpriteRenderer>().sortingOrder = -currentSnakeLength;

        //new part should follow last part
        var followGameObject = newSnakeBodyPart.GetComponent<FollowGameObjectThatProvidesLastPosition>();
        followGameObject.objectToFollow = lastSnakeBodyPart;

        //we have a new last part
        lastSnakeBodyPart = newSnakeBodyPart;
        currentSnakeLength++;
    }

    //VERY dirty code to get real coordinates of city (game coordinates are not equal to real city coordinates)
    private Vector3 getRealCityCoordinatesOfFoodGameObject(Collider2D food)
    {
        int cityID = -1;
        int.TryParse(food.name.Substring(food.name.LastIndexOf('_') + 1), out cityID);
        //Debug.Log ("cityID: " + cityID);
        //Debug.Log("food.transform.position: " + food.transform.position);
        City city = AntAlgorithmManager.Instance.Cities[cityID];
        Vector3 realCityCoordinates = new Vector3(city.XPosition, city.YPosition, 0);
        //Debug.Log ("REAL city coordinates: " + realCityCoordinates);

        return realCityCoordinates;
    }
}
