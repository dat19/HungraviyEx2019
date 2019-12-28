using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class Item : MonoBehaviour
    {
        [Tooltip("得点"), SerializeField]
        int point = 100;

        bool isEating = false;
        float eatStartTime = 0;
        float eatTime;
        float eatMinTime;
        Rigidbody2D rb = null;

        /// <summary>
        /// 食べたとみなす距離
        /// </summary>
        const float EatDistance = 0.015f;

        const string ItemLayer = "Item";
        const string EatingLayer = "EatingItem";

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            isEating = false;
        }

        private void Start()
        {
            GameManager.AddItemLeft();
        }

        private void FixedUpdate()
        {
            if (!isEating) return;

            float t = (Time.time - eatStartTime);
            float dist = Vector3.Distance(Graviy.MouthPosition, transform.position);

            if ((t >= eatTime) 
                || ((t >= eatMinTime)
                    && (dist <= EatDistance)))
            {
                // 食べる
                Graviy.instance.EatDone(this);
                GameParams.AddScore(point);
                Destroy(gameObject);
                return;
            }

            t = Mathf.Clamp01(t / eatTime);

            Vector3 next = Vector3.Lerp(transform.position, Graviy.MouthPosition, t);
            Vector3 move = (next - transform.position) / Time.fixedDeltaTime;
            rb.velocity = move;
        }

        /// <summary>
        /// 食べた時にぐらびぃから呼び出す処理。
        /// </summary>
        /// <param name="eattm">食べ終えるのに必要な秒数</param>
        public void Eat(float eattm, float eatMin)
        {
            isEating = true;
            eatStartTime = Time.time;
            eatTime = eattm;
            eatMinTime = eatMin;
            gameObject.layer = LayerMask.NameToLayer(EatingLayer);
        }

        /// <summary>
        /// ぐらびぃがダメージを受けた時に呼び出す途中で放出する処理
        /// </summary>
        public void ReleaseEat()
        {
            isEating = false;
            gameObject.layer = LayerMask.NameToLayer(ItemLayer);
        }
    }
}