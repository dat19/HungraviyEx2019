using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class Blackhole : MonoBehaviour
    {
        [Tooltip("最高速度"), SerializeField]
        float speedMax = 10f;

        public static bool CanMove
        {
            get
            {
                return !Fade.IsFading;
            }
        }

        public static bool IsSpawn { get; private set; }

        Rigidbody2D rb;
        Animator anim;
        float Energy= 100f;
        Vector3 targetPosition;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            if (!CanMove) return;


        }


    }
}