FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ColoursAPI.csproj", "."]
RUN dotnet restore "./ColoursAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ColoursAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ColoursAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ColoursAPI.dll"]