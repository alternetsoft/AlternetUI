"C:\Program Files\Oracle\VirtualBox\VBoxManage" guestcontrol "Ubuntu 24.04 Server" run --exe "/bin/bash" --username vboxuser --password 1234 --wait-stdout --wait-stderr -- -c "%*"
