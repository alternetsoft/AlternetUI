# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o /app/out

# Use a minimal runtime image for deployment
FROM busybox
WORKDIR /app
COPY --from=build /app/out /app/out
