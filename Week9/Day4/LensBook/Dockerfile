# ==========================================
# Stage 1: Build
# ==========================================

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY ["LensBook/LensBook.csproj", "LensBook/"]

RUN dotnet restore "LensBook/LensBook.csproj"

COPY . .

WORKDIR "/src/LensBook"

RUN dotnet publish "LensBook.csproj" \
    -c Release \
    -o /app/publish

# ==========================================
# Stage 2: Run
# ==========================================

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "LensBook.dll"]