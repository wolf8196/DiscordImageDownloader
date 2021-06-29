sc delete "Discord Image Downloader"
sc create "Discord Image Downloader" start=auto binpath=%~dp0DiscordImageDownloader.exe
sc description "Discord Image Downloader" "Small and simple app to download images from Discord channels"
sc start "Discord Image Downloader"
pause