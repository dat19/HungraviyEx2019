using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019 {
    public class Graviy : MonoBehaviour
    {
        public static Graviy instance = null;

        [Tooltip("重力係数"), SerializeField]
        float gravityScale = 1f;
        [Tooltip("ブラックホール発生時に、エネルギーを減らす秒速"), SerializeField]
        float energySubBlackhole = -0.3f;
        [Tooltip("着地時のエネルギーが回復秒速"), SerializeField]
        float energyRecoveryOnGround = 0.3f;
        [Tooltip("空中のエネルギー回復秒速"), SerializeField]
        float energyRecoveryInTheAir = 0.1f;
        [Tooltip("食べ物を食べた時に回復するエネルギー量"), SerializeField]
        float energyFood = 0.5f;
        [Tooltip("はらぺこ切り替えエネルギー"), SerializeField]
        float hungryAnim = 0.33f;
        [Tooltip("アイテムを吸い込む秒数"), SerializeField]
        float eatItemSeconds = 0.5f;
        [Tooltip("アイテムを吸い込める最低秒数"), SerializeField]
        float eatItemMinSeconds = 0.3f;

        public enum AnimType
        {
            Idle,
            Sucked,
            Fall,
        }

        /// <summary>
        /// エネルギーの最大値
        /// </summary>
        public const float EnergyMax = 1f;
        /// <summary>
        /// エネルギーの最小値
        /// </summary>
        public const float EnergyMin = -0.1f;

        public static Transform MouthTransform { get; private set; }

        /// <summary>
        /// 同時に食べられるアイテムの数
        /// </summary>
        const int EatingMax = 8;

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
        static ContactFilter2D contactFilter2D = new ContactFilter2D();
        static bool isEating = false;
        static int eatingCount = 0;
        static Item[] eatingObjects = new Item[EatingMax];

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

        /// <summary>
        /// 現在のエネルギー。0が空。1が満タン。
        /// </summary>
        public static float Energy { get; private set; }

        void Awake()
        {
            instance = this;
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
            anim.SetInteger("State", (int)AnimType.Idle);
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            spRenderer = GetComponentInChildren<SpriteRenderer>();
            suiyose = GetComponent<Suiyose>();
            Energy = EnergyMax;
            contactFilter2D.layerMask = LayerMask.GetMask("Map");
            isEating = false;
            eatingCount = 0;
            MouthTransform = transform.Find("MouthPosition").transform;
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
                spRenderer.flipX = Blackhole.instance.transform.position.x < transform.position.x;
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

            if (Blackhole.IsSpawn)
            {
                Energy += energySubBlackhole * Time.fixedDeltaTime;
            }
            else
            {
                if (OnGroundChecker.Check(capsuleCollider2D))
                {
                    Energy += energyRecoveryOnGround * Time.fixedDeltaTime;
                }
                else
                {
                    Energy += energyRecoveryInTheAir * Time.fixedDeltaTime;
                }
            }

            Energy = Mathf.Clamp(Energy, EnergyMin, EnergyMax);

            // はらぺこアニメ切り替えチェック
            anim.SetLayerWeight(1, Energy < hungryAnim ? 1 : 0);

            // 食べ終わりチェック
            if (isEating && anim.GetFloat("EatSpeed") < 0)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0)
                {
                    anim.SetBool("Inhole", false);
                    isEating = false;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Item"))
            {
                if (eatingCount >= EatingMax) {
#if UNITY_EDITOR
                    Debug.Log($"これ以上食べられない");
#endif
                    return; 
                }

                eatingObjects[eatingCount] = collision.collider.GetComponent<Item>();
                eatingObjects[eatingCount].Eat(eatItemSeconds, eatItemMinSeconds);
                eatingCount++;

                if (!isEating)
                {
                    isEating = true;
                    anim.SetBool("Inhole", true);
                    anim.SetFloat("EatSpeed", 1);
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

        /// <summary>
        /// 食べ終わったアイテムのインスタンスを報告。
        /// </summary>
        /// <param name="item">食べ終わったインスタンス</param>
        public void EatDone(Item item)
        {
            // エネルギーを増やす
            Energy = Mathf.Clamp01(Energy + energyFood);

            for (int i=0;i<eatingCount;i++)
            {
                if (eatingObjects[i] == item)
                {
                    eatingObjects[i] = eatingObjects[eatingCount - 1];
                    break;
                }
            }
            eatingCount--;
            Debug.Log($"  eatingCount={eatingCount}");

            // 全て食べていたら、口を閉じる
            if (eatingCount <= 0)
            {
                anim.SetFloat("EatSpeed", -1);
            }
        }
    }
}