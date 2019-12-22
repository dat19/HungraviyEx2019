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
    }
}
