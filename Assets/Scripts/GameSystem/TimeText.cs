using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HungraviyEx2019
{
    public class TimeText : MonoBehaviour
    {
        TextMeshProUGUI timeText = null;
        const string FontSize = "<size=75>";

        private void Awake()
        {
            timeText = GetComponent<TextMeshProUGUI>();
        }

        void LateUpdate()
        {
            int sec = Mathf.FloorToInt(GameParams.PlayTime);
            int milli = Mathf.FloorToInt(GameParams.PlayTime * 100);
            timeText.text = $"{sec}{FontSize}.{(milli - sec * 100):00}</size>";
        }
    }
}