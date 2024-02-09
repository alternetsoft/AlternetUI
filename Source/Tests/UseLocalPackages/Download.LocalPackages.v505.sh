
rm -rf obj/*.*
rm -rf bin/*.*
rm -rf LocalPackages/*.nupkg
rm -rf LocalPackages/*.snupkg

pushd LocalPackages

wget "https://drive.google.com/uc?export=download&id=1JwhP5nZGqQ7K7YQE8dLwbsNUuvrsHbdP"
wget "https://drive.google.com/uc?export=download&id=1YV8tVKQJ3FCucbkUuzNY91WISUkLw1pe"
wget "https://drive.google.com/uc?export=download&id=1qTHthghA3M97O78xLEX_KoMlcEedWVl1"

popd

read -rsp $'Press any key to continue...\n' -n1 key