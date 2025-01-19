#!/usr/bin/env bash
set -e  # エラーが発生した場合、スクリプトを終了するオプション

# TODO:ソリューション内にプロジェクトを追加できるようなシェルの作成
# TODO:プロジェクトの自動作成シェル追加

# テンプレートディレクトリ
TEMPLATE_DIR="/workspace/.createProject/template"

# 現在の .NET バージョンを取得
INSTALLED_VERSION=$(dotnet --version)

# バージョンチェック (基本的にメジャーバージョンを抽出)
MAJOR_VERSION=$(echo "$INSTALLED_VERSION" | cut -d. -f1)
FRAMEWORK_VERSION="net${MAJOR_VERSION}.0"

echo "使用するフレームワーク: $FRAMEWORK_VERSION"

# カレントディレクトリチェック
CURRENT_DIR=$(pwd)
REQUIRED_DIR="/workspace"

# カレントディテクトリが「/workspace」でない場合はエラーをだす
if [ "$CURRENT_DIR" != "$REQUIRED_DIR" ]; then
    echo "エラー: このスクリプトは $REQUIRED_DIR ディレクトリ内で実行してください。"
    echo "現在のディレクトリ: $CURRENT_DIR"
    exit 1
fi

# プロジェクト設定
echo "Web API プロジェクトの名前を教えてください"
read -p "プロジェクト名: " PROJECT_NAME

# プロジェクト名が空欄の場合はエラーをだす
if [ -z "$PROJECT_NAME" ]; then
    echo "プロジェクト名が空欄です。もう一度スクリプトを実行してください。"
    exit 1
fi

# プロジェクトディレクトリ
PROJECT_DIR="workspace"

# Web API プロジェクト作成 SDKのバージョンからフレームワークを指定
dotnet new webapi -n "$PROJECT_NAME" --framework "$FRAMEWORK_VERSION"

# ソリューションファイル作成
dotnet new sln -n "$PROJECT_NAME"

# ソリューションにプロジェクト追加
dotnet sln add "$PROJECT_NAME/$PROJECT_NAME.csproj"

echo "プロジェクトが作成されました。"

# Swagger  アノテーションを有効するためのパッケージをインストール
dotnet add package Swashbuckle.AspNetCore.Annotations

# デバッグ用の設定ファイルを作成

VS_CODE_DIR=".vscode"

# ディレクトリ作成
mkdir -p "$VS_CODE_DIR"

# launch.json 作成
cp "$TEMPLATE_DIR"/launch.json /"$PROJECT_DIR"/"$VS_CODE_DIR"/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" /"$PROJECT_DIR"/"$VS_CODE_DIR"/launch.json
sed -i "s/FRAMEWORK_VERSION/$FRAMEWORK_VERSION/g" /"$PROJECT_DIR"/"$VS_CODE_DIR"/launch.json
# tasks.json 作成 (プロジェクトのビルドやテスト用)
cp "$TEMPLATE_DIR"/tasks.json /"$PROJECT_DIR"/"$VS_CODE_DIR"/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" /"$PROJECT_DIR"/"$VS_CODE_DIR"/tasks.json
# settings.json 作成 (OmniSharpやC# FormatterなどVS Code用設定)
cp "$TEMPLATE_DIR"/settings.json /"$PROJECT_DIR"/"$VS_CODE_DIR"/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" /"$PROJECT_DIR"/"$VS_CODE_DIR"/settings.json

echo "VS Codeの設定ファイルが作成されました。"

# プロジェクトディレクトリに移動
cd "$REQUIRED_DIR/$PROJECT_NAME"

# 必要フォルダ作成
mkdir -p "Controllers"
mkdir -p "Services"
mkdir -p "Repositories"
mkdir -p "Utils"
mkdir -p "Models"
cd "Models"
mkdir -p "Entities"
mkdir -p "Requests"
cd "Requests"
mkdir -p "Sample"
cd ..
mkdir -p "Responses"
cd "Responses"
mkdir -p "Sample"
cd ..
cd ..

# サンプルファイル作成
# Program.csファイルを削除して .createProject/template からコピー
rm Program.cs
cp "$TEMPLATE_DIR"/Program.cs .
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" Program.cs

# サンプルController
cp "$TEMPLATE_DIR"/SampleController.cs ./Controllers/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Controllers/SampleController.cs
# ErrorController(共通ハンドラ)
cp "$TEMPLATE_DIR"/ErrorController.cs ./Controllers/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Controllers/ErrorController.cs
# BaseApiController
cp "$TEMPLATE_DIR"/BaseController.cs ./Controllers/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Controllers/BaseController.cs
# サンプルサービス
cp "$TEMPLATE_DIR"/SampleService.cs ./Services/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Services/SampleService.cs
# サンプルリポジトリ
cp "$TEMPLATE_DIR"/SampleRepository.cs ./Repositories/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Repositories/SampleRepository.cs
# サンプルエンティティ
cp "$TEMPLATE_DIR"/SampleEntity.cs ./Models/Entities/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Models/Entities/SampleEntity.cs
# サンプルリクエスト
cp "$TEMPLATE_DIR"/ReadSampleRequest.cs ./Models/Requests/Sample/
cp "$TEMPLATE_DIR"/ReadSamplesWithDivisionRequest.cs ./Models/Requests/Sample/
cp "$TEMPLATE_DIR"/CreateSampleRequest.cs ./Models/Requests/Sample/
cp "$TEMPLATE_DIR"/UpdateSampleRequest.cs ./Models/Requests/Sample/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Models/Requests/Sample/ReadSampleRequest.cs
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Models/Requests/Sample/ReadSamplesWithDivisionRequest.cs
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Models/Requests/Sample/CreateSampleRequest.cs
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Models/Requests/Sample/UpdateSampleRequest.cs
# サンプルレスポンス
cp "$TEMPLATE_DIR"/ReadSampleResponse.cs ./Models/Responses/Sample/
cp "$TEMPLATE_DIR"/ReadSamplesWithDivisionResponse.cs ./Models/Responses/Sample/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Models/Responses/Sample/ReadSampleResponse.cs
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Models/Responses/Sample/ReadSamplesWithDivisionResponse.cs
# 定数ファイル
cp "$TEMPLATE_DIR"/Consts.cs ./Utils/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Utils/Consts.cs
# DIExtension(依存性注入用拡張メソッド)
cp "$TEMPLATE_DIR"/DependencyInjectionExtensions.cs ./Utils/
sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Utils/DependencyInjectionExtensions.cs

# appsettings.json 作成
rm "$REQUIRED_DIR/$PROJECT_NAME"/appsettings.json
cp "$TEMPLATE_DIR"/appsettings.json .
# appsettings.Development.json 作成
rm "$REQUIRED_DIR/$PROJECT_NAME"/appsettings.Development.json
cp "$TEMPLATE_DIR"/appsettings.Development.json .

cd ..
# .gitignore 作成
dotnet new gitignore

echo "テンプレートファイル作成しました。"

# データベースを使用するか確認
use_db_flag=false
while true; do
    echo "データベースを使用しますか？"
    echo "1) 使用する"
    echo "2) 使用しない"
    read -p "番号を入力してください (1-2): " use_db

    case $use_db in
        1)
            use_db_flag=true
            break
            ;;
        2)
            break
            ;;
        *)
            echo "無効な選択です。もう一度入力してください。"
            ;;
    esac
done


# データベース選択処理（使用する場合のみ）
# createDBConnection.shを実行
# TODO: 依存性の注入の自動生成
# TODO:存在すればプロジェクトを選択させて、そのプロジェクトにDB接続ファイルを追加する
# TODO:DB自動生成シェルを作成してここで実行(複数のDBを使用する場合があるため)
if $use_db_flag; then
    
    cd "$REQUIRED_DIR/$PROJECT_NAME"

    is_selected_db=false
    echo "使用するデータベースを選択:"
    
    PS3="番号を選択してください (1-4): "  # プロンプトを設定
    options=("Sqlserver" "MySQL" "PostgreSQL" "なし")
    select opt in "${options[@]}"
    do
        case $opt in
            "Sqlserver")
                is_selected_db=true
                echo "Sqlserverのコネクションファイルを作成します"
                # DBConection作成
                cp "$TEMPLATE_DIR"/DataBaseConnectionForSqlServer.cs ./Utils/
                sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Utils/DataBaseConnectionForSqlServer.cs
                
                echo "作成が完了しました"

                echo "クライアントパッケージをインストールしています..."

                dotnet add package Microsoft.Data.SqlClient

                if [ $? -eq 0 ]; then
                    echo "クライアントパッケージのインストールが成功しました。"
                else
                    echo "クライアントパッケージのインストールに失敗しました。"
                fi

                break
                ;;
            "MySQL")
                is_selected_db=true
                echo "MySQLのコネクションファイルを作成します"
                # DBConection作成
                cp "$TEMPLATE_DIR"/DataBaseConnectionForMySql.cs ./Utils/
                sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Utils/DataBaseConnectionForMySql.cs
                echo "作成が完了しました"

                echo "クライアントパッケージをインストールしています..."

                dotnet add package MySql.Data

                if [ $? -eq 0 ]; then
                    echo "クライアントパッケージのインストールが成功しました。"
                else
                    echo "クライアントパッケージのインストールに失敗しました。"
                fi

                break
                ;;
            "PostgreSQL")
                is_selected_db=true
                echo "PostgreSQLのコネクションファイルを作成します"
                # DBConection作成
                cp "$TEMPLATE_DIR"/DataBaseConnectionForPostgreSQL.cs ./Utils/
                sed -i "s/PROJECT_NAME/$PROJECT_NAME/g" ./Utils/DataBaseConnectionForPostgreSQL.cs
                echo "作成が完了しました"

                echo "クライアントパッケージをインストールしています..."

                dotnet add package Npgsql

                if [ $? -eq 0 ]; then
                    echo "クライアントパッケージのインストールが成功しました。"
                else
                    echo "クライアントパッケージのインストールに失敗しました。"
                fi

                break
                ;;
            "なし")
                break
                ;;
            *)
                echo "無効な選択です。もう一度入力してください。"
                ;;
        esac
    done

    # Dapperパッケージをインストール
    if $is_selected_db; then
        echo "Dapperパッケージをインストールしています..."
        dotnet add package Dapper

        if [ $? -eq 0 ]; then
            echo "Dapperパッケージのインストールが成功しました。"
        else
            echo "Dapperパッケージのインストールに失敗しました。"
        fi

        # TODO:DIExtension(依存性注入用拡張メソッド)にDB接続用のコードを追加
        
    fi

fi

exit 1
