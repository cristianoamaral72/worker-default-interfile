FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Worker.Robo/Worker.Robo.csproj", "Worker.Robo/"]
RUN dotnet restore "Worker.Robo/Worker.Robo.csproj"
COPY . .
WORKDIR "/src/Worker.Robo"
RUN dotnet build "Worker.Robo.csproj" -c Release -o /app/build

ENV TZ 'America/Sao_Paulo'
RUN echo $TZ > /etc/timezone && \
apt-get update && apt-get install -y tzdata && \
rm /etc/localtime && \
ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && \
dpkg-reconfigure -f noninteractive tzdata && \
apt-get clean

# COPY /chromedriver.exe, "/src/Worker.Robo"


# # install essential tools
# RUN apt-get update && apt-get install unzip

# # install Chrome
# RUN wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb \
#     && apt-get install ./google-chrome-stable_current_amd64.deb -y

# # download matching Chrome Driver
# # https://stackoverflow.com/a/61928952/167920
# RUN chromeVersion=$(google-chrome --product-version) \
#     && chromeMajorVersion=${chromeVersion%%.*} \
#     && latestDriverReleaseURL=https://chromedriver.storage.googleapis.com/LATEST_RELEASE_$chromeMajorVersion \
#     && wget $latestDriverReleaseURL \
#     && latestDriverVersionFileName="LATEST_RELEASE_"$chromeMajorVersion \
#     && latestFullDriverVersion=$(cat $latestDriverVersionFileName) \
#     && rm $latestDriverVersionFileName \
#     && finalURL="http://chromedriver.storage.googleapis.com/"$latestFullDriverVersion"/chromedriver_linux64.zip" \
#     && wget $finalURL \
#     && unzip chromedriver_linux64.zip \
#     && rm chromedriver_linux64.zip

FROM build AS publish
RUN dotnet publish "Worker.Robo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY ["Worker.Robo/chromedriver.exe", "/app/chromedriver"]

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Worker.Robo.dll"]
