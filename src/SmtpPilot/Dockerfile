FROM microsoft/dotnet:runtime
ENV COMPlus_DebugWriteToStdErr 1

RUN [ "mkdir", "smtppilot" ]

WORKDIR /smtppilot

COPY out .

EXPOSE 25

ENTRYPOINT [ "dotnet", "SmtpPilot.dll", "-l", "-i", "any" ,"-p", "25" ]
