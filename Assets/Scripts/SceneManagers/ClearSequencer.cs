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
        const float WaitNextSeconds = 1f;

        /// <summary>
        /// ライフボーナスの待ち時間
        /// </summary>
        const float WaitLifeSeconds = 0.25f;

        /// <summary>
        /// ライフボーナス
        /// </summary>
        const int LifeBonusPoint = 1000;

        /// <summary>
        /// パーフェクトの得点
        /// </summary>
        const int PerfectPoint = 5000;

        const string TimeBonusPrefix = "タイムボーナス <mspace=0.7em>";
        const string LifeBonusPrefix = "ライフボーナス <mspace=0.7em>";
        readonly static string PerfectBonusMessage = $"パーフェクト!!      <mspace=0.7em>{PerfectPoint}</mspace>";
        const string Kosu = "<size=40>";
        static readonly WaitForFixedUpdate waitFixed = new WaitForFixedUpdate();

        public static void Start(ClearObject clearObject)
        {
            CanNext = false;

            // 残り時間をボーナス点にまるめこむ
            int leftTime = Mathf.FloorToInt(GameParams.playTime * GameParams.TimeBonus);
            GameParams.playTime = ((float)leftTime / GameParams.TimeBonus) + (0.1f / GameParams.TimeBonus);

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
            Graviy.AddEnergy(Graviy.EnergyMax);
            Graviy.instance.CloseMouth();
            GameManager.ShowClear();

            yield return WaitForSecondsOrClick(WaitNextSeconds);

            // タイムボーナス
            yield return TimeBonus();
            yield return WaitForSecondsOrClick(WaitNextSeconds);

            // ライフボーナス
            yield return LifeBonus();
            yield return WaitForSecondsOrClick(WaitNextSeconds);

            // アイテムパーフェクトボーナス
            if (GameManager.GetItem >= GameManager.ItemCount)
            {
                GameManager.instance.PerfectBonusText(PerfectBonusMessage);
                GameParams.AddScore(PerfectPoint);
            }
            else
            {
                GameManager.instance.PerfectBonusText($"{GameManager.ItemCount}{Kosu}コ</size>中{GameManager.GetItem}{Kosu}コ</size>ゲット。{GameManager.ItemCount - GameManager.GetItem}{Kosu}コ</size>とりのがし...");
            }

            yield return WaitForSecondsOrClick(WaitNextSeconds);

            GameManager.ShowClick();
            CanNext = true;
        }

        static IEnumerator LifeBonus()
        {
            int bonus = 0;
            GameManager.instance.LifeBonusText($"{LifeBonusPrefix}{bonus,5}</mspace>");

            yield return WaitForSecondsOrClick(WaitNextSeconds);

            while (GameParams.Life > 0)
            {
                yield return WaitForSecondsOrClick(WaitLifeSeconds);

                bonus += LifeBonusPoint;
                GameParams.AddScore(LifeBonusPoint);
                GameParams.LifeDecrement();
                GameManager.instance.LifeBonusText($"{LifeBonusPrefix}{bonus,5}</mspace>");
            }
        }

        static IEnumerator TimeBonus()
        {
            int bonus = 0;
            GameManager.instance.TimeBonusText($"{TimeBonusPrefix}{bonus,5}</mspace>");
            int leftTime = Mathf.FloorToInt(GameParams.playTime * GameParams.TimeBonus);

            yield return WaitForSecondsOrClick(WaitNextSeconds);

            int keta = 1;
            int waru = 10;
            while (leftTime > 0)
            {
                int count = (leftTime % waru) / keta;
                for (int i = 0; i < count; i++)
                {
                    bonus += keta;
                    GameParams.AddScore(keta);
                    leftTime -= keta;

                    GameManager.instance.TimeBonusText($"{TimeBonusPrefix}{bonus,5}</mspace>");
                    GameParams.playTime = ((float)leftTime / GameParams.TimeBonus) + (0.1f / GameParams.TimeBonus);
                    yield return waitFixed;
                }

                keta *= 10;
                waru *= 10;
            }
        }

        static IEnumerator WaitForSecondsOrClick(float sec)
        {
            float start = Time.time;
            while (((Time.time-start) < sec) && !Input.GetMouseButtonDown(0))
            {
                yield return waitFixed;
            }
        }
    }
}