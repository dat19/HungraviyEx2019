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
        [Tooltip("移動データ配列")]
        public Enemy01RouteData[] routes = null;

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Hungraviy/Create Enemy01Route")]
        public static void Create()
        {
            var assetPath = "Assets/Scripts/Enemy01/ScriptableObjects/Enemy01Route.asset";
            Enemy01Route asset = ScriptableObject.CreateInstance<Enemy01Route>();
            ProjectWindowUtil.CreateAsset(asset, assetPath);
        }
#endif

    }
}