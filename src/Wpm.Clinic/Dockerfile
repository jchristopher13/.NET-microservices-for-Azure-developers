FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["/Wpm.Clinic/Wpm.Clinic.csproj", "/Wpm.Clinic/"]
RUN dotnet restore "/Wpm.Clinic/Wpm.Clinic.csproj"
COPY . .
WORKDIR "/src/Wpm.Clinic"
RUN dotnet build "Wpm.Clinic.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Wpm.Clinic.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wpm.Clinic.dll"]
