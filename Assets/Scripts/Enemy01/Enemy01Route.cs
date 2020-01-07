using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HungraviyEx2019
{
    [System.Serializable]
    public class Enemy01RouteData
    {
        [Tooltip("待ち時間")]
        public float waitSeconds = 1f;
        [Tooltip("現在の座標から目的座標への位置ベクトル"), SerializeField]
        public Vector2 targetPosition = Vector2.zero;
        [Tooltip("速度"), SerializeField]
        public float speed = 5f;
    }

    public class Enemy01Route : ScriptableObject
    {
        [Tooltip("移動データ配列"), SerializeField]
        Enemy01RouteData[] routes = null;

        enum StateType
        {
            Start,  // 0=現在のデータの再生を開始
            Wait,   // 1=待つ
            Move,   // 2=目的地に到着するまで移動
        }

        /// <summary>
        /// 現在の状態
        /// </summary>
        StateType state = StateType.Start;

        Enemy01 enemy01 = null;

        /// <summary>
        /// 現在のインデックス
        /// </summary>
        int index = 0;

        /// <summary>
        /// 開始時間
        /// </summary>
        float startTime;

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Hungraviy/Create Enemy01Route")]
        public static void Create()
        {
            var assetPath = "Assets/Scripts/Enemy01/ScriptableObjects/Enemy01Route.asset";
            Enemy01Route asset = ScriptableObject.CreateInstance<Enemy01Route>();
            ProjectWindowUtil.CreateAsset(asset, assetPath);
        }
#endif

        /// <summary>
        /// 最初に呼び出して、パラメーターを初期化しておきます。
        /// </summary>
        public void Init(Enemy01 ins)
        {
            index = 0;
            state = StateType.Start;
            enemy01 = ins;
        }

        /// <summary>
        /// 更新時に呼び出します。
        /// </summary>
        public void FixedUpdate()
        {
            switch (state)
            {
                case StateType.Start:
                    state = StateType.Wait;
                    startTime = Time.time;
                    break;

                case StateType.Wait:
                    if (Time.time-startTime >= routes[index].waitSeconds)
                    {
                        // 移動へ
                        state = StateType.Move;
                        enemy01.SetTarget(routes[index].targetPosition, routes[index].speed);
                    }

                    break;

                case StateType.Move:
                    if (enemy01.Move())
                    {
                        // 移動完了
                        state = StateType.Start;
                        index = (index < routes.Length - 1) ? index + 1 : 0;
                    }
                    break;
            }
        }
    }
}