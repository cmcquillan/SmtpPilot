#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 25

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/SmtpPilot/SmtpPilot.csproj", "src/SmtpPilot/"]
COPY ["src/SmtpPilot.WebHooks/SmtpPilot.WebHooks.csproj", "src/SmtpPilot.WebHooks/"]
COPY ["src/SmtpPilot.Server/SmtpPilot.Server.csproj", "src/SmtpPilot.Server/"]
RUN dotnet restore "src/SmtpPilot/SmtpPilot.csproj"
COPY . .
WORKDIR "/src/src/SmtpPilot"
RUN dotnet build "SmtpPilot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmtpPilot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmtpPilot.dll", "-i", "any"]