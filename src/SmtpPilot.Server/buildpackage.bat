@echo OFF
msbuild /p:Configuration=Release;TargetFrameworkVersion=v3.5;OutDir=.\bin\Net35
msbuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;OutDir=.\bin\Net40
msbuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;OutDir=.\bin\Net45

nuget pack SmtpPilot.Server.csproj -Prop Configuration=Release
@echo ON