FROM mcr.microsoft.com/dotnet/sdk:6.0 as base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS="http://*:80"

FROM base as build
WORKDIR /src
COPY ["Auth.csproj", "./"]
RUN dotnet restore "./Auth.csproj"
COPY . .
WORKDIR /src/.
RUN dotnet build "./Auth.csproj" -c Debug -o /app/build

FROM build as publish
RUN dotnet publish "./Auth.csproj" -c Debug -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .

FROM base as app
COPY ./bin/Release/net6.0/ /app/
WORKDIR /app

ENTRYPOINT ["dotnet", "Auth.dll"]