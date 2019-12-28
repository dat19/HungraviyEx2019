using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        [Tooltip("タイムボーナステキスト"), SerializeField]
        TextMeshProUGUI timeBonusText = null;
        [Tooltip("ライフボーナステキスト"), SerializeField]
        TextMeshProUGUI lifeBonusText = null;
        [Tooltip("パーフェクトボーナステキスト"), SerializeField]
        TextMeshProUGUI perfectBonusText = null;
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
                if (!instance.clickAnimator.gameObject.activeSelf)
                {
                    // ゲームオーバーになって、クリック可能になる秒数が経過した初回にクリック表示とハイスコアチェック
                    instance.clickAnimator.gameObject.SetActive(true);
                    instance.clickAnimator.SetBool("Show", true);
                    if (GameParams.CheckHighScore())
                    {
                        instance.highScoreAnimator.gameObject.SetActive(true);
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
            else if (state == StateType.Clear && ClearSequencer.CanNext)
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

        /// <summary>
        /// クリアシーケンスを開始します。
        /// </summary>
        public static void Clear(ClearObject co)
        {
            state = StateType.Clear;
            waitStartTime = Time.time;
            ClearSequencer.Start(co);
        }

        public static void ShowClear()
        {
            instance.clearAnimator.gameObject.SetActive(true);
            instance.clearAnimator.SetTrigger("Clear");
            waitStartTime = Time.time;
        }

        /// <summary>
        /// タイムボーナス表示
        /// </summary>
        /// <param name="text"></param>
        public void TimeBonusText(string text)
        {
            if (!timeBonusText.gameObject.activeSelf)
            {
                timeBonusText.gameObject.SetActive(true);
            }
            timeBonusText.text = text;
        }

        /// <summary>
        /// ライフボーナス表示
        /// </summary>
        /// <param name="text"></param>
        public void LifeBonusText(string text)
        {
            if (!lifeBonusText.gameObject.activeSelf)
            {
                lifeBonusText.gameObject.SetActive(true);
            }
            lifeBonusText.text = text;
        }

        /// <summary>
        /// パーフェクトボーナス表示
        /// </summary>
        public void PerfectBonusText()
        {
            if (!perfectBonusText.gameObject.activeSelf)
            {
                perfectBonusText.gameObject.SetActive(true);
            }
        }

        public static void GameOver()
        {
            instance.gameOverAnimator.gameObject.SetActive(true);
            instance.gameOverAnimator.SetTrigger("GameOver");
            state = StateType.GameOver;
            waitStartTime = Time.time;
        }

        public static void ShowClick()
        {
            instance.clickAnimator.gameObject.SetActive(true);
            instance.clickAnimator.SetBool("Show", true);
        }
    }
}
