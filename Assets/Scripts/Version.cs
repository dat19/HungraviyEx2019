using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class Version : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = $"Ver{Application.version}";            
        }
    }
}