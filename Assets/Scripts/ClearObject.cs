using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class ClearObject : MonoBehaviour
    {
        [Tooltip("ぐらびぃに対してブラックホールを発生させるオフセット座標"), SerializeField]
        Vector3 targetOffset = new Vector3(0f, 6f, 0f);
        [Tooltip("吸い込みにかかる秒数"), SerializeField]
        float inhaleSeconds = 1f;
        [Tooltip("飲み込み始める高さ"), SerializeField]
        float gulpHeight = 1f;

        enum AnimType
        {
            None,
            Inhale,
            ToEat,
            Gulp
        }

        /// <summary>
        /// クリック表示までの秒数
        /// </summary>
        const float WaitClick = 0.5f;

        /// <summary>
        /// 次のステージやエンディングに進んでよい時、trueを返します。
        /// </summary>
        public static bool CanNext { get;private set; }

        Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void ToEatScale()
        {
            anim.SetInteger("State", (int)AnimType.ToEat);
        }

        public void Inhale()
        {
            CanNext = false;
            StartCoroutine(InhaleCoroutine());
        }

        IEnumerator InhaleCoroutine()
        {
            Blackhole.instance.ClearStart(targetOffset);

            anim.SetInteger("State", (int)AnimType.Inhale);
            float time = 0f;
            WaitForFixedUpdate wait = new WaitForFixedUpdate();
            Vector3 startPos = transform.position;

            while (time < inhaleSeconds)
            {
                time += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(time / inhaleSeconds);
                transform.position = Vector3.Lerp(startPos, Graviy.MouthPosition + targetOffset, t);
                yield return wait;
            }

            Blackhole.instance.ClearDone();
            Graviy.instance.StartEat();

            // 落下
            Vector3 v = Vector3.zero;
            bool startGulp = false;
            Vector3 pos = transform.position;
            while (transform.position.y > Graviy.MouthPosition.y)
            {
                v.y += Physics.gravity.y * Time.fixedDeltaTime;
                pos += v * Time.fixedDeltaTime;
                pos.x = Graviy.MouthPosition.x;
                transform.position = pos;
                if (!startGulp && (transform.position.y < (Graviy.MouthPosition.y+gulpHeight))) {
                    startGulp = true;
                    anim.SetInteger("State", (int)AnimType.Gulp);
                }

                yield return wait;
            }

            // 口を閉じてクリア表示
            Graviy.instance.CloseMouth();
            GameManager.Clear();

            yield return new WaitForSeconds(WaitClick);

            GameManager.ShowClick();
            CanNext = true;
        }
    }
}