# Running MAUI apps on macOs

We have MAUI version of our Alternet.Editor product which is created using our Alternet.UI library.
In this article you can find short instructions on how to install MAUI on macOs and run apps there.

## Installation

- Install Net 8.0 from this [url](https://dotnet.microsoft.com/en-us/download/dotnet/8.0). 
You need to install version that is compatible with processor architecture of your pc (arm64 or x64).
- Install Xcode.
- Install Xcode Command Line Tools, using terminal command ```xcode-select --install```.
- Install MAUI workload, using terminal command ```dotnet workload install maui```.

## Running the application

- Open terminal and go to the folder with MAUI application.

- In order to build the application, use terminal command ```dotnet build```.

- In order to run the application, use terminal command ```dotnet run -f net8.0-maccatalyst```.

## Problems

- Net compiler sometimes shows errors during the compilation that some files are blocked. Just build or run the project
more times. One of the next builds will be successful.