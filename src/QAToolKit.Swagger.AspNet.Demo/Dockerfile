#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/QAToolKit.Swagger.AspNet.Demo/QAToolKit.Swagger.AspNet.Demo.csproj", "src/QAToolKit.Swagger.AspNet.Demo/"]
RUN dotnet restore "src/QAToolKit.Swagger.AspNet.Demo/QAToolKit.Swagger.AspNet.Demo.csproj"
COPY . .
WORKDIR "/src/src/QAToolKit.Swagger.AspNet.Demo"
RUN dotnet build "QAToolKit.Swagger.AspNet.Demo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QAToolKit.Swagger.AspNet.Demo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QAToolKit.Swagger.AspNet.Demo.dll"]