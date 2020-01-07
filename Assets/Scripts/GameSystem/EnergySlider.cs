using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HungraviyEx2019
{
    public class EnergySlider : MonoBehaviour
    {
        Slider energySlider = null;

        void Awake()
        {
            energySlider = GetComponent<Slider>();
        }

        private void LateUpdate()
        {
            energySlider.value = Mathf.Clamp01(Graviy.Energy / Graviy.EnergyMax);
        }
    }
}
