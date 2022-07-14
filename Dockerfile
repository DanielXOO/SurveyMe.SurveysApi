FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY . .

RUN dotnet restore "/app/Surveys.Api/Surveys.Api.csproj"
RUN dotnet publish -c Debug -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Surveys.Api.dll"]