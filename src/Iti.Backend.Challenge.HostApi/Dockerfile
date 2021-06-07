#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Iti.Backend.Challenge.HostApi/Iti.Backend.Challenge.HostApi.csproj", "Iti.Backend.Challenge.HostApi/"]
COPY ["Iti.Backend.Challenge.Application/Iti.Backend.Challenge.Application.csproj", "Iti.Backend.Challenge.Application/"]
COPY ["Iti.Backend.Challenge.Core/Iti.Backend.Challenge.Core.csproj", "Iti.Backend.Challenge.Core/"]
COPY ["Iti.Backend.Challenge.Contract/Iti.Backend.Challenge.Contract.csproj", "Iti.Backend.Challenge.Contract/"]
COPY ["Iti.Backend.Challenge.CrossCutting/Iti.Backend.Challenge.CrossCutting.csproj", "Iti.Backend.Challenge.CrossCutting/"]
COPY ["Iti.Backend.Challenge.Provider/Iti.Backend.Challenge.Provider.csproj", "Iti.Backend.Challenge.Provider/"]
RUN dotnet restore "Iti.Backend.Challenge.HostApi/Iti.Backend.Challenge.HostApi.csproj"
COPY . .
WORKDIR "/src/Iti.Backend.Challenge.HostApi"
RUN dotnet build "Iti.Backend.Challenge.HostApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Iti.Backend.Challenge.HostApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Iti.Backend.Challenge.HostApi.dll"]