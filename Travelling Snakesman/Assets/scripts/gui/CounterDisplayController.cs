using UnityEngine;
using UnityEngine.UI;

namespace gui
{
    public class CounterDisplayController : MonoBehaviour
    {
        [SerializeField] private Text counterLabel;
        private int _foodEated;
        public int MaxFood { get; set; }

        // Use this for initialization
        void Start ()
        {
            counterLabel.text = "0.00";
            _foodEated = 0;
        }
	
        // Update is called once per frame
        void Update ()
        {
            MaxFood = AntAlgorithmManager.Instance.Cities.Count;
            counterLabel.text = _foodEated.ToString() + " / " + MaxFood.ToString();
        }

        public void UpdateRange(int count)
        {
            _foodEated = count;
        }
    }
}
