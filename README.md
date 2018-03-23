# Updater
Start with download information parameters and start the download.

# Parameters
1. ApplicationName with extension
2. Old Version // Style "X.X.X.X" == "Major.Minor.Build.Revision"
3. DownloadLinkUpdate
4. DownloadLinkUpdateXML
5. XMLTagNames (XMLTagNames have to be seperated by "|"; First TagName = Version (String); Second TagName = DownloadLink (String); If Second TagName is empty, DownloadLinkUpdate get's used for downloading)
6. Description
7. DownloadFolder

# Example
'.\Updater.exe' 'Updater.exe' '1.0.0.0' ' ' 'https://raw.githubusercontent.com/SebastianNetsch/Updater/master/Update.xml' 'UpdateVersion|UpdateDownloadLink' 'Update for Updater' '.'
