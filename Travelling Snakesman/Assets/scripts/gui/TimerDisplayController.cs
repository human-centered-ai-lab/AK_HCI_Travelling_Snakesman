using UnityEngine;
using UnityEngine.UI;

namespace gui
{
    public class TimerDisplayController : MonoBehaviour
    { 
        [SerializeField] private Text timerLabel;
        public float Time { get; set; }

        // Update is called once per frame
        void Update ()
        {
            if (AntAlgorithmManager.Instance.IsGameFinished)
            {
                return;
            }

            Time += UnityEngine.Time.deltaTime;

            timerLabel.text = StringOperations.GetStringFromTime(Time * 1000);
        }
    }
}
