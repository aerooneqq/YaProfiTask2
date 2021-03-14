FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["YaProfiTask2/YaProfiTask2.csproj", "YaProfiTask2/"]
RUN dotnet restore "YaProfiTask2/YaProfiTask2.csproj"
COPY . .
WORKDIR "/src/YaProfiTask2"
RUN dotnet build "YaProfiTask2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YaProfiTask2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YaProfiTask2.dll"]
