using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class EndingManager : SceneManagerBase
    {
        [Tooltip("シーン切り替えから、シーンを切り替えられるようになるまでの待ち時間"), SerializeField]
        float nextSceneWait = 2f;
        [Tooltip("ハイスコア表示"), SerializeField]
        GameObject highScoreText = null;

        float startTime;
        bool isHighScore;

        private void Start()
        {
            startTime = nextSceneWait;
            isHighScore = GameParams.CheckHighScore();
        }

        private void FixedUpdate()
        {
            if (Fade.IsFading) return;

            startTime -= Time.fixedDeltaTime;
            if (isHighScore && !highScoreText.activeSelf)
            {
                highScoreText.SetActive(isHighScore);                
                highScoreText.GetComponent<Animator>().SetTrigger("Show");
            }

            if (startTime > 0) return;

            if (Input.GetMouseButtonDown(0))
            {
                SoundController.Play(SoundController.SeType.Click);
                SceneChanger.ChangeScene(SceneChanger.SceneType.Title);
            }
        }

        public override void OnFadeInDone()
        {
            base.OnFadeInDone();
            SoundController.PlayBGM(SoundController.BgmType.Ending);
        }
    }
}
