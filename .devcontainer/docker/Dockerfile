# dotnet sdk設定
FROM mcr.microsoft.com/dotnet/sdk:8.0

# デバッグ用の自己署名書
RUN dotnet dev-certs https --trust

# timezone
RUN ln -sf /usr/share/zoneinfo/Asia/Tokyo /etc/localtime

# apt
RUN apt update

# git
RUN apt-get install -y git