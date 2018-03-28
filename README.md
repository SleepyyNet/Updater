# Updater
Start with download information parameters and start the download.

# Parameters
-l | --logpath
---
Change the default logpath and creates the directory

-s | --silent
---
Activate the silent-mode

-a | --appname
---
ApplicationName with extension (required)

-v | --version
---
Version // Style "X.X.X.X" == "Major.Minor.Build.Revision" (required)

-d | --directlink
---
Direct download link (required if no -xs specified)

-dx | --xmllink
---
Xml download link (required)

-tx | --xmltags
---
Xml tag names (required)

-xf | --xmlfirst
---
First TagName = Version (String) requires -tx (required)

-xs | --xmlsecond
---
Second TagName = DownloadLink (String) requires -tx (required if no -d specified)

-c | --comment
---
Description (required)

-f | --folder
---
DownloadFolder (required)

# Example
'.\Updater.exe' '-a' 'Updater.exe' '-v' '1.0.0.0' '-dx' 'https://raw.githubusercontent.com/SebastianNetsch/Updater/master/Update.xml' '-tx -xf' 'UpdateVersion' '-xs' 'UpdateDownloadLink' '-c' 'Update for Updater' '-f' '.' '-l' '.\log\Updater.log'
