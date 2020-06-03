FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
COPY . ./
RUN apk add --no-cache icu-libs \
 && mkdir cert -p \
 && cp cert/*.crt /usr/local/share/ca-certificates/ 2>/dev/null \
 && update-ca-certificates \
 || echo "No specific certificate given."
RUN cp [nN]u[gG]et.[cC]onfig /usr/local/share/NuGet.Config 2>/dev/null \
 || echo "No specific Nuget configuration given."
WORKDIR /src/Axle
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS final
COPY --from=build cert ./cert
RUN apk add --no-cache icu-libs \
 && cp cert/*.crt /usr/local/share/ca-certificates/ 2>/dev/null \
 && update-ca-certificates \
 || echo "No specific certificate given."
RUN apk update && apk add bash
WORKDIR /app
COPY --from=build /app .
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENTRYPOINT ["dotnet", "Axle.dll"]