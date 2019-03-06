FROM microsoft/dotnet:2.2-sdk-alpine AS build
COPY . ./
RUN apk add --no-cache icu-libs \
 && cp cert/*.crt /usr/local/share/ca-certificates/ 2>/dev/null \
 && update-ca-certificates \
 || echo "No specific certificate given."
RUN cp [nN]u[gG]et.[cC]onfig /usr/local/share/NuGet.Config 2>/dev/null \
 || echo "No specific Nuget configuration given."
WORKDIR /src/Axle
RUN dotnet publish -c Release -o /app

FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Axle.dll"]