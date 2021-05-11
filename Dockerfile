FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DevBootCampNetCoreLogging/DevBootCampNetCoreLogging.csproj", "DevBootCampNetCoreLogging/"]
RUN dotnet restore "DevBootCampNetCoreLogging/DevBootCampNetCoreLogging.csproj"
COPY . .
WORKDIR "/src/DevBootCampNetCoreLogging"
RUN dotnet build "DevBootCampNetCoreLogging.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DevBootCampNetCoreLogging.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DevBootCampNetCoreLogging.dll"]