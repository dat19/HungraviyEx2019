using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class MainCameraEnabler : MonoBehaviour
    {
        private void Awake()
        {
            SubCameraManager.SetEnable(false);
        }

        private void OnDestroy()
        {
            GetComponent<Camera>().enabled = false;
            SubCameraManager.SetEnable(true);            
        }
    }
}