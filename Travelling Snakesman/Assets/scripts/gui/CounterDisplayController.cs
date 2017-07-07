//using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace gui
{
    public class CounterDisplayController : MonoBehaviour
    {
        [SerializeField] private Text counterLabel;
        private int _foodEaten;
        public int MaxFood { get; set; }

        // Use this for initialization
        void Start ()
        {
            counterLabel.text = "0.00";
            _foodEaten = 0;
        }
	
        // Update is called once per frame
        void Update ()
        {
            //Assert.NotNull(AntAlgorithmManager.Instance);
            //Assert.NotNull(AntAlgorithmManager.Instance.Cities);

            MaxFood = AntAlgorithmManager.Instance.Cities.Count;
            counterLabel.text = _foodEaten + " / " + MaxFood;
        }

        public void UpdateRange (int count)
        {
            _foodEaten = count;
        }
    }
}
