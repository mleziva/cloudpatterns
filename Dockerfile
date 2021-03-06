FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
WORKDIR /src
COPY ["CloudPatterns/CloudPatterns.csproj", "CloudPatterns/"]
RUN dotnet restore "CloudPatterns/CloudPatterns.csproj"
COPY . .
WORKDIR "/src/CloudPatterns"
RUN dotnet build "CloudPatterns.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudPatterns.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudPatterns.dll"]