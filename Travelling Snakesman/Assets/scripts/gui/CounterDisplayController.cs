using UnityEngine;
using UnityEngine.UI;

namespace gui
{
    public class CounterDisplayController : MonoBehaviour
    {
        [SerializeField] private Text counterLabel;
        private int _foodEated;
        public int maxFood { get; set; }
        private bool IsInitialized { get; set; }

        // Use this for initialization
        void Start ()
        {
            counterLabel.text = "0.00";
            _foodEated = 0;
            
            maxFood = 0; // initialised by FoodController
        }
	
        // Update is called once per frame
        void Update ()
        {
            counterLabel.text = _foodEated.ToString() + " / " + maxFood.ToString();
        }

        public void UpdateRange(int count)
        {
            _foodEated = count;
        }
    }
}
