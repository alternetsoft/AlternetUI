# Use a base image with a C++ compiler
FROM ubuntu:24.10

# Install necessary build tools
RUN apt update && apt install -y build-essential cmake

# Set working directory
WORKDIR /app

# Copy source files into the container
COPY . .

# Run the build commands
RUN mkdir -p build && cd build && cmake .. && make

# Ensure the compiled library is available for extraction
RUN cp build/libmylibrary.so /app/output/libmylibrary.so