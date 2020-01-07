# 実機テスト
iOSはTest Flightがあるが、Androidでも利用したいので、双方のサービスがあるDeploy Gateを利用する。古いが、比較記事は以下。

[Developers.IO. [iOS] TestFlight の特徴 と DeployGate との違い](https://dev.classmethod.jp/smartphone/testflight-deploygate/)

Deploy Gateの公式ページです。

https://deploygate.com/?locale=ja

## DeployGateの価格
Freeプランと月額1,550円のProプランがある。最大の違いはアプリの非表示ができるかどうかと思われる。公開状態の場合、アカウントのプロフィールページに登録しているアプリが表示されるが無断でのインストールはできない。

アップロードできるアプリ数やアプリを配布できる端末数はAndroidは無制限、iOSはiOS Developer Programの場合は100台まで。

[DeployGate. 料金](https://deploygate.com/pricing)

## 準備
まずはユーザー登録をする。

- https://deploygate.com/?locale=ja を開く
- GitHubアカウント、Googleアカウント、SAML認証と紐づけができるが、アカウントの作成は必要
- メールアドレス、ユーザー名、パスワード、利用目的を設定して、登録する

## アプリの登録
https://deploygate.com/dashboard を参照。

### Android
- apkファイルがビルドできたら、DeployGateのページにドラッグ＆ドロップでアップする
- スマホにDeployGateのアプリをインストール
- ログインが分かりづらい。ログイン後、アプリの再起動などをしているうちに、動いた
  - Googleなどにログインしている状態で、デプロイゲートでのログインも必要そう。詳細不明
- テスト時は、キーストアの設定は不要だった
