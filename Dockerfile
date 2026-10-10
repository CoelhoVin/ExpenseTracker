FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY ["ExpenseTracker.API/ExpenseTracker.API.csproj", "ExpenseTracker.API/"]
COPY ["ExpenseTracker.Application/ExpenseTracker.Application.csproj", "ExpenseTracker.Application/"]
COPY ["ExpenseTracker.Domain/ExpenseTracker.Domain.csproj", "ExpenseTracker.Domain/"]
COPY ["ExpenseTracker.Infrastructure/ExpenseTracker.Infrastructure.csproj", "ExpenseTracker.Infrastructure/"]

RUN dotnet restore "ExpenseTracker.API/ExpenseTracker.API.csproj"

COPY . .

WORKDIR "/src/ExpenseTracker.API"

RUN dotnet publish "ExpenseTracker.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ExpenseTracker.API.dll"]