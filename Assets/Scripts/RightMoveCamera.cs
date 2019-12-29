using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    [RequireComponent(typeof(BGScroller))]
    public class RightMoveCamera : MonoBehaviour
    {
        /// <summary>
        /// プレイヤーからカメラの座標を求めるためのオフセット値
        /// </summary>
        static Vector3 offset;

        static BGScroller bgScroller = null;

        void Start()
        {
            offset = transform.position - Graviy.instance.transform.position;
            bgScroller = GetComponent<BGScroller>();
        }

        void LateUpdate()
        {
            Vector3 next = Graviy.instance.transform.position + offset;
            if (next.x < transform.position.x)
            {
                next.x = transform.position.x;
            }
            transform.position = next;

            Graviy.instance.AdjustLeftPosition();
            bgScroller.UpdateBGPosition();
        }
    }
}