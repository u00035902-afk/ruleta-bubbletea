FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build 
WORKDIR /src 
COPY ["RuletanBublee.csproj", "./"] 
RUN dotnet restore "RuletanBublee.csproj" 
COPY . . 
RUN dotnet build "RuletanBublee.csproj" -c Release -o /app/build 
RUN dotnet publish "RuletanBublee.csproj" -c Release -o /app/publish /p:UseAppHost=false 
 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final 
WORKDIR /app 
EXPOSE 8080 
ENV ASPNETCORE_URLS=http://+:8080 
COPY --from=build /app/publish . 
ENTRYPOINT ["dotnet", "RuletanBublee.dll"]
