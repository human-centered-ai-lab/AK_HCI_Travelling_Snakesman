using System;
using UnityEngine;
using UnityEngine.UI;

namespace gui
{
    public class RangeDisplayController : MonoBehaviour
    {
        [SerializeField]
        private Image rangeLabel;
        private bool IsInitialized { get; set; }
        private Vector3 lastPosition;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (!AntAlgorithmManager.Instance.BestTourLength.Equals(null))
                if (AntAlgorithmManager.Instance.BestTourLength <= (AntAlgorithmManager.Instance.BestAlgorithmLength))
                {
                    rangeLabel.color = Color.green;
                }
                else if (AntAlgorithmManager.Instance.BestTourLength > AntAlgorithmManager.Instance.BestAlgorithmLength && AntAlgorithmManager.Instance.BestTourLength < AntAlgorithmManager.Instance.BestAlgorithmLength + (AntAlgorithmManager.Instance.BestAlgorithmLength / 50))
                {
                    rangeLabel.color = Color.yellow;
                }
                else if (AntAlgorithmManager.Instance.BestTourLength > AntAlgorithmManager.Instance.BestAlgorithmLength + (AntAlgorithmManager.Instance.BestAlgorithmLength / 50))
                {
                    rangeLabel.color = Color.red;
                }

        }
    }
}
