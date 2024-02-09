
rm -rf obj/*.*
rm -rf bin/*.*
rm -rf LocalPackages/*.nupkg
rm -rf LocalPackages/*.snupkg

pushd LocalPackages

wget "https://drive.google.com/uc?export=download&id=1JwhP5nZGqQ7K7YQE8dLwbsNUuvrsHbdP" -O "Alternet.UI.0.9.505-beta.nupkg"
wget "https://drive.google.com/uc?export=download&id=1YV8tVKQJ3FCucbkUuzNY91WISUkLw1pe" -O "Alternet.UI.0.9.505-beta.snupkg"
wget "https://drive.google.com/uc?export=download&id=1qTHthghA3M97O78xLEX_KoMlcEedWVl1" -O "Alternet.UI.Pal.0.9.505-beta.nupkg"

popd

read -rsp $'Press any key to continue...\n' -n1 key
