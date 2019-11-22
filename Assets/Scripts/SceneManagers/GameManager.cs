using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class GameManager : SceneManagerBase
    {

        private void Update()
        {
#if DEBUG
            if (Input.GetKeyDown(KeyCode.E))
            {
                // エンディングへ
                SceneChanger.ChangeScene(SceneChanger.SceneType.Ending);
            }
#endif
        }
    }
}
