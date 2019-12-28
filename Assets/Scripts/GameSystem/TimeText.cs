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
            int sec = Mathf.FloorToInt(GameParams.playTime);
            int milli = Mathf.FloorToInt(GameParams.playTime * 100f);
            timeText.text = $"{sec}{FontSize}.{(milli - sec * 100f):00}</size>";
        }
    }
}