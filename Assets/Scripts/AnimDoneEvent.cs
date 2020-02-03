using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HungraviyEx2019
{
    public class AnimDoneEvent : MonoBehaviour
    {
        [Tooltip("アニメが終了した時に実行するメソッド"), SerializeField]
        UnityEvent animDoneEvent = null;

        /// <summary>
        /// アニメが完了した時に実行するイベント
        /// </summary>
        public void OnAnimDone()
        {
            animDoneEvent.Invoke();
        }
    }
}