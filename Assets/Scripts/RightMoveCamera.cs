using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class RightMoveCamera : MonoBehaviour
    {
        /// <summary>
        /// プレイヤーからカメラの座標を求めるためのオフセット値
        /// </summary>
        static Vector3 offset;

        void Start()
        {
            offset = transform.position - Graviy.Instance.transform.position;
        }

        void LateUpdate()
        {
            Vector3 next = Graviy.Instance.transform.position + offset;
            if (next.x < transform.position.x)
            {
                next.x = transform.position.x;
            }
            transform.position = next;

            Graviy.Instance.AdjustLeftPosition();
        }
    }
}