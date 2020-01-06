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
        [Tooltip("背景の上限オフセット"), SerializeField]
        float bgTop = 0.18f;
        [Tooltip("背景の下限オフセット"), SerializeField]
        float bgBottom = -0.18f;

        /// <summary>
        /// カメラの移動範囲
        /// </summary>
        float cameraRange;

        /// <summary>
        /// カメラ座標から背景座標への変換率
        /// </summary>
        float camToBGRate;

        Material [] bgMaterials;

        void Start()
        {
            cameraRange = cameraTop - cameraBottom;
            camToBGRate = (bgTop - bgBottom) / (cameraRange);
            Renderer[] rends = targetBG.GetComponentsInChildren<Renderer>();
            bgMaterials = new Material[rends.Length];
            for (int i=0; i<rends.Length;i++)
            {
                bgMaterials[i] = rends[i].material;
            }
        }

        public void UpdateBGPosition()
        {
            Vector2 ofs = Vector2.zero;

            // X
            ofs.x = transform.position.x*camToBGRate;

            // Y
            float t = (transform.position.y-cameraBottom)/ cameraRange;
            ofs.y = Mathf.Lerp(bgBottom, bgTop, t);

            for (int i=0;i<bgMaterials.Length;i++)
            {
                bgMaterials[i].mainTextureOffset = ofs;
            }
        }
    }
}