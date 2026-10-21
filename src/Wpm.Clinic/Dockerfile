FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5038
EXPOSE 7222

ENV ASPNETCORE_URLS=http://+:5038

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Wpm.Clinic/Wpm.Clinic.csproj", "src/Wpm.Clinic/"]
RUN dotnet restore "src/Wpm.Clinic/Wpm.Clinic.csproj"
COPY . .
WORKDIR "/src/src/Wpm.Clinic"
RUN dotnet build "Wpm.Clinic.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Wpm.Clinic.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wpm.Clinic.dll"]
