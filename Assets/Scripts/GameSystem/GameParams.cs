using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    /// <summary>
    /// 残りのライフや、プレイヤーが操作が可能な状況かどうかなど、
    /// ゲーム全体を制御するための変数や機能を提供するためのクラス。
    /// </summary>
    public class GameParams : Singleton<GameParams>
    {
        /// <summary>
        /// 操作可能な時、trueを返します。
        /// </summary>
        public static bool CanMove
        {
            get
            {
                return SceneChanger.IsBooting
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
        /// ライフの上限数
        /// </summary>
        public const int LifeMax = 3;

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Life = LifeMax;
            }
        }
#endif


        /// <summary>
        /// 新しくゲームを開始する時の初期化処理
        /// </summary>
        public static void NewGame()
        {
            Life = LifeMax;
            Stage = 0;
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
    }
}
