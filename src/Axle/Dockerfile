FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY . ./
WORKDIR /src/Axle
RUN dotnet build -c Release -r linux-x64 -o /app

FROM build AS publish
RUN dotnet publish -c Release -r linux-x64 -o /app

FROM publish AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Axle.dll"]