FROM microsoft/dotnet:runtime

RUN [ "mkdir", "smtppilot" ]

WORKDIR /smtppilot

COPY out .

EXPOSE 25

ENTRYPOINT [ "dotnet", "SmtpPilot.dll", "-l", "-i", "any" ,"-p", "25" ]
