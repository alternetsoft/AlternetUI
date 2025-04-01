# Use .NET SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -r win-x64 -o /app/out

# Use minimal runtime image (not required for EXE creation)
FROM busybox
WORKDIR /app
COPY --from=build /app/out /app/out