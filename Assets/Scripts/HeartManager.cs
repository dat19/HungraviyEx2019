using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class HeartManager : MonoBehaviour
    {
        [Tooltip("ハートオブジェクト"), SerializeField]
        Heart[] hearts = null;
        [Tooltip("ハードを出現させる秒数"), SerializeField]
        float heartOnInterval = 0.1f;

        int lastHeartCount = 0;

        void Start()
        {
            lastHeartCount = GameParams.Life;
            StartCoroutine(HeartOn());
        }

        IEnumerator HeartOn()
        {
            WaitForSeconds wait = new WaitForSeconds(heartOnInterval);

            for (int i=0; i< GameParams.Life; i++)
            {
                yield return wait;
                hearts[i].On();
            }
        }

        public void Update()
        {
            if (lastHeartCount != GameParams.Life)
            {
                for (int i=0; i<hearts.Length;i++)
                {
                    if (i < GameParams.Life)
                    {
                        hearts[i].On();
                    }
                    else
                    {
                        hearts[i].Off();
                    }
                }
                lastHeartCount = GameParams.Life;
            }
        }

    }
}