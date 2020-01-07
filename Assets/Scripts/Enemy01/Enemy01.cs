using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class Enemy01 : MonoBehaviour
    {
        [Tooltip("移動データ"), SerializeField]
        Enemy01Route route = null;

        Rigidbody2D rb = null;
        Vector2 target;
        float speed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            route.Init(this);
        }

        void FixedUpdate()
        {
            if (!Graviy.CanMove) return;

            route.FixedUpdate();
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
            if (to.magnitude <= step)
            {
                // 到着
                step = to.magnitude;
                isReached = true;
            }

            rb.velocity = to.normalized * step / Time.fixedDeltaTime;

            return isReached;
        }
    }
}