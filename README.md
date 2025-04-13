# Dotnet WEBAPI(開発資料)

## プロジェクト作成

### dotnet version 設定

devcontainer → docker → Dockerfile 内「dotnet sdk 設定」に使用したいバージョンを設定しコンテナを起動

```Dockerfile
  # dotnet sdk設定                   ↓ここ
  FROM mcr.microsoft.com/dotnet/sdk:8.0
```

### シェル実行

ターミナルを開き、bash から下記コマンドを実行

```sh
/workspace/.createProject/newDotnetSolution.sh
```

※実行権限がなく実行できない時は、以下のコマンドを実行

```sh
chmod +x /workspace/.createProject/newDotnetSolution.sh
```

### ウィンドの再読み込み

下記のショートカットからコマンドパレットを開き「reload Window」を入力し「開発者：ウィンドウの再読み込み」を選択

```
ctrl + shift + P
```

## 開発について

- ソリューションエクスプローラー
  vscode の左にあるエクスプローラータブに`solution explorer`タブが存在するので、こちらを開いて開発していく  
  ※`workSpace`タブでもいいが、visual studio のエクスプローラーに近いのは`solution explorer`なのでこちらをお勧めする

### フォルダ構成

```bash
FolderName
│  .gitignore # ignoreファイル
│  SolutionName.sln # ソリューションファイル
│  README.md # READMEファイル
│
├─.devcontainer # Docker関係フォルダ
│  │  devcontainer.json
│  │
│  └─docker
│          Compose.yml
│          Dockerfile
│
├─.vscode # vscodeでのデバッグ設定フォルダ
│      launch.json
│      settings.json
│      tasks.json
│
└─ProjectName # プロジェクトフォルダ
    │  appsettings.Development.json # 環境変数ファイル(開発)
    │  appsettings.json # 環境変数ファイル(本番)
    │  ProjectName.csproj # プロジェクトファイル
    │  ProjectName.http # 簡易テストファイル
    │  Program.cs # エントリーファイル
    │
    ├─Controllers # コントローラーフォルダ
    │      BaseController.cs # 共通コントローラーファイル
    │      ErrorController.cs # 共通エラーハンドリングコントローラーファイル
    │      SampleController.cs # サンプルコントローラーファイル
    │
    ├─Models # モデルフォルダ
    │  ├─Entities # エンティティフォルダ
    │  │      SampleEntity.cs # サンプルエンティティファイル
    │  │
    │  ├─Requests # リクエストフォルダ
    │  │  └─Sample
    │  │          CreateSampleRequest.cs
    │  │          ReadSampleRequest.cs
    │  │          ReadSamplesWithDivisionRequest.cs
    │  │          UpdateSampleRequest.cs
    │  │
    │  └─Responses # レスポンスフォルダ
    │      └─Sample
    │              ReadSampleResponse.cs
    │              ReadSamplesWithDivisionResponse.cs
    │
    ├─Properties # デバッグ設定(visual studio用)フォルダ
    │      launchSettings.json # デバッグ設定(visual studio用)ファイル
    │
    ├─Repositories # リポジトリフォルダ
    │      SampleRepository.cs # サンプルリポジトリファイル
    │
    ├─Services # サービスフォルダ
    │      SampleService.cs # サンプルサービスフォルダ
    │
    └─Utils # ユーティリティフォルダ
            Consts.cs # 定数ファイル
            DataBaseConnection.cs # DB接続共通ファイル
            DependencyInjectionExtensions.cs # DI共通ファイル
```

### 三層アーキテクチャ（Three-Tier Architecture）

1. Controller

- 役割  
  HTTP リクエストを受け付け、適切な Service メソッドを呼び出し、レスポンスを返す。

- 進め方  
  作成した Controller クラスに`BaseController.cs`を継承する
  詳しい内容は、`SampleController.cs`を参照

2. Service

- 役割  
  ビジネスロジックを担当。複数の Repository のデータを統合して加工。
- 進め方  
  機能ごとにファイルを作成
  DB 操作を行いたい場合は、Repository ファイルをサービスのコンストラクタで呼び出しそれぞれの処理で呼び出す
  詳しい内容は、`SampleService.cs`を参照

3. Repository

- 役割  
  データベースとのやり取りを担当（CRUD 操作）

- 進め方  
  DB テーブルの数だけファイルを作成  
  DB の CRUD 操作を書く  
  テーブルをリレイションしたデータを取得したい場合は、リレイションする主テーブルの Repository ファイルに取得ソースを書く  
  詳しい内容は、`SampleRepository.cs`を参照

4. Models

- 役割  
  データの構造を定義  
  Request：クライアントから送信されるデータを受け取るためのモデル  
  Response：クライアントに返すデータを表現するモデル  
  Entity：データベーステーブルとのマッピングを行うモデル

- 進め方

  Request：コントローラ名毎にフォルダを切り、その中にリクエストモデルを作成する  
  Response：コントローラ名毎にフォルダを切り、その中にレスポンスモデルを作成する  
  Entity：DB テーブルの数だけファイルを作成  
  詳しい内容は、`ReadSampleRequest.cs`と`ReadSampleResponse.cs`等を参照

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
- SampleRepository.cs
  サンプルリポジトリ
- SampleService.cs
  サンプルサービス
- SampleRequest.cs
  サンプルリクエスト
- SampleResponse.cs
  サンプルレスポンス
- DBConection.cs
  DB と接続するためのクラス  
  プロジェクト作成シェル実行時、使用する DB を選択したら自動で作成される  
  MariaDB(MySQL)用：DataBaseConnectionForMySql.cs  
  SQLServer 用：DataBaseConnectionForSqlServer.cs  
  PostgreSQL 用：DataBaseConnectionForPostgreSQL.cs

  TODO: oracle connecttion ファイル  
  TODO: mySQL BulkInsert 関数の作成(CSV ファイルから取り込むものしかない)

### ライブラリの追加

- `dotnet` コマンドによる追加  
  vsCode のコンソールを開きパッケージを追加したいプロジェクトまで移動  
  `dotnet` コマンドを用いて `nuget` からパッケージを追加

- `solution explorer` タブから追加  
  `solution explorer` タブからパッケージを追加したいプロジェクトを右クリックし`Add Nuget Pakage`を選択  
  検索欄が開かれるので、追加したいパッケージを入力  
  入力条件に当てはまるパッケージが表示されるので該当するものを選択してパッケージを追加

下記の nuget 公式サイトから、パッケージを検索することが可能  
パッケージインストールの dotnet コマンドが記載してあるため便利  
https://www.nuget.org/

### ファイル追加(C# Extention 拡張機能使用)

- `workSpace` タブからの追加
  Explorer タブから新規作成ファイルを配置するフォルダを選択して右クリックし`New C#`を選択  
  Class や Interface などテンプレートが作成できるので、該当するものを選択  
  上部ヘッダーにファイル名を入力すると新しいファイルが作成される

- `solution explorer` タブからの追加  
  Explorer タブから新規作成ファイルを配置するフォルダを選択して右クリックし`New File`を選択
  Class や Interface などテンプレートが作成できるので、該当するものを選択  
  上部ヘッダーにファイル名を入力すると新しいファイルが作成される

### 環境変数設定ファイル

Release 時は、`appsetting.json`ファイルが参照される  
Debug 時は、`appsetting.Development.json`ファイルが参照される  
環境変数を追加する際は、上記のファイルに環境変数を定義後`Consts`ファイルの`EnvConsts`クラスに取得するためのキーを定数として  
定義システム内で使用する際は、IConfiguration を DI する  
詳しくは、`sampleController.cs`にやり方を記載

### エントリーポイント

`Program.cs`ファイルがエントリーポイントになる  
上記のファイル内に認証や CORS 設定などをやっていく  
詳しい内容は、ファイル内にコメントで記載してあるので  
必要に応じてコメントアウトして使用する

### 実行/デバッグ

- `実行とデバッグ`タブから用途に合わせて実行

1. デバッグ(develop)
   `.NET Core デバッグ(Development)`を実行
2. デバッグ(release)
   `.NET Core デバッグ(Product)`を実行

- タスクから実行  
  下記のショートカットからコマンドパレットを開き「tasks: Run task」を入力し「タスクの実行」を選択  
  表示される選択肢の中から「watch」を選択する  
  watch のデバッグは、ホットリロード有効

```
ctrl + shift + P
```

## ビルド

- `SOLUTION EXPLORER`タブからビルド  
  vsCode の下部の`Any CPU`から`Debug`と`Release`を切り替え可能  
  ビルドしたプロジェクトを右クリックして`ビルド`を選択

- タスクからビルド
  下記のショートカットからコマンドパレットを開き「tasks: Run task」を入力し「タスクの実行」を選択

```
ctrl + shift + P
```

下記の選択肢の中から選択  
Debug：`build Debug`  
Release：`build Release`

## デプロイ

- タスクからデプロイ
  下記のショートカットからコマンドパレットを開き「tasks: Run task」を入力し「タスクの実行」を選択

```
ctrl + shift + P
```

下記の選択肢の中から選択  
Debug：`publish Debug`  
Release：`publish Release`
