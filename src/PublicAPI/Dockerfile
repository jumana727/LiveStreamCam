FROM phadkesharanmatrixcomsec/opencv-aspnet:4.8.0-8.0-jammy AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM phadkesharanmatrixcomsec/opencv-sdk:4.8.0-8.0-jammy AS build
ARG configuration=Release
WORKDIR /src
COPY ["./Directory.Build.props", "./"]
COPY ["./Directory.Packages.props", "./"]
COPY ["src/FaceDetectionSDK/FaceDetectionSDK.csproj", "src/FaceDetectionSDK/"]
COPY ["src/ApplicationCore/ApplicationCore.csproj", "src/ApplicationCore/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/PublicAPI/PublicAPI.csproj", "src/PublicAPI/"]
RUN dotnet restore "src/PublicAPI/PublicAPI.csproj"
COPY . .
WORKDIR "/src/src/PublicAPI"
RUN dotnet build "PublicAPI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "PublicAPI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY "../lib" /app/publish/lib
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublicAPI.dll"]
