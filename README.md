# DiscordImageDownloader
 Small and simple app to download and keep in sync images (or any other attachments) from Discord channels.
 
 ## Configuration (in progress)
 Configuration is done via *appsettings.json* files.
 
1. Create settings file.
   - Copy/paste default *appsettings.json* file and rename it to *appsettings.production.json*;
3. Configure authentication.
   - Open Discord in a browser, open developer tool and go to Network tab;
   - View some channels/scroll some messages until you see a row that starts with */messages?*;
   - Click on the row & find *authorization:* in *Request Headers* group;
   - Copy/paste the value into "Token" part of the settings file from (1);
   - From time to time you will need to redo these steps to update the token; 
4. Configure channels you want to keep in sync.
   - Use "Channels" block to configure the collection of channels that you want to keep in sync;
