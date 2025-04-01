@echo off
set IMAGE_NAME=my-wpf-app
set CONTAINER_NAME=temp-build-container
set OUTPUT_DIR=%CD%\output

echo Cleaning up previous output...
rmdir /s /q "%OUTPUT_DIR%"
mkdir "%OUTPUT_DIR%"

echo Building Docker image...
docker build -t %IMAGE_NAME% .

echo Running container to perform build...
docker create --name %CONTAINER_NAME% %IMAGE_NAME%

echo Copying build results from container to host...
docker cp %CONTAINER_NAME%:/app/out "%OUTPUT_DIR%"

echo Cleaning up container...
docker rm -f %CONTAINER_NAME%

echo Build completed. Files are available in %OUTPUT_DIR%
