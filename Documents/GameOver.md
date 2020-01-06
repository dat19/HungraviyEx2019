# ゲームオーバーの実装
今回は、ゲームオーバーやクリアはシーンでは分けずに、シーンを分けて管理する。状態はGameManagerに列挙子で宣言して、`state`という変数で管理するようにする。

```cs
        public enum StateType
        {
            Game,
            GameOver,
            Clear,
            NextScene,
        }

        /// <summary>
        /// ゲームの状態
        /// </summary>
        public static StateType state = StateType.Game;
```


# ゲームオーバー中に操作をできないようにする

## ぐらびぃ
Graviyに実装した`CanMove`に、ゲームオーバーやクリアの条件を追加する。

```cs
        /// <summary>
        /// 移動可能かどうかのフラグ
        /// </summary>
        public static bool CanMove
        {
            get
            {
                return !Fade.IsFading
                    && GameManager.state == GameManager.StateType.Game;
            }
        }
```

ゲームオーバーになった時にノックバックは有効にしておきたいので、操作不能になっても速度は0にならないようにしたい。そこで、速度を0にするのはゲーム中のみに限定するために、以下のように*FixedUpdate()*の先頭の部分を書き換える。

            if (!CanMove)
            {
                if (GameManager.state == GameManager.StateType.Game)
                {
                    rb.velocity = Vector2.zero;
                }
                return;
            }

## ブラックホール
*CanMove*は、ブラックホールにも実装していたが、Graviyと条件は同じなので統合した方がよい。*Blackhole.cs*を開いて、`CanMove`の定義を削除して、エラーが発生するようになる部分を`Graviy.CanMove`に書き換える。これによって、同じ条件で操作を無効にする。同時に、ゲームオーバーやクリア時にはブラックホールの発生を止めたいので、アニメの停止を呼び出すようにする。`Blackhole.cs`の*FixedUpdate()*の先頭の処理を、以下に書き換える。

```cs
            if (!Graviy.CanMove)
            {
                anim.SetBool("Spawn", false);
                return;
            }

```
