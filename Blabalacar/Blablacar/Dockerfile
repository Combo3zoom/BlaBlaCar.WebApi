FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Blabalacar/Blabalacar.csproj", "Blabalacar/"]
RUN dotnet restore "Blabalacar/Blabalacar.csproj"
COPY . .
WORKDIR "/src/Blabalacar"
RUN dotnet build "Blabalacar.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blabalacar.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blabalacar.dll"]
