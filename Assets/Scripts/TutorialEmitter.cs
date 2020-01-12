using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class TutorialEmitter : MonoBehaviour
    {
        [Tooltip("接触したら発動させるチュートリアルインデックス"), SerializeField]
        int index = 0;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (Tutorial.requestIndex < index)
                {
                    Tutorial.requestIndex = index;
                }
            }
        }
    }
}