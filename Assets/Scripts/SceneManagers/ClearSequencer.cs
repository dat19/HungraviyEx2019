using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public static class ClearSequencer
    {
        /// <summary>
        /// 次のステージか、エンディングへ進行してよいならtrueを返します。
        /// </summary>
        public static bool CanNext { get; private set; }

        /// <summary>
        /// 次の動作までの待ち時間
        /// </summary>
        const float WaitNext = 1f;

        const string TimeBonusPrefix = "タイムボーナス <mspace=0.7em>";
        static readonly WaitForFixedUpdate waitFixed = new WaitForFixedUpdate();
        static readonly WaitForSeconds waitNext = new WaitForSeconds(WaitNext);

        public static void Start(ClearObject clearObject)
        {
            CanNext = false;
            GameManager.instance.StartCoroutine(ClearCoroutine(clearObject));
        }

        static IEnumerator ClearCoroutine(ClearObject clearObject)
        {

            // ブラックホールに吸い込まれる
            yield return clearObject.ToBlackhole();

            Blackhole.instance.ClearDone();
            Graviy.instance.StartEat();

            // 落下
            yield return clearObject.Fall();

            // 口を閉じてクリア表示
            Graviy.instance.CloseMouth();
            GameManager.ShowClear();

            yield return waitNext;

            // タイムボーナス
            yield return TimeBonus();

            GameManager.ShowClick();
            CanNext = true;
        }

        static IEnumerator TimeBonus()
        {
            int bonus = 0;
            GameManager.instance.TimeBonusText($"{TimeBonusPrefix}{bonus,5}</mspace>");
            int leftTime = Mathf.FloorToInt(GameParams.playTime * GameParams.TimeBonus);
            GameParams.playTime = ((float)leftTime / GameParams.TimeBonus) + (0.1f / GameParams.TimeBonus);
            Debug.Log($"leftTIme={leftTime}");

            yield return waitNext;

            int keta = 1;
            int waru = 10;
            while (leftTime > 0)
            {
                int count = (leftTime % waru) / keta;
                for (int i = 0; i < count; i++)
                {
                    bonus += keta;
                    leftTime -= keta;

                    GameManager.instance.TimeBonusText($"{TimeBonusPrefix}{bonus,5}</mspace>");
                    GameParams.playTime = ((float)leftTime / GameParams.TimeBonus) + (0.1f / GameParams.TimeBonus);
                    yield return waitFixed;
                }

                keta *= 10;
                waru *= 10;
            }
        }
    }
}