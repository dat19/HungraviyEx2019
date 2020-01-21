using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class SubCameraManager : MonoBehaviour
    {
        public static SubCameraManager instance = null;

        static Camera myCamera = null;

        private void Awake()
        {
            instance = this;
            myCamera = GetComponent<Camera>();
        }

        public static void SetEnable(bool flag)
        {
            if (myCamera != null)
            {
                myCamera.enabled = flag;
            }
        }
    }
}