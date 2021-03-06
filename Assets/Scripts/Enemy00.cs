﻿using System.Collections;
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
        [Tooltip("点数"), SerializeField]
        int point = 100;
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
        int mapNameLayer;

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
            mapNameLayer = LayerMask.NameToLayer("Map");
        }

        private void Start()
        {
            if (sweets != null)
            {
                GameManager.AddItemCount();
            }
        }

        private void FixedUpdate()
        {
            if (!CanMove)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            bool isSucked = suiyose.Suck();

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

                        lastSucked = true;
                        break;
                    }
                    // 吸い寄せされていたら、歩きはキャンセルして、吸い寄せ
                    if (isSucked)
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
                        GameParams.AddScore(point);
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

                // 足場による反転チェック
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

                UpdateVelocity();
            }
            else
            {
                // 足場がない時は、アニメをStandに変更して慣性移動
                anim.SetInteger("State", (int)AnimType.Stand);
            }
        }

        void UpdateVelocity()
        {
            Vector2 v = rb.velocity;
            v.x = spRenderer.flipX ? walkSpeed : -walkSpeed;
            rb.velocity = v;
            anim.speed = Mathf.Abs(v.x) * velocityToAnimSpeed;
        }

        /// <summary>
        /// スイーツを出現させる
        /// </summary>
        void ToSweets()
        {
            if (sweets != null)
            {
                SoundController.Play(SoundController.SeType.ToItem);
                GameManager.DecrementItemCount();
                Instantiate(sweets, transform.position, Quaternion.identity);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((collision.collider.gameObject.layer == mapNameLayer)
                ||  (collision.collider.gameObject.CompareTag("Enemy"))) {

                for (int i=0;i<collision.contacts.Length;i++)
                {
                    float footY = capCollider.bounds.min.y + footCheckDistance;
                    if (collision.contacts[i].point.y >= footY)
                    {
                        float dir = Mathf.Sign(collision.contacts[i].point.x - capCollider.bounds.center.x);
                        spRenderer.flipX = dir < -0.5f;
                        UpdateVelocity();
                        return;
                    }
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            OnCollisionEnter2D(collision);            
        }
    }
}