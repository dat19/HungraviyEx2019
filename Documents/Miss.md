# ミスの処理
ぐらびぃが敵やトゲに触れた時のミスの処理を実装する。

## きっかけ
きっかけは、OnTriggerEnter(トゲ)とOnCollisionEnter(敵)の双方に必要。吸い込み中の敵も、触れたらダメージを受ける。

## 処理の流れ
ミスに伴う処理は以下の通り。

- ライフを減らす
- ライフが残っていない場合、ゲームオーバー状態へ。残っていたら以下の処理を継続
- 無敵時間開始
- 接触した相手の逆方向に加速
- 吸い込み中のアイテムがあったら、吸い込み解除
- 無敵時間中、アイテムの取得をキャンセル

## 変数の定義
ライフや無敵時間を表す変数は、すでに実装済みなので、それを利用する。何秒間、無敵にするかのパラメーターをGraviyに追加する。

```cs
[Tooltip("無敵秒数"), SerializeField]
float mutekiSeconds = 2f;
```

# ダメージ床の実装
まずは、実装がシンプルなダメージ床の実装を行う。ダメージ床は、TriggerなのでOntriggerEnter2Dでタグをチェックして発動させる。そのまま、ミスの処理を呼び出してよい。引数として、Collider2Dを渡す。

## ライフを減らす
ライフを減らすのと、ゲームオーバー判定を実装する。GameParamsにライフを減らす処理を実装して、戻り値でゲームオーバーかをわかるようにする。

```cs
/// <summary>
/// ライフを減らします。ゲームオーバーの状態になったらtrueを返します。
/// </summary>
/// <returns>ゲームオーバーならtrue</returns>
public static bool LifeDecrement()
{
    Life = Life > 0  ? Life-1 : 0;
    return Life == 0;
}
```

これを、Graviyがトゲに衝突した時に呼び出す。

```cs
private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("DamageTile"))
    {
        Miss(collision);
    }
}

void Miss(Collider2D tile)
{
    // ゲームオーバーチェック
    if (GameParams.LifeDecrement())
    {
        Debug.Log($"ゲームオーバーへ");
        return;
    }

    // 無敵時間設定
    mutekiTime = mutekiSeconds;
}
```

ゲームオーバーはまだ実装していないので、ログに表示しておく。無敵時間も設定しておく。

これで、ダメージ床に触れた時のダメージと、ゲームオーバーへの切り替え場所が完了。

## 無敵の実装
無敵時間を実装したので、効果を持たせる。先に作成したMissメソッドの冒頭に以下を入れればよい。

```cs
if (mutekiTime >= 0) { return; }
```

これでミスが呼び出されても処理しなくなる。あとは、無敵時間を減らす処理と、Animatorに値を渡す処理を、FixedUpdate()の最後に追加する。

```cs
// 無敵処理
mutekiTime -= Time.fixedDeltaTime;
anim.SetFloat("MutekiTime", mutekiTime);
```

## 吹っ飛び
ダメージを受けたら、接触した座標とは反対側に、力を加えて吹っ飛ばす。

接触した座標は、Collider2DのClosestPointを使って、ぐらびぃの中心座標から、衝突した相手の最寄り座標を得ればよい。ぐらびぃの中心座標から求めた座標を引いたベクトルを使って、吹っ飛ばすベクトルを生成して、AddForceで力を加える。

パラメーターは以下の通り。

```cs
[Tooltip("ふっとばす時の加速"), SerializeField]
float blowOffAdd = 15f;
```

吹っ飛ばす処理は以下の通り。

- `ClosestPoint`で、ぐらびぃの中心座標から、接触した最寄り座標を求める
- ぐらびぃの中心座標-求めた座標を算出
- 算出したベクトルの単位ベクトルを求めて、blowOffAddをかけて、加える力のベクトルを求める
- rb.velocity = Vector2.zero; で、現在の速度を一旦0に
- rb.AddForce()のForceMode2D.Impulseで、力を加える

ForceMode2Dには、`Force`と`Impulse`の2種類がある。`Force`は継続的に加える力で、1秒間力を加え続けると、設定した力が加わるという設定になる。`Impulse`は、設定した力を一瞬で加える。今回は瞬間的な力を加えたいので、`Impulse`で直接力を設定した。

## 吸い込み中のアイテムがあったら、吸い込み解除
食べようとしているアイテムがあったら解除します。吸い込みアニメも無効にします。Missに、以下の処理を加えます。

```cs
// 吸い寄せ中のアイテムがあったら解除
for (int i=0; i<eatingCount;i++)
{
    eatingObjects[i].ReleaseEat();
}
eatingCount = 0;
anim.SetBool("Inhole", false);
```

## 無敵中は、アイテムの取得をキャンセル
最後に、無敵中はアイテムを取れないようにします。OnTriggerEnter2Dと同様に、OnCollisionEnter2Dの先頭に、以下を加えます。

```cs
if (mutekiTime >= 0) return;
```

## 無敵が切れたら、ダメージを受けるようにする
OnTriggerStay2D()から、OnTriggerEnter2Dを呼び出して、接触中でもダメージを受けるようにする。

```cs
private void OnTriggerStay2D(Collider2D collision)
{
    OnTriggerEnter2D(collision);            
}
```

以上で、ダメージ床の処理は完了。


# 敵のダメージを受ける
Miss()メソッドができているので、敵と接触した時に呼び出すだけ。OnCollisionEnter2Dに`Enemy`タグとの接触があったら、ミスを呼び出せばよい。

```cs
else if (collision.collider.CompareTag("Enemy"))
{
    Miss(collision.collider);
}
```

接触を続けていてもダメージを受けるように、以下も追加。

```cs
private void OnCollisionStay2D(Collision2D collision)
{
    OnCollisionEnter2D(collision);
}
```
