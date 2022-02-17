FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
EXPOSE 80
EXPOSE 443

ARG publishPath=/bin/Debug/net6.0
ENV PUBLISHED_PATH=${publishPath}

COPY $publishPath .

# Without this SQL Client fails (https://github.com/dotnet/SqlClient/issues/220)
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["dotnet", "Rat.Api.dll"]