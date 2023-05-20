#https://learn.microsoft.com/ru-ru/dotnet/core/install/linux-scripted-manual#scripted-install
#https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-3.1.426-linux-x64-binaries
#https://learn.microsoft.com/ru-ru/dotnet/core/install/linux-ubuntu#my-ubuntu-distribution-doesnt-include-the-net-version-i-want-or-i-need-an-out-of-support-net-version

sudo apt -y update
sudo apt -y upgrade 

sudo apt -y install dotnet-sdk-7.0
sudo apt -y install dotnet-sdk-6.0
sudo apt -y install dotnet-sdk-3.1
   

sudo apt-get -y install libgtk-3-dev 
sudo apt-get -y install build-essential gdb
sudo apt-get -y install cmake

sudo apt-get install -y freeglut3-dev libsecret-1-dev libgspell-1-dev 
sudo apt-get install -y libnotify-dev  libcurl4-openssl-dev libwebkit2gtk-4.0-dev 

sudo apt-get install -y libgstreamer1.0-dev libgstreamer-plugins-base1.0-dev 
sudo apt-get install -y libgstreamer-plugins-bad1.0-dev gstreamer1.0-plugins-base 
sudo apt-get install -y gstreamer1.0-plugins-good gstreamer1.0-plugins-bad gstreamer1.0-plugins-ugly 
sudo apt-get install -y gstreamer1.0-libav gstreamer1.0-tools gstreamer1.0-x gstreamer1.0-alsa 
sudo apt-get install -y gstreamer1.0-gl 
sudo apt-get install -y gstreamer1.0-gtk3 gstreamer1.0-qt5 gstreamer1.0-pulseaudio


sudo apt-get install -y libwxgtk-media3.0-gtk3 #wxWidgets Cross-platform C++ GUI toolkit (GTK 3 media library runtime)
sudo apt-get install -y libwxgtk-media3.0-gtk3-dev #wxWidgets Cross-platform C++ GUI toolkit (GTK 3 media library development)
sudo apt-get install -y libwxgtk-webview3.0-gtk3 #wxWidgets Cross-platform C++ GUI toolkit (GTK 3 webview library runtime)
sudo apt-get install -y libwxgtk-webview3.0-gtk3-dev #wxWidgets Cross-platform C++ GUI toolkit (GTK 3 webview library development)
sudo apt-get install -y libwxgtk3.0-gtk3 #wxWidgets Cross-platform C++ GUI toolkit (GTK 3 runtime)
sudo apt-get install -y libwxgtk3.0-gtk3-dev # wxWidgets Cross-platform C++ GUI toolkit (GTK 3 development)
