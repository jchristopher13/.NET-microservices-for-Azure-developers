FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["/src/Wpm.Management.Api/Wpm.Management.Api.csproj", "/src/Wpm.Management.Api/"]
RUN dotnet restore "/src/Wpm.Management.Api/Wpm.Management.Api.csproj"
COPY . .
WORKDIR "/src/Wpm.Management.Api"
RUN dotnet build "Wpm.Management.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Wpm.Management.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wpm.Management.Api.dll"]
