using UnityEngine;
using UnityEngine.UI;

namespace gui
{
    public class TimerDisplayController : MonoBehaviour
    { 
        [SerializeField] private Text timerLabel;
        private float _time;
        // Update is called once per frame
        void Update ()
        {
            if(AntAlgorithmManager.Instance.IsGameFinished)
                return;

            _time += Time.deltaTime;

            var minutes = _time / 60;
            var seconds = _time % 60;
            var fraction = _time * 1000 % 1000;

            timerLabel.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        }
    }
}
