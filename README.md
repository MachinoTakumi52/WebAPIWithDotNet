# WEB API開発環境

## プロジェクト作成

### dotnet version 設定

devcontainer → docker → Dockerfile 内「dotnet sdk 設定」に使用したいバージョンを設定

```Dockerfile
  # dotnet sdk設定                   ↓ここ
  FROM mcr.microsoft.com/dotnet/sdk:8.0
```

### シェル実行

ターミナルを開き、bash から下記コマンドを実行

```sh
/workspace/.createProject/newDotnetProject.sh
```

### ウィンドの再読み込み

下記のショートカットからコマンドパレットを開き「reload Window」を入力し「開発者：ウィンドウの再読み込み」を選択

```
ctrl + shift + P
```

## 開発について

### フォルダ構成

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
|  |__DTOs データ転送オブジェクト
|  |　|
|  |　|__Requests リクエストモデル
|  |　|
|  |　|__Responses レスポンスモデル
|  |　|
|  |　|__Joined DBテーブル結合モデル
|  |
|  |__Entities Entity
|
|__Properties デバッグ設定ファイル格納フォルダ
|
|__Utils 共通で使用する関数ファイルなどを格納するフォルダ
```

### テンプレートファイル

- launch.json  
  vsCode デバッグ設定
- settings.json  
  vsCode 設定
- tasks.json  
  vsCode デバッグ設定
- appsettings.json  
  本番用システム環境変数
- appsettings.Development.json  
  開発用システム環境変数
- EnvConsts.cs  
  環境変数取得用のキーを定義する定数クラス
- Program.cs  
  エントリーファイル
- ErrorController.cs  
  共通エラーハンドリングコントローラー
- SampleContrller.cs  
  サンプルコントローラー
- BaseController.cs
  ベースコントローラー
  このコントローラーを継承して作成していく
- SampleModels.cs  
  サンプルモデル
  リクエストやレスポンスのモデルを格納
- TODO:テンプレート作成 repository
- TODO:テンプレート作成 service
- TODO:テンプレート作成 request
- TODO:テンプレート作成 response
- TODO:テンプレート作成 joined
- TODO:DBConection
  DB と接続するためのクラス
  プロジェクト作成シェル実行時、使用する DB を選択したら自動で作成される
  TODO: oracle connecttion ファイル
  TODO: mySQL BulkInsert 関数の作成(CSV ファイルから取り込むものしかない)

### ライブラリの追加

Nuget からライブラリを追加  
vsCode の場合は、コンソールから追加  
公式サイトにコマンドが書いてあるのでそれをコンソールに打ち込む  
https://www.nuget.org/

### ファイル追加(C# Extention 拡張機能使用)

Explorer タブからファイルを作成したいフォルダを選択して右クリックし`New C#`を選択
Class や Interface などテンプレートが作成できるので、該当するものを選択
上部ヘッダーにファイル名を入力すると新しいファイルが作成される

### 環境変数設定ファイル

Release 時は、`appsetting.json`ファイルが参照される  
Debug 時は、`appsetting.Development.json`ファイルが参照される  
環境変数を追加する際は、上記のファイルに環境変数を定義後`EnvConsts`ファイルに取得するためのキーを定数として定義
システム内で使用する際は、IConfiguration を DI する  
詳しくは、`sampleController.cs`にやり方を記載

### エントリーポイント

`Program.cs`ファイルがエントリーポイントになる  
上記のファイル内に認証や CORS 設定などをやっていく  
詳しい内容は、ファイル内にコメントで記載してあるので  
必要に応じてコメントアウトして使用してほしい

### 実行/デバッグ

- `実行とデバッグ`タブから用途に合わせて実行

1. デバッグ(develop)
   `.NET Core デバッグ(Development)`を実行
2. デバッグ(release)
   `.NET Core デバッグ(Product)`を実行

- ホットリロード有効

- デバッグ(ホットリロード有効)
  下記のショートカットからコマンドパレットを開き「tasks: Run task」を入力し「タスクの実行」を選択
  表示される選択肢の中から「watch」を選択する

```
ctrl + shift + P
```

## ビルド/デプロイ

下記のショートカットからコマンドパレットを開き「tasks: Run task」を入力し「タスクの実行」を選択

```
ctrl + shift + P
```

選択肢の中から、用途に合ったタスクを選択すると実行される

- ビルド
  Debug：`build Debug`
  Release：`build Release`
- デプロイ
  Debug：`publish Debug`
  Release：`publish Release`

TODO:デプロイ
