using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HungraviyEx2019
{
    public class StageManager : SceneManagerBase
    {
        public override void OnFadeOutDone()
        {
            SceneManager.SetActiveScene(gameObject.scene);
        }

#if DEBUG
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SoundController.Play(SoundController.SeType.Click);
                SceneChanger.ChangeScene(SceneChanger.SceneType.Game);
            }
        }
#endif
    }
}
