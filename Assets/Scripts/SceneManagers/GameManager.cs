using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class GameManager : SceneManagerBase
    {
        public static GameManager instance = null;

        [Tooltip("クリア用アニメ"), SerializeField]
        Animator clearAnimator = null;
        [Tooltip("ゲームオーバー用アニメ"), SerializeField]
        Animator gameOverAnimator = null;
        [Tooltip("クリックアニメ"), SerializeField]
        Animator clickAnimator = null;
        [Tooltip("ハイスコアアニメ"), SerializeField]
        Animator highScoreAnimator = null;
        [Tooltip("シーン切り替えから、シーンを切り替えられるようになるまでの待ち時間"), SerializeField]
        float nextSceneWait = 1f;

        public enum StateType
        {
            Game,
            GameOver,
            Clear,
            NextScene,
        }

        /// <summary>
        /// ゲームの状態
        /// </summary>
        public static StateType state = StateType.Game;

        /// <summary>
        /// ウェイト待ちを開始した時の経過秒数
        /// </summary>
        static float waitStartTime;

        private new void Awake()
        {
            instance = this;
            state = StateType.Game;
            base.Awake();
            waitStartTime = Time.time;
        }

        private void Update()
        {
            if ((state == StateType.Game)
                ||(state == StateType.NextScene)
                || (Time.time - waitStartTime < nextSceneWait))
                return;

            if (state == StateType.GameOver)
            {
                if (!instance.clickAnimator.GetBool("Show"))
                {
                    instance.clickAnimator.SetBool("Show", true);
                    if (GameParams.CheckHighScore())
                    {
                        instance.highScoreAnimator.SetTrigger("Show");
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    instance.clickAnimator.SetBool("Show", false);
                    SoundController.Play(SoundController.SeType.Click);
                    SceneChanger.ChangeScene(SceneChanger.SceneType.Title);
                    state = StateType.NextScene;
                }
            }
            else if (state == StateType.Clear && ClearObject.CanNext)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SoundController.Play(SoundController.SeType.Click);
                    instance.clickAnimator.SetBool("Show", false);

                    // ステージクリア
                    if (GameParams.NextStage())
                    {
                        SceneChanger.ChangeScene(SceneChanger.SceneType.Ending);
                    }
                    else
                    {
                        SceneChanger.ChangeScene(SceneChanger.SceneType.Game);
                    }
                    state = StateType.NextScene;
                }
            }
        }

        public static void Clear()
        {
            instance.clearAnimator.SetTrigger("Clear");
            state = StateType.Clear;
            waitStartTime = Time.time;
        }

        public static void GameOver()
        {
            instance.gameOverAnimator.SetTrigger("GameOver");
            state = StateType.GameOver;
            waitStartTime = Time.time;
        }

        public static void ShowClick()
        {
            instance.clickAnimator.SetBool("Show", true);
        }
    }
}
