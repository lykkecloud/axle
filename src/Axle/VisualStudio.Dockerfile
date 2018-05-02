# NOTE (Cameron): Visual Studio tooling does not currently offer the flexibility to use the actual Dockerfile so this is included as a workaround.

FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY . ./
WORKDIR /src/Axle
RUN dotnet publish -c Release -r linux-x64 -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Axle.dll"]