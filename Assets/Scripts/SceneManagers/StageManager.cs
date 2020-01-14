using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HungraviyEx2019
{
    public class StageManager : SceneManagerBase
    {
        [Tooltip("ステージ時間"), SerializeField]
        float stageTime = 99.99f;
        [Tooltip("ステージごとのBGM"), SerializeField]
        SoundController.BgmType stageBgm = SoundController.BgmType.Stage1;

        public override void OnFadeOutDone()
        {
            Graviy.SetAudioListener(true);
            GameParams.SetStartTime(stageTime);
            SceneManager.SetActiveScene(gameObject.scene);
        }

        public override void OnFadeInDone()
        {
            base.OnFadeInDone();
            SoundController.PlayBGM(stageBgm);
            Tutorial.Next();
        }
    }
}
