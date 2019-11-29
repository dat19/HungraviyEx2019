# カメラの移動
今回のカメラは、プレイヤーの開始時点の座標を原則としては維持しつつ、右にしかスクロールさせません。よって、プレイヤーの子供にするわけにはいきません。スクリプトを作成して、初期位置を記録して、それを元にプレイヤーを追うように作ります。

## 必要な変数
今回はシンプルに追いたいだけなのでシンプルです。シーン開始時に、プレイヤーからカメラへのオフセット値を保存しておくためのVector3型の変数が1つあればOKです。

# スクリプトの作成
RightMoveCameraの名前で新規にC#スクリプトを作成して、カメラのプレハブにアタッチします。スクリプトは以下の通りです。

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class RightMoveCamera : MonoBehaviour
    {
        /// <summary>
        /// プレイヤーからカメラの座標を求めるためのオフセット値
        /// </summary>
        static Vector3 offset;

        void Start()
        {
            offset = transform.position - Graviy.Instance.transform.position;
        }

        void LateUpdate()
        {
            Vector3 next = Graviy.Instance.transform.position + offset;
            if (next.x < transform.position.x)
            {
                next.x = transform.position.x;
            }
            transform.position = next;
        }
    }
}
```

- Start()で、カメラの現在位置からぐらびぃの座標を引いて記録しておきます
  - この値をぐらびぃの現在座標に加えれば、カメラを表示したい座標が求まります
  - GraviyのInstanceはAwakeで初期化されるので、この処理はそのあとで実行されるStart()に実装します。これで、処理順の違いによるInstanceがnullでエラーになるのを防ぎます
- カメラの処理は描画の直前であるLateUpdate()で行うのがセオリーです
- 次の座標を求めて、それが現在の座標より左だったら、次のX座標を現在の座標に据え置くようにします。これにより、右のみ動くようになります。Yは更新しているので、Y方向は常に動作します


以上で完成です。実行して、動きを確認してください。
