using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HungraviyEx2019
{
    public class TitleManager : SceneManagerBase
    {
        static bool isStart = false;

        private new void Awake()
        {
            isStart = false;
            base.Awake();
        }

        public override void OnFadeOutDone()
        {
            SoundController.PlayBGM(SoundController.BgmType.Title, true);
            SceneManager.SetActiveScene(gameObject.scene);            
        }

        private void Update()
        {
            if (!GameParams.CanMove || isStart) return;

            if (Input.GetMouseButtonDown(0))
            {
                SoundController.Play(SoundController.SeType.Click);
                GameParams.NewGame();
                SceneChanger.ChangeScene(SceneChanger.SceneType.Game);
                isStart = true;
            }
        }
    }
}
