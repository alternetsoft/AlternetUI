@echo off
set IMAGE_NAME=cpp-builder
set CONTAINER_NAME=temp-container
set OUTPUT_DIR=%CD%\output

echo Cleaning up previous output...
rmdir /s /q "%OUTPUT_DIR%"
mkdir "%OUTPUT_DIR%"

echo Building Docker image...
docker build -t %IMAGE_NAME% .

echo Creating temporary container...
docker create --name %CONTAINER_NAME% %IMAGE_NAME%

echo Copying compiled library to the host...
docker cp %CONTAINER_NAME%:/app/output/libmylibrary.so "%OUTPUT_DIR%"

echo Cleaning up container...
docker rm -f %CONTAINER_NAME%

echo Build completed. Executable is available in %OUTPUT_DIR%

