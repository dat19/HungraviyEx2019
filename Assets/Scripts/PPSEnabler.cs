using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace HungraviyEx2019
{
    public class PPSEnabler : MonoBehaviour
    {
        private void Awake()
        {

            PostProcessLayer ppl;
            ppl = GetComponent<PostProcessLayer>();

#if UNITY_ANDROID
            string st = SystemInfo.graphicsDeviceType.ToString().ToLower();
            if (st != "opengles2")
            {
                if (ppl != null)
                {
                    ppl.enabled = true;
                }
                Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier3;
            }
            else
            {
                if (ppl != null)
                {
                    ppl.enabled = false;
                }
                Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier1;
            }
#else
            if (ppl != null)
            {
                ppl.enabled = true;
            }
#endif
        }

    }
}