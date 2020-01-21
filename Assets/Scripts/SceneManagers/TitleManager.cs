using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HungraviyEx2019
{
    public class TitleManager : SceneManagerBase
    {
        [Tooltip("クレジットオブジェクト"), SerializeField]
        GameObject creditObject = null;

        static bool isStart = false;

        private new void Awake()
        {
            isStart = false;
            base.Awake();
        }

        public override void OnFadeOutDone()
        {
            SoundController.StopBGM();
            SceneManager.SetActiveScene(gameObject.scene);            
        }

        private void Update()
        {
            if (!GameParams.CanMove || isStart) return;

            if (!creditObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                Application.Quit();
#endif
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SoundController.Play(SoundController.SeType.Click);
                    DisplayCredit(false);
                }
            }
        }

        /// <summary>
        /// ゲーム開始
        /// </summary>
        public void GameStart()
        {
            if (isStart) return;

            SoundController.Play(SoundController.SeType.Start);
            GameParams.NewGame();
            SceneChanger.ChangeScene(SceneChanger.SceneType.Game);
            isStart = true;
        }

        /// <summary>
        /// クレジットの表示を設定
        /// </summary>
        /// <param name="flag"></param>
        public void DisplayCredit(bool flag)
        {
            SoundController.Play(SoundController.SeType.Click);
            creditObject.SetActive(flag);
        }
    }
}
