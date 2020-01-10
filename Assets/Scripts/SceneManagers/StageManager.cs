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

        public override void OnFadeOutDone()
        {
            Debug.Log($"  fade out done");
            GameParams.SetStartTime(stageTime);
            SceneManager.SetActiveScene(gameObject.scene);
        }

        public override void OnFadeInDone()
        {
            Debug.Log($"  fade in done");
            base.OnFadeInDone();
            Debug.Log($"  fade in done2");
            SoundController.PlayBGM(SoundController.BgmType.Stage1 + GameParams.Stage);
        }
    }
}
