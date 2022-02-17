FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /published
EXPOSE 80
EXPOSE 443

COPY . .

FROM base as final
WORKDIR /app
COPY --from=base /app/publish .

# Without this SQL Client fails (https://github.com/dotnet/SqlClient/issues/220)
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["dotnet", "Rat.Api.dll"]