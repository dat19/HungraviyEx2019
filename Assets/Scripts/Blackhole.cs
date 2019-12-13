using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class Blackhole : MonoBehaviour
    {
        public static Blackhole instance = null;

        [Tooltip("最高速度"), SerializeField]
        float speedMax = 10f;

        public static bool CanMove
        {
            get
            {
                return !Fade.IsFading;
            }
        }

        /// <summary>
        /// エネルギーがあって、マウスをクリックしている時、true
        /// </summary>
        public static bool IsSpawn { get; private set; }

        Rigidbody2D rb;
        Animator anim;
        Camera myCamera = null;
        /// <summary>
        /// 前回の発生状況
        /// </summary>
        bool lastSpawned;
        /// <summary>
        /// SpawnかSpawnedの時true、DestroyかHidedの時falseにするようにアニメから設定するフラグ
        /// </summary>
        bool animSpawn;

        private void Awake()
        {
            instance = this;
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            animSpawn = false;
        }

        private void FixedUpdate()
        {
            if (!CanMove) return;

            bool mouseClicked = Input.GetMouseButton(0);
            IsSpawn = (mouseClicked && (Graviy.Energy > 0));
            anim.SetBool("Spawn", IsSpawn);

            // 発生アニメでなければ移動なし
            if (!animSpawn)
            {
                lastSpawned = false;
                return;
            }

            // 動かす
            if (myCamera == null)
            {
                myCamera = Camera.main;
            }
            Vector3 mpos = Input.mousePosition;
            mpos.z = 1;
            Vector3 target = myCamera.ScreenToWorldPoint(mpos);

            if (!lastSpawned)
            {
                transform.position = target;
                rb.velocity = Vector3.zero;
                lastSpawned = true;
                return;
            }

            Vector3 move = (target - transform.position) / Time.fixedDeltaTime;
            move = Vector3.ClampMagnitude(move, speedMax);
            rb.velocity = move;
        }

        /// <summary>
        /// アニメがSpawn状態に設定する。
        /// </summary>
        public void AnimSpawn()
        {
            animSpawn = true;
        }

        /// <summary>
        /// アニメが非表示の状態に設定する。
        /// </summary>
        public void AnimHide()
        {
            animSpawn = false;
        }

    }
}