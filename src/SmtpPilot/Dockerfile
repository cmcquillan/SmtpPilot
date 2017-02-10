FROM microsoft/windowsservercore:latest

RUN mkdir smtppilot

COPY * /smtppilot/
EXPOSE 25

WORKDIR /smtppilot/

ENTRYPOINT powershell -NoProfile -Command ./smtppilot.exe -l -i any -m
