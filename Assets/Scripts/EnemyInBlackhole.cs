using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class EnemyInBlackhole : MonoBehaviour
    {
        public enum StateType
        {
            None = -1,
            In,
            Out,
            InDone,
        }

        /// <summary>
        /// 現在の吸い込まれ状態
        /// </summary>
        public StateType NowState { get; private set; }

        /// <summary>
        /// 次に切り替えたい状態
        /// </summary>
        public StateType NextState { get; private set; }

        /// <summary>
        /// ブラックホールへの吸い込まれが完了
        /// </summary>
        public bool isBlackholeInDone = false;

        /// <summary>
        /// ブラックホールから生還
        /// </summary>
        public bool isBlackholeRecoveried = false;

        Animator anim;

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            NowState = StateType.None;
            NextState = StateType.None;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Blackhole"))
            {
                NextState = StateType.In;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            OnTriggerEnter2D(collision);
        }

        /// <summary>
        /// 復活したフラグ
        /// </summary>
        public void BlackholeInDone()
        {
            isBlackholeInDone = (anim.GetFloat("Speed") > 0);
        }

        /// <summary>
        /// 敵のFixedUpdateから呼び出して、状態を更新します。
        /// アニメーションの開始は、呼び出しもとで行います。
        /// ブラックホールから外れた時の逆再生はここで行います。
        /// アニメの終了はここで行います。
        /// </summary>
        /// <returns>切り替え後の状態</returns>
        public StateType FixedUpdateState()
        {
            // 吸い込みの開始を検出。アニメ開始は戻った先で行うのでこれで終わり
            if (NextState == StateType.In)
            {
                NowState = StateType.In;
                // 終了チェック
                if (isBlackholeInDone)
                {
                    NowState = StateType.InDone;
                }

                ClearFlags();
                return NowState;
            }

            // 状態がなければ変化なし
            if (NowState == StateType.None)
            {
                ClearFlags();
                return NowState;
            }

            // 逆再生への切り替えチェック
            if (NowState == StateType.In)
            {
                anim.SetFloat("Speed", -1);
                NowState = StateType.Out;
            }

            // 逆再生終了チェック
            if ((anim.GetFloat("Speed") < 0) && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0))
            { 
                NowState = StateType.None;
            }

            ClearFlags();
            return NowState;
        }

        void ClearFlags()
        {
            isBlackholeInDone = isBlackholeRecoveried = false;
            NextState = StateType.None;
        }
    }
}