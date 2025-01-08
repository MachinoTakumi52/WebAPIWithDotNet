#!/usr/bin/env bash
set -e  # エラーが発生した場合、スクリプトを終了するオプション

# テンプレートディレクトリ
TEMPLATE_DIR="/workspace/.createProject/template"

# プロジェクト名
# TODO:既に存在するか
# TODO:存在すればプロジェクトを選択させて、そのプロジェクトにDB接続ファイルを追加する
PROJECT_NAME=""

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
    fi