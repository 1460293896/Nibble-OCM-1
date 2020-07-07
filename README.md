# Nibble-OCM 

<p align="center">
<img width="600" height="342" src="https://i.imgur.com/6tDCbYM.png">
</p>

One click miner GUI for xmrig and xmr-stak to specifically mine [Nibble](https://nibble-nibble.com)

## What this program does
Nibble-OCM is a GUI for the cryptonight mining software `xmrig` by @psychocrypt `xmr-stak` by @fireice-uk  

## Features
* Mine to your personal address!
* Automatic selection of pools according to their ping, payout threshold and more parameters
* Integrated pool statistics (partly implemented)
* Show stats at specified refresh rate
* Specify usage of CPU/GPU
* Some advanced settings

## How to build
Download the repository and extract it. Open the .sln file with a recent version of Visual Studio (Community Edition / make sure you have C# packages installed). Build the project using the green run button or "Build Solution" in the Build menu. You'll find the binaries inside the project folder in the directory `bin/debug/` or `bin/release/`. Copy `Nibble-OCM.exe` to your preferred directory and make sure to have the miner executables in the same folder.

## Important
* Mine at your own risk concerning your hardware
* Refer to [xmr-stak](https://github.com/fireice-uk/xmr-stak) and [xmrig](https://github.com/xmrig)

## How to add a Pool
Fork this Project and edit the pools.json file. After that, make a Pull Request to our Repository. After a review we will add your pool.

## Credits
Coded by EncryptedUnicorn for Turtlecoin [turtlecoinOCM](https://github.com/encryptedunicorn/turtlecoinOCM)

Modified by The Nibble Developers, special thanks to Belorion/Seimax

