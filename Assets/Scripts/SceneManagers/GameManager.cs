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

        /// <summary>
        /// ステージのアイテム数
        /// </summary>
        public static int ItemCount { get; private set; }

        /// <summary>
        /// 取得したアイテムの個数
        /// </summary>
        public static int GetItem { get; private set; }

        private new void Awake()
        {
            instance = this;
            state = StateType.Game;
            base.Awake();
            waitStartTime = Time.time;
            ItemCount = 0;
            GetItem = 0;
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

        public override void OnFadeOutDone()
        {
            base.OnFadeOutDone();
            GameParams.InitStageParams();
        }

        /// <summary>
        /// クリアシーケンスを開始します。
        /// </summary>
        public static void Clear(ClearObject co)
        {
            state = StateType.Clear;
            waitStartTime = Time.time;
            SoundController.PlayBGM(SoundController.BgmType.Clear);
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
            SetText(timeBonusText, text);
        }

        /// <summary>
        /// ライフボーナス表示
        /// </summary>
        /// <param name="text"></param>
        public void LifeBonusText(string text)
        {
            SetText(lifeBonusText, text);
        }

        /// <summary>
        /// パーフェクトボーナス表示
        /// </summary>
        public void PerfectBonusText(string text)
        {
            SetText(perfectBonusText, text);
        }

        void SetText(TextMeshProUGUI tmp, string text)
        {
            if (!tmp.gameObject.activeSelf)
            {
                tmp.gameObject.SetActive(true);
            }
            tmp.text = text;
        }


        public static void GameOver()
        {
            instance.gameOverAnimator.gameObject.SetActive(true);
            instance.gameOverAnimator.SetTrigger("GameOver");
            state = StateType.GameOver;
            waitStartTime = Time.time;
            SoundController.PlayBGM(SoundController.BgmType.GameOver);
        }

        public static void ShowClick()
        {
            instance.clickAnimator.gameObject.SetActive(true);
            instance.clickAnimator.SetBool("Show", true);
        }

        /// <summary>
        /// アイテムやアイテムを持っている敵のStartから呼び出します。
        /// </summary>
        public static void AddItemCount()
        {
            ItemCount++;
        }

        /// <summary>
        /// 敵が発生させる時には多重カウントになるので、これを呼び出して戻す。
        /// </summary>
        public static void DecrementItemCount()
        {
            ItemCount--;
        }

        /// <summary>
        /// アイテムを取った時に呼び出します。
        /// </summary>
        public static void GetItemCount()
        {
            GetItem++;
        }
    }
}
