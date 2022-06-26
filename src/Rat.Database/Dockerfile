FROM mcr.microsoft.com/mssql/server:2019-latest
EXPOSE 1433

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Password1!
ENV MSSQL_PID=Developer
ENV MSSQL_TCP_PORT=1433

ARG BAK_PATH

COPY $BAK_PATH /baks/

RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" &&  /opt/mssql-tools/bin/sqlcmd -S 127.0.0.1 -U sa -P Password1! -Q "RESTORE DATABASE RatDb FROM DISK='/baks/RatDbFull.bak';"
