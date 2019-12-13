using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    [RequireComponent(typeof(Suiyose))]
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
        [Tooltip("出現させるスイーツ"), SerializeField]
        GameObject sweets = null;

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

        readonly RaycastHit2D[] hits = new RaycastHit2D[HitMax];
        CapsuleCollider2D capCollider = null;
        SpriteRenderer spRenderer = null;
        Suiyose suiyose = null;
        bool lastSucked = false;
        EnemyInBlackhole enemyInBlackhole = null;

        private void Awake()
        {
            state = StateType.Move;
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
            anim.SetInteger("State", (int)AnimType.Stand);

            capCollider = GetComponent<CapsuleCollider2D>();
            spRenderer = GetComponentInChildren<SpriteRenderer>();
            suiyose = GetComponent<Suiyose>();
            enemyInBlackhole = GetComponentInChildren<EnemyInBlackhole>();
        }

        private void FixedUpdate()
        {
            if (!CanMove)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            switch (state)
            {
                case StateType.Move:
                    // 縮小チェック
                    EnemyInBlackhole.StateType instate = enemyInBlackhole.FixedUpdateState();
                    if (instate == EnemyInBlackhole.StateType.In)
                    {
                        anim.SetInteger("State", (int)AnimType.Blackhole);
                        anim.SetFloat("Speed", 1);
                        state = StateType.Blackhole;

                        suiyose.Suck();
                        lastSucked = true;
                        break;
                    }
                    // 吸い寄せされていたら、歩きはキャンセルして、吸い寄せ
                    if (suiyose.Suck())
                    {
                        lastSucked = true;
                        anim.SetInteger("State", (int)AnimType.Sucked);
                        break;
                    }

                    if (lastSucked)
                    {
                        // 吸い寄せられていたら、歩き速度未満になるまでは歩きに復帰しない
                        float spd = rb.velocity.magnitude;
                        if (spd >= walkSpeed)
                        {
                            break;
                        }
                    }
                    lastSucked = false;
                    UpdateMove();
                    break;

                case StateType.Blackhole:
                    lastSucked = true;

                    // 吸い込まれのチェック
                    EnemyInBlackhole.StateType st = enemyInBlackhole.FixedUpdateState();
                    if (st == EnemyInBlackhole.StateType.InDone)
                    {
                        // 吸い込まれが完了した
                        ToSweets();
                        Destroy(gameObject);
                    }
                    else if (st == EnemyInBlackhole.StateType.None)
                    {
                        // 戻った
                        state = StateType.Move;
                        UpdateMove();
                    }

                    break;
            }
        }


        void UpdateMove()
        {
            if (OnGroundChecker.Check(capCollider))
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
                var hitCount = Physics2D.RaycastNonAlloc(
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

        /// <summary>
        /// スイーツを出現させる
        /// </summary>
        void ToSweets()
        {
            Instantiate(sweets, transform.position, Quaternion.identity);
        }
    }
}