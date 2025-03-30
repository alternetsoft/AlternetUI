# https://ccifra.github.io/PortingWPFAppsToLinux/Overview.html

sudo dpkg --add-architecture i386
sudo apt update

sudo mkdir -pm755 /etc/apt/keyrings
sudo wget -O /etc/apt/keyrings/winehq-archive.key https://dl.winehq.org/wine-builds/winehq.key
sudo apt-key add /etc/apt/keyrings/winehq-archive.key
sudo apt-add-repository "deb https://dl.winehq.org/wine-builds/ubuntu/ $(lsb_release -cs) main"
sudo apt update


sudo apt install --install-recommends winehq-stable

sudo apt-get -y install winetricks