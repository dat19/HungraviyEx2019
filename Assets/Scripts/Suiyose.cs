//#define DEBUG_FPS

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
        [Tooltip("自分でSuck()を呼び出す時、チェックを入れます"), SerializeField]
        bool isSelfSuck = false;

        /// <summary>
        /// 重力の最小レート
        /// </summary>
        const float gravityRate = 0.8f;

        /// <summary>
        /// 吸い寄せられたフラグ
        /// </summary>
        [HideInInspector]
        public bool isSucked = false;

        Rigidbody2D rb = null;
        Collider2D myCollider = null;
        float defaultGravityScale = 0;

#if DEBUG_FPS
        float debugFps = 0;
        float lastSuckTime = 0;
        int lastFrameCount = 0;
#endif

        AudioSource myAudioSource = null;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            myCollider = GetComponent<Collider2D>();
            defaultGravityScale = rb.gravityScale;
            myAudioSource = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            if (!isSelfSuck || !Graviy.CanMove)
            {
                if (myAudioSource != null)
                {
                    myAudioSource.Stop();
                }
                return;
            }

            Suck();
        }

        public bool Suck()
        {
            // ブラックホールが発生しているか判定
            isSucked = false;
            if (Blackhole.IsSpawn)
            {
                var bl = Blackhole.instance.transform;
                Vector2 move = bl.position - myCollider.bounds.center;

                var kyori = move.magnitude;
                if (kyori <= distanceMax)
                {
                    // 効果音調整
                    if (myAudioSource != null)
                    {
                        myAudioSource.volume = (1f-(kyori / distanceMax));
                        if (!myAudioSource.isPlaying)
                        {
                            myAudioSource.Play();
                        }
                    }

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

#if DEBUG_FPS
                    if (Mathf.Approximately(Time.time, lastSuckTime))
                    {
                        debugFps = -1;
                    }
                    else
                    {
                        debugFps = 1f / (float)(Time.time - lastSuckTime);
                    }
                    lastSuckTime = Time.time;
#endif

                }
                else
                {
                    if (myAudioSource != null)
                    {
                        myAudioSource.Stop();
                    }
                }
            }
            else
            {
                if (myAudioSource != null)
                {
                    myAudioSource.Stop();
                }
            }

            if (!isSucked)
            {
                rb.gravityScale = defaultGravityScale;
            }

            return isSucked;
        }

#if DEBUG_FPS
        private void OnGUI()
        {
            if ((lastFrameCount == Time.frameCount)
                || Mathf.Approximately(debugFps, 0)) return;

            lastFrameCount = Time.frameCount;
            GUI.color = Color.red;
            GUI.Label(new Rect(20, 100, 200, 50), debugFps.ToString());//, GameParams.LabelSkin);
        }
#endif
    }
}