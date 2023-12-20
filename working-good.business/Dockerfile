FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY src/working-good.business.core/working-good.business.core.csproj ./working-good.business.core/
COPY src/working-good.business.application/working-good.business.application.csproj ./working-good.business.application/
COPY src/working-good.business.infrastructure/working-good.business.infrastructure.csproj ./working-good.business.infrastructure/
COPY src/working-good.business.api/working-good.business.api.csproj ./working-good.business.api/
RUN dotnet restore ./working-good.business.api/working-good.business.api.csproj -s https://api.nuget.org/v3/index.json 
COPY . ./
RUN dotnet publish ./working-good.business.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "working-good.business.api.dll"]

ENV ASPNETCORE_ENVIRONMENT="Development"
ENV TZ="Europe/Warsaw"
EXPOSE 80
EXPOSE 443

