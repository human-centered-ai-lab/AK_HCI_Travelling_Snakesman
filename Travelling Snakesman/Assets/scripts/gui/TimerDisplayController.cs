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

            var minutes = Mathf.Floor(Time / 60);
            var seconds = Time % 60;

            timerLabel.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
