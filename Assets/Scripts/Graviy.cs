using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019 {
    public class Graviy : MonoBehaviour
    {
        public static Graviy Instance = null;

        [Tooltip("重力係数"), SerializeField]
        float gravityScale = 1f;

        public enum AnimType
        {
            Idle,
            Sucked,
            Fall,
        }

        /// <summary>
        /// 無敵秒数
        /// </summary>
        static float mutekiTime = 0f;

        static Rigidbody2D rb = null;
        static Animator anim = null;
        static CapsuleCollider2D capsuleCollider2D = null;
        static Camera mainCamera = null;
        static SpriteRenderer spRenderer = null;
        static Suiyose suiyose = null;

        /// <summary>
        /// 移動可能かどうかのフラグ
        /// </summary>
        public static bool CanMove
        {
            get
            {
                return !Fade.IsFading;
            }
        }

        void Awake()
        {
            Instance = this;
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
            anim.SetInteger("State", (int)AnimType.Idle);
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            spRenderer = GetComponentInChildren<SpriteRenderer>();
            suiyose = GetComponent<Suiyose>();
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

        void FixedUpdate()
        {
            if (!CanMove)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            if (suiyose.Suck())
            {
                // 吸い寄せられている
                spRenderer.flipX = Blackhole.Instance.transform.position.x < transform.position.x;
                anim.SetInteger("State", (int)AnimType.Sucked);
            }
            else
            {
                // 吸い寄せられていない時
                if (rb.velocity.y >= 0)
                {
                    anim.SetInteger("State", (int)AnimType.Idle);
                }
                else
                {
                    anim.SetInteger("State", (int)AnimType.Fall);
                }
            }

        }

        public void AdjustLeftPosition()
        {
            // 当たり判定の左端座標
            var left = capsuleCollider2D.bounds.min;

            // ビューポート座標に変換
            var vpos = mainCamera.WorldToViewportPoint(left);

            // 負の値なら補正
            if (vpos.x < 0f)
            {
                var target = mainCamera.ViewportToWorldPoint(Vector3.zero);
                var adjust = transform.position;
                adjust.x = target.x + capsuleCollider2D.bounds.extents.x;
                transform.position = adjust;
            }
        }
    }
}