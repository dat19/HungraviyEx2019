using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

        Animator anim;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public IEnumerator ToBlackhole()
        {
            Blackhole.instance.ClearStart(targetOffset);

            anim.SetInteger("State", (int)AnimType.Inhale);
            float time = 0f;
            Vector3 startPos = transform.position;

            while (time < inhaleSeconds)
            {
                time += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(time / inhaleSeconds);
                transform.position = Vector3.Lerp(startPos, Graviy.MouthPosition + targetOffset, t);
                yield return wait;
            }
        }

        public IEnumerator Fall()
        {
            Vector3 v = Vector3.zero;
            bool startGulp = false;
            Vector3 pos = transform.position;
            while (transform.position.y > Graviy.MouthPosition.y)
            {
                v.y += Physics.gravity.y * Time.fixedDeltaTime;
                pos += v * Time.fixedDeltaTime;
                pos.x = Graviy.MouthPosition.x;
                transform.position = pos;
                if (!startGulp && (transform.position.y < (Graviy.MouthPosition.y + gulpHeight)))
                {
                    startGulp = true;
                    anim.SetInteger("State", (int)AnimType.Gulp);
                }

                yield return wait;
            }
        }

        public void ToEatScale()
        {
            anim.SetInteger("State", (int)AnimType.ToEat);
        }
    }
}