# Desktop File Information

On Ubuntu (and most Linux systems), .desktop files are used to define how an application is launched 
and how it appears in menus.

## Location

You can create them in several locations depending on your goal:

### For Your User Only

Create the file in:

```~/.local/share/applications/```

This makes the shortcut available only to your user.

Ideal for custom launchers or portable apps.

### For All Users (System-Wide)

Create the file in:

```/usr/share/applications/```

Requires root permissions. Makes the launcher available to all users on the system.

### Temporary or Testing

You can also place a .desktop file on your desktop:

```~/Desktop/```

## You may need to mark it as executable:

```chmod +x ~/Desktop/myapp.desktop```

Some desktop environments (like GNOME) may hide it unless permissions are correct.

## Sample .desktop File

```sh
[Desktop Entry]
Name=MyApp
Exec=/home/sergiy/myapp/run.sh
Icon=/home/sergiy/myapp/icon.png
Type=Application
Categories=Utility;
Terminal=false
```

Save it as myapp.desktop in one of the locations above.

## What are categories in desktop file

In a .desktop file on Linux, the Categories= field defines where your application appears in the system's 
application menu. It helps desktop environments like GNOME, KDE, or XFCE organize apps into logical 
sections — such as "Development", "Graphics", or "Game".

You can list multiple categories, separated by semicolons (;).
The first category should be a Main Category (required for proper menu placement).
Additional categories provide fine-grained classification.

Categories=Development;IDE;Qt;

This example places the app under "Development" and tags it as an IDE built with Qt.

## Common Main Categories

Category	Description

- AudioVideo	Multimedia apps (players, editors)
- Development	IDEs, compilers, debuggers
- Education	Learning tools
- Game		Games of all types
- Graphics	Image editors, viewers
- Network	Browsers, chat clients
- Office	Word processors, spreadsheets
- Settings	System or user settings
- System	System tools like monitors or log viewers
- Utility	Small tools like calculators or text editors

You can find the full list of registered categories here 
https://specifications.freedesktop.org/menu-spec/latest/category-registry.html

## Category Tips

- Category names are case-sensitive.
- Always validate your .desktop file with command: ```desktop-file-validate myapp.desktop```
- If you use an incorrect or unrecognized category, your app may not appear in the menu.

## What are icon format supported

In a .desktop file on Linux, the Icon= field supports several image formats — but with some nuances depending 
on the desktop environment (GNOME, KDE, XFCE, etc.). Supported Icon Formats:

Format		Description

- .png		Most widely supported; ideal for transparency and scaling
- .svg		Scalable vector graphics; great for high-DPI but not supported everywhere
- .xpm		Legacy format; supported but outdated
- .ico		Rarely used; mostly for Windows compatibility
- No extension	If you specify just a name (e.g. myapp), it looks in icon themes

If you specify Icon=myapp, the system searches for myapp.png, myapp.svg, etc. in:

```~/.local/share/icons/```
```/usr/share/icons/``
```Theme directories (e.g. hicolor, Adwaita, Papirus)```

If you provide a full path: ```Icon=/home/sergiy/icons/myapp.png```, it uses that file directly.

- Use .png for broad compatibility.
- Prefer square icons (e.g. 48×48 or 256×256).
- Include transparent backgrounds for better theme integration.
- For theme-based icons, install into hicolor or your custom theme directory.

## What size of the icon I should prefer

The ideal icon size depends on where and how your .desktop file will be used, but here’s a quick guide to help you choose:

Recommended Icon Sizes for .desktop Files:

Use Case			Preferred Size		Notes

- Standard desktop launcher	48×48 pixels		Works well across most desktop environments
- High-DPI displays		128×128 or 256×256	Ensures crisp rendering on modern screens
- App menu integration		64×64 or 128×128	GNOME, KDE, XFCE scale icons based on resolution
- Icon themes			Multiple sizes		Include variants: 16×16, 32×32, 48×48, 128×128
- Full path in .desktop		Any size works		But stick to square .png or .svg for consistency

Suggestions:

- Use .png for raster icons and .svg for scalable vector icons.
- Keep icons square and centered with transparent backgrounds.
- If installing into a theme, place icons in appropriate subfolders like:
```/usr/share/icons/hicolor/48x48/apps/myapp.png```

## Is it possible to have desktop file in the same folder as my app

Yes, you can place a .desktop file in the same folder as your application, but with some important caveats:

What Happens If You Do:

The .desktop file won’t be recognized by the system menu (GNOME, KDE, etc.) unless it’s placed in a standard location like:
```~/.local/share/applications/``` (user-specific)
```/usr/share/applications/``` (system-wide)

However, if you double-click the .desktop file in your app folder (e.g. ~/myapp/myapp.desktop), it can still launch 
your app — especially if marked executable:

```chmod +x myapp.desktop```

On some desktop environments (like GNOME), you may need to right-click -> Allow Launching the first time.

Use Cases for Keeping It Local:

- Portable apps: You bundle the .desktop file with your app for users to copy or install manually.
- Testing: You’re developing and want to quickly launch without installing system-wide.
- Custom launchers: You use it in scripts or shortcuts without needing menu integration.

Tip: Use Absolute Paths

If the .desktop file is in the same folder as your app, make sure the Exec= and Icon= fields use absolute paths or relative 
paths that work from the file’s location.

## Sample MyApp.desktop File

```sh
[Desktop Entry]
Name=MyApp
Exec=./MyApp
Icon=./icon.png
Type=Application
Terminal=false
Categories=Utility;
```

Description:

- ```Exec=./MyApp``` : relative path to your executable
- ```Icon=./icon.png``` : relative path to your icon
- ```Terminal=false``` : set to true if your app needs a terminal

Tip: Use absolute paths if your app might be launched from outside the folder.

Make It Executable To allow launching with a double-click (on GNOME and others):

```chmod +x MyApp.desktop```

If the file is ignored or treated as plain text, right-click and choose "Allow Launching" (on GNOME desktop) 
or move it temporarily to ~/Desktop/.

Launcher Script for Convenience. Add a run.sh script:

```bash
bash
#!/bin/bash
cd "$(dirname "$0")"
./MyApp
```
Then set Exec=./run.sh in your .desktop file for consistent behavior.

