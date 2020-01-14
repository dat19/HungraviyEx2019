using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class Enemy01 : MonoBehaviour
    {
        [Tooltip("移動データ"), SerializeField]
        Enemy01Route route = null;

        enum StateType
        {
            Start,  // 0=現在のデータの再生を開始
            Wait,   // 1=待つ
            Move,   // 2=目的地に到着するまで移動
        }

        /// <summary>
        /// 現在の状態
        /// </summary>
        StateType state = StateType.Start;

        /// <summary>
        /// 現在のインデックス
        /// </summary>
        int index = 0;

        /// <summary>
        /// 開始時間
        /// </summary>
        float startTime;

        Rigidbody2D rb = null;
        Vector2 target;
        Vector2 startPosition;
        float speed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            index = 0;
            state = StateType.Start;
            startPosition = transform.position;
        }

        /// <summary>
        /// 更新時に呼び出します。
        /// </summary>
        public void FixedUpdate()
        {
            if (!Graviy.CanMove) return;

            switch (state)
            {
                case StateType.Start:
                    state = StateType.Wait;
                    startTime = Time.time;
                    break;

                case StateType.Wait:
                    if (Time.time - startTime >= route.routes[index].waitSeconds)
                    {
                        // 移動へ
                        state = StateType.Move;
                        SetTarget(route.routes[index].targetPosition, route.routes[index].speed);
                    }
                    break;

                case StateType.Move:
                    if (Move())
                    {
                        // 移動完了
                        state = StateType.Start;
                        index = (index < route.routes.Length - 1) ? index + 1 : 0;
                    }
                    break;
            }
        }


        /// <summary>
        /// 移動目的地と速度を設定します。
        /// </summary>
        /// <param name="respos">目的地の相対座標</param>
        /// <param name="spd">速度</param>
        public void SetTarget(Vector2 respos, float spd)
        {
            target = (Vector2)transform.position + respos;
            speed = spd;
        }

        /// <summary>
        /// 移動
        /// </summary>
        /// <returns>移動完了=true / 移動継続=false</returns>
        public bool Move()
        {
            bool isReached = false;
            Vector2 to = target - (Vector2)transform.position;
            float step = speed * Time.fixedDeltaTime;
            rb.velocity = to.normalized * step / Time.fixedDeltaTime;

            if (to.magnitude <= step)
            {
                // 到着
                step = to.magnitude;
                isReached = true;
                rb.velocity = Vector2.zero;
                transform.position = target;
            }

            return isReached;
        }
    }
}