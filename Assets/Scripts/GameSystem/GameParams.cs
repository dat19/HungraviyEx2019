using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HungraviyEx2019
{
    /// <summary>
    /// 残りのライフや、プレイヤーが操作が可能な状況かどうかなど、
    /// ゲーム全体を制御するための変数や機能を提供するためのクラス。
    /// </summary>
    public class GameParams : Singleton<GameParams>
    {
        [Tooltip("デバッグ用開始ステージ"), SerializeField]
        int StartStage = 0;

        /// <summary>
        /// 全ステージ数
        /// </summary>
        public const int StageMax = 3;

        /// <summary>
        /// ライフの上限数
        /// </summary>
        public const int LifeMax = 3;

        /// <summary>
        /// スコア上限
        /// </summary>
        public const int ScoreMax = 999999;

        /// <summary>
        /// パーフェクトボーナス
        /// </summary>
        public const int PerfectBonus = 10000;

        /// <summary>
        /// 秒あたりの残り時間ボーナス
        /// </summary>
        public const float TimeBonus = 100;

        /// <summary>
        /// ハイスコアを記録
        /// </summary>
        static bool isHighScore = false;

        /// <summary>
        /// 操作可能な時、trueを返します。
        /// </summary>
        public static bool CanMove
        {
            get
            {
                return !SceneChanger.IsBooting
                    && !SceneChanger.IsChanging
                    && !Fade.IsFading;
            }
        }

        /// <summary>
        /// 残りライフ
        /// </summary>
        public static int Life { get; private set; }

        /// <summary>
        /// 遊ぶステージ数。Stage1を0とする。
        /// </summary>
        public static int Stage { get; private set; }

        public static string StageName
        {
            get
            {
                return $"Stage{Stage+1}";
            }
        }

        /// <summary>
        /// 現在のスコア
        /// </summary>
        public static int Score { get; private set; }

        /// <summary>
        /// 起動してからのハイスコア
        /// </summary>
        public static int HighScore { get; private set; }

        /// <summary>
        /// プレイタイム
        /// </summary>
        public static float playTime;

        private void Awake()
        {
            Score = 0;
            HighScore = 0;
            playTime = 0;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Life = LifeMax;
            }
        }
#endif

        private void FixedUpdate()
        {
            if (Graviy.CanMove)
            {
                playTime -= Time.fixedDeltaTime;
            }
        }

        /// <summary>
        /// 新しくゲームを開始する時の初期化処理
        /// </summary>
        public static void NewGame()
        {
            Life = LifeMax;
#if UNITY_EDITOR
            Stage = Instance.StartStage;
#endif
            Score = 0;
            isHighScore = false;
        }

        /// <summary>
        /// ライフを指定の数増やします。
        /// </summary>
        /// <param name="add">回復させるハートの数</param>
        public static void AddLife(int add)
        {
            Life = Mathf.Min(Life + add, LifeMax);
        }

        /// <summary>
        /// ライフを減らします。ゲームオーバーの状態になったらtrueを返します。
        /// </summary>
        /// <returns>ゲームオーバーならtrue</returns>
        public static bool LifeDecrement()
        {
            Life = Life > 0  ? Life-1 : 0;
            return Life == 0;
        }

        /// <summary>
        /// 次のステージへ。エンディングにする場合、trueを返します。
        /// </summary>
        /// <returns>true=エンディングへ</returns>
        public static bool NextStage()
        {
            Stage++;
            return (Stage >= StageMax);
        }

        /// <summary>
        /// フェードアウト時に呼び出すステージ開始前の初期化処理
        /// </summary>
        public static void InitStageParams()
        {
            Life = LifeMax;
        }

        #region Private Methods

        /// <summary>
        /// 加算する得点
        /// </summary>
        /// <param name="add">得点</param1>
        public static void AddScore(int add)
        {
            Score = Mathf.Min(Score + add, ScoreMax);
        }

        /// <summary>
        /// ステージの持ち時間を設定
        /// </summary>
        /// <param name="tm"></param>
        public static void SetStartTime(float tm)
        {
            playTime = tm;
        }

        /// <summary>
        /// ハイスコアを記録したかを確認
        /// </summary>
        /// <returns></returns>
        public static bool CheckHighScore()
        {
            isHighScore = false;
            if (Score > HighScore)
            {
                HighScore = Score;
                isHighScore = true;
            }

            return isHighScore;
        }

        #endregion Private Methods

    }
}
