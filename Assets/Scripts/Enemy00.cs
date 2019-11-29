using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class Enemy00 : MonoBehaviour
    {
        [Tooltip("歩く速度"), SerializeField]
        float walkSpeed = 1f;
        [Tooltip("足元の座標"), SerializeField]
        Vector2 footOffset = new Vector2(-1f, -1.5f);
        [Tooltip("足元チェックの距離"), SerializeField]
        float footCheckDistance = 0.2f;
        [Tooltip("歩き速度とアニメの係数"), SerializeField]
        float velocityToAnimSpeed = 0.75f;

        /// <summary>
        /// 行動状態の種類
        /// </summary>
        public enum StateType
        {
            Move,
            Blackhole,
            BlackholeReverse,
        }

        /// <summary>
        /// アニメ状態の種類。Stateに対応
        /// </summary>
        public enum AnimType
        {
            Stand,
            Walk,
            Sucked,
            Blackhole,
        }

        /// <summary>
        /// 衝突の上限数
        /// </summary>
        const int HitMax = 4;

        public bool CanMove
        {
            get
            {
                return !Fade.IsFading;
            }
        }

        StateType state = StateType.Move;
        Rigidbody2D rb = null;
        Animator anim = null;

        RaycastHit2D[] hits = new RaycastHit2D[HitMax];
        CapsuleCollider2D capCollider = null;
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        SpriteRenderer spRenderer = null;
        ContactPoint2D[] contactPoints = new ContactPoint2D[HitMax];

        private void Awake()
        {
            state = StateType.Move;
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
            anim.SetInteger("State", (int)AnimType.Stand);

            capCollider = GetComponent<CapsuleCollider2D>();
            contactFilter2D.layerMask = LayerMask.GetMask("Map");
            spRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (!CanMove)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            UpdateMove();
        }

        void UpdateMove()
        {
            bool onGround = false;

            // 着地チェック。上向きの接触があれば着地
            int hitCount = capCollider.GetContacts(contactFilter2D, contactPoints);
            for (int i=0; i<hitCount;i++)
            {
                if (contactPoints[i].normal.y >= 0.9f)
                {
                    onGround = true;
                }
            }

            if (onGround)
            {
                // 足場がある時は移動
                anim.SetInteger("State", (int)AnimType.Walk);

                // 反転チェック
                Vector3 offset = footOffset;
                if (spRenderer.flipX)
                {
                    offset.x = -offset.x;
                }
                var footPos = transform.position + offset;
                hitCount = Physics2D.RaycastNonAlloc(
                    footPos,
                    Vector2.down,
                    hits,
                    footCheckDistance,
                    LayerMask.GetMask("Map"));
                if (hitCount == 0)
                {
                    spRenderer.flipX = !spRenderer.flipX;
                }
                anim.speed = Mathf.Abs(rb.velocity.x) * velocityToAnimSpeed;

                Vector2 v = rb.velocity;
                v.x = spRenderer.flipX ? walkSpeed : -walkSpeed;
                rb.velocity = v;
            }
            else
            {
                // 足場がない時は、アニメをStandに変更して慣性移動
                anim.SetInteger("State", (int)AnimType.Stand);
            }
        }
    }
}