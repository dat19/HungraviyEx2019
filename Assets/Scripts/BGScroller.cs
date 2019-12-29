using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class BGScroller : MonoBehaviour
    {
        [Tooltip("背景オブジェクト"), SerializeField]
        Transform targetBG = null;
        [Tooltip("カメラの上限"), SerializeField]
        float cameraTop = 29.5f;
        [Tooltip("カメラの下限"), SerializeField]
        float cameraBottom = -8.5f;
        [Tooltip("背景の上限"), SerializeField]
        float bgTop = 24.6f;
        [Tooltip("背景の下限"), SerializeField]
        float bgBottom = -3.6f;

        /// <summary>
        /// カメラの移動範囲
        /// </summary>
        float cameraRange;

        /// <summary>
        /// カメラ座標から背景座標への変換率
        /// </summary>
        float camToBGRate;

        void Start()
        {
            cameraRange = cameraTop - cameraBottom;                 
        }

        public void UpdateBGPosition()
        {
            Vector3 campos = targetBG.position;
            float t = (transform.position.y-cameraBottom)/ cameraRange;
            campos.y = Mathf.Lerp(bgBottom, bgTop, t);

            targetBG.position = campos;
        }
    }
}