FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
COPY . ./
RUN cp NuGet.*onfig /usr/local/share/NuGet.Config 2>/dev/null || :
WORKDIR /src/Axle
RUN dotnet build -c Release -r linux-x64 -o /app

FROM build AS publish
RUN dotnet publish -c Release -r linux-x64 -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Axle.dll"]