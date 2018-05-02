FROM microsoft/aspnetcore-build:2.0 AS build
ARG version=0.0.1-developer
WORKDIR /src
COPY . ./
WORKDIR /src/Axle
RUN dotnet publish -c Release -r linux-x64 -o ../../build /p:ShowLinkerSizeComparison=true /p:Version=$version

FROM microsoft/aspnetcore:2.0 AS final
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT Docker
COPY --from=build /build/ .
ENTRYPOINT ["dotnet", "Axle.dll"]