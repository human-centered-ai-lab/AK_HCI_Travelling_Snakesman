using UnityEngine;
using UnityEngine.UI;

namespace gui
{
    public class RangeDisplayController : MonoBehaviour
    {
        [SerializeField] private Text rangeLabel;
        private float range;
        private bool IsInitialized { get; set; }
        private Vector3 lastPosition;

        // Use this for initialization
        void Start ()
        {
            rangeLabel.text = "0.00";
        }
	
        // Update is called once per frame
        void Update ()
        {
			rangeLabel.text = "Travelled distance: " + range.ToString("0");
        }

        public void UpdateRange(Vector3 newPosition)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                lastPosition = newPosition;
                return;
            }
            range += Vector3.Distance(lastPosition, newPosition);
            lastPosition = newPosition;
        }
    }
}
