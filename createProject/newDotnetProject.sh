#!/usr/bin/env bash
set -e  # エラーが発生した場合、スクリプトを終了するオプション

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

# デバッグ用の設定ファイルを作成

VS_CODE_DIR=".vscode"

# ディレクトリ作成
mkdir -p "$VS_CODE_DIR"

# TODO:launch.json 作成
# TODO:tasks.json 作成 (プロジェクトのビルドやテスト用)
# TODO:settings.json 作成 (OmniSharpやC# FormatterなどVS Code用設定)
echo "VS Codeの設定ファイルが作成されました。"

# プロジェクトディレクトリに移動
cd "$REQUIRED_DIR/$PROJECT_NAME"

# 必要フォルダ作成
mkdir -p "Controllers"
mkdir -p "Services"
mkdir -p "Models"
mkdir -p "Consts"
mkdir -p "Utility"

echo "テンプレートファイル作成しました。"
exit 1