#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#This stage creates an image called base, based on the asp.net image from docker hub. It has the runtime components. 
#This image exposes port 80, and sets the working dir to /app
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base 
WORKDIR /app
EXPOSE 80


#This stage creates an image called build, based on the asp.net SDK image from docker hub. It has all the tools needed to build the application.
#This image exposes port 80, and sets the working dir to /src
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CommandsService/CommandsService.csproj", "CommandsService/"]
RUN dotnet restore "CommandsService/CommandsService.csproj"
COPY . .
WORKDIR "/src/CommandsService"
RUN dotnet build "CommandsService.csproj" -c Release -o /app/build



FROM build AS publish
RUN dotnet publish "CommandsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommandsService.dll"]