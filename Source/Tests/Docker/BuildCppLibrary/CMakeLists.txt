
cmake_minimum_required(VERSION 3.10)
project(MyCppLibrary)

set(CMAKE_CXX_STANDARD 17)

# Specify source and header files
set(SOURCES math_operations.cpp)
set(HEADERS math_operations.h)

# Create a shared library
add_library(mylibrary SHARED ${SOURCES} ${HEADERS})

# Specify the output location
set_target_properties(mylibrary PROPERTIES OUTPUT_NAME "mylibrary")

