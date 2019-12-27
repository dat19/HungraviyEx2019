using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HungraviyEx2019
{
    public class ScoreText : MonoBehaviour
    {
        int lastScore = -1;
        TextMeshProUGUI scoreText = null;

        private void Awake()
        {
            scoreText = GetComponent<TextMeshProUGUI>();
        }

        void LateUpdate()
        {
            if (lastScore != GameParams.Score)
            {
                lastScore = GameParams.Score;
                scoreText.text = GameParams.Score.ToString("000000");
            }
        }
    }
}