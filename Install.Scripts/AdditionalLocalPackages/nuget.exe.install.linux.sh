sudo apt update
sudo apt install mono-complete
wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
sudo mv nuget.exe /usr/local/bin/nuget
sudo chmod +x /usr/local/bin/nuget
mono /usr/local/bin/nuget

