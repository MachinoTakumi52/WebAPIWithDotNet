# .NET 　 Core 　 WEB 　 API 　テンプレート

## プロジェクト作成

### dotnet version 設定

devcontainer → docker → Dockerfile 内「dotnet sdk 設定」に使用したいバージョンを設定

```Dockerfile
  # dotnet sdk設定                   ↓ここ
  FROM mcr.microsoft.com/dotnet/sdk:8.0
```

### 下記のコマンド bash で実行

```sh
/workspace/.createProject/newDotnetProject.sh
```

### vsCode 設定の反映のためウィンドの再読み込みを行う

下記のショートカットからコマンドパレットを開き「reload Window」を入力し「開発者：ウィンドウの再読み込み」を選択

```
ctrl + shift + P
```

### テンプレート

TODO サンプルファイル配置

- launch.json(vsCode デバッグ設定)
- settings.json(vsCode 設定)
- tasks.json(vsCode デバッグ設定)
- appsetting.json(システム環境変数)
- Program.cs(エントリーファイル)
- SampleContrller.cs(サンプルコントローラー)
- SampleModels.cs(サンプルモデル)

## ファイル構成

TODO API アーキテクチャの選定の反映
TODO アプリバージョンをどう管理するか

```bash
solutionName
|
|__Consts 定数フォルダ
|
|__Controllers コントローラー
|
|__Services サービス
|
|__Repositories リポジトリ
|
|__Models モデル
|  |
|  |__DTO データ転送オブジェクト
|
|__Properties デバッグ設定ファイル格納フォルダ
```

## 開発について

### ライブラリの追加

Nuget からライブラリを追加  
vsCode の場合は、コンソールから追加  
公式サイトにコマンドが書いてあるのでそれをコンソールに打ち込む  
https://www.nuget.org/

### 環境設定ファイル

TODO もうちょいうまいことしたい
`appsetting.json`ファイルに環境設定を参照している  
追加する際は、上記のファイルに環境変数を入力後`EnvConsts`ファイルにプロパティを追加

### エントリーポイント

`Program.cs`ファイルがエントリーポイントになる  
上記のファイル内に認証や CORS 設定などをやっていく
詳しい内容は、ファイル内にコメントで記載してあるので  
必要に応じてコメントアウトして使用してほしい

### コントローラー/モデル

詳しい開発方法は、  
`sampleController.cs`  
`sampleModel.cs`  
のコメントを参考に作成していってほしい

### 実行/デバッグ

デバッグポイント有効
ホットリロード有効(Program.cs 修正時は、再デバッグが必要)

## ビルド/デプロイ

TODO ビルド
TODO デプロイ
