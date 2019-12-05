using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class Suiyose : MonoBehaviour
    {
        [Tooltip("ブラックホールの最大距離"), SerializeField]
        float distanceMax = 15f;
        [Tooltip("ブラックホールの距離が0の時の加速度"), SerializeField]
        float speedMax = 25f;

        /// <summary>
        /// 吸い寄せられたフラグ
        /// </summary>
        public bool isSucked = false;

        Rigidbody2D rb = null;
        /// <summary>
        /// 処理したフレーム数。すでに同じフレームが設定されていたら、別で処理済みなので処理しない
        /// </summary>
        int procFrame = 0;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!Graviy.CanMove)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            Suck();
        }

        public bool Suck()
        {
            if (procFrame >= Time.frameCount)
            {
                return isSucked;
            }
            procFrame = Time.frameCount;

            // ブラックホールが発生しているか判定
            isSucked = false;
            if (Blackhole.IsSpawn)
            {
                var bl = Blackhole.Instance.transform;
                Vector2 move = bl.position - transform.position;

                var kyori = move.magnitude;
                if (kyori <= distanceMax)
                {
                    var kasoku = (-speedMax / distanceMax) * kyori + speedMax;
                    rb.AddForce(move.normalized * kasoku, ForceMode2D.Force);
                    isSucked = true;
                }
            }

            return isSucked;
        }
    }
}