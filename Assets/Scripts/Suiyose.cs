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
        /// 重力の最小レート
        /// </summary>
        const float gravityRate = 0.8f;

        /// <summary>
        /// 吸い寄せられたフラグ
        /// </summary>
        public bool isSucked = false;

        Rigidbody2D rb = null;
        /// <summary>
        /// 処理したフレーム数。すでに同じフレームが設定されていたら、別で処理済みなので処理しない
        /// </summary>
        int procFrame = 0;
        Collider2D myCollider = null;
        float defaultGravityScale = 0;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            myCollider = GetComponent<Collider2D>();
            defaultGravityScale = rb.gravityScale;
        }

        private void FixedUpdate()
        {
            if (!Graviy.CanMove)
            {
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
                var bl = Blackhole.instance.transform;
                Vector2 move = bl.position - myCollider.bounds.center;

                var kyori = move.magnitude;
                if (kyori <= distanceMax)
                {
                    // 高さによる重さ変更
                    var distY = Mathf.Abs(bl.position.y - myCollider.bounds.center.y);
                    distY = Mathf.Max(0f, distY - myCollider.bounds.extents.y);
                    var len = distanceMax - myCollider.bounds.extents.y;
                    var yRate = distY / len;
                    rb.gravityScale = Mathf.Lerp(defaultGravityScale * gravityRate, defaultGravityScale, yRate);

                    // 距離による加速
                    var kasoku = (-speedMax / distanceMax) * kyori + speedMax;
                    rb.AddForce(move.normalized * kasoku, ForceMode2D.Force);
                    isSucked = true;
                }
            }

            if (!isSucked)
            {
                rb.gravityScale = defaultGravityScale;
            }

            return isSucked;
        }
    }
}